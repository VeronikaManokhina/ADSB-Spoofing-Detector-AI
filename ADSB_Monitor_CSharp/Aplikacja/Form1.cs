using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace Aplikacja
{
    public partial class Form1 : Form
    {
        private Bitmap iconGreen;
        private Bitmap iconRed;
        private Bitmap iconYellow;
        private GMapOverlay markersOverlay = new GMapOverlay("markers");
        private List<Aircraft> aircrafts = new List<Aircraft>();
        private InferenceSession session;
        private Random rnd = new Random();

        // lat, lon, velocity, heading, vertrate, baroaltitude
        float[] minVals = { -46.7024f, -177.8681f, 0.0f, 0.0f, -165.8112f, -304.8f };
        float[] maxVals = { 67.8869f, 177.6654f, 476.3421f, 359.8898f, 165.8112f, 38648.64f };

        public Form1()
        {
            InitializeComponent();
            InitMap();
            iconGreen = new Bitmap(new Bitmap("plane_green.png"), new Size(35, 35));
            iconRed = new Bitmap(new Bitmap("plane_red.png"), new Size(35, 35));
            iconYellow = new Bitmap(new Bitmap("plane_yellow.png"), new Size(35, 35));

            try
            {
                session = new InferenceSession("model.onnx");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd ładowania AI: " + ex.Message);
            }

            ClearPanel();

            CreateAircrafts();

        }

        private void ClearPanel()
        {
            lblICAO.Text = "IDENT: ---";
            lblAlt.Text = "ALT: ---";
            lblSpeed.Text = "VEL: ---";
            lblProb.Text = "RISK: 0%";
            lblIssue.Text = "ISSUE: None";
            pbSpoofing.Value = 0;
        }

        private void InitMap()
        {
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(52.23, 21.01);
            gMapControl1.Zoom = 8;
            gMapControl1.Overlays.Add(markersOverlay);

            gMapControl1.MinZoom = 2;              // Minimalne oddalenie
            gMapControl1.MaxZoom = 18;             // Maksymalne przybliżenie
            gMapControl1.MouseWheelZoomEnabled = true; // Włącza przybliżanie kółkiem myszy
            gMapControl1.CanDragMap = true;        // Pozwala przesuwać mapę
            gMapControl1.DragButton = MouseButtons.Left; // Przesuwanie lewym przyciskiem myszy

            gMapControl1.OnMarkerClick += GMapControl1_OnMarkerClick;
        }

        private void GMapControl1_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            var ac = aircrafts.FirstOrDefault(x => x.Marker == item);
            if (ac != null)
            {
                lblICAO.Text = "IDENT: " + ac.ID;
                lblAlt.Text = "ALT: " + ac.Alt.ToString("0") + " m";
                lblSpeed.Text = "VEL: " + ac.Velocity.ToString("0") + " m/s";
                lblIssue.Text = "ISSUE: " + ac.SuspectedParameter;
                lblProb.Text = "INTEGRITY RISK: " + ac.Probability.ToString("0") + "%";
                pbSpoofing.Value = (int)ac.Probability;

                // Zmiana koloru paska w zależności od ryzyka
                if (ac.Probability > 50) pbSpoofing.ForeColor = Color.Red;
                else if (ac.Probability > 25) pbSpoofing.ForeColor = Color.Yellow;
                else pbSpoofing.ForeColor = Color.Lime;

                pbSpoofing.Style = ProgressBarStyle.Continuous;
                pbSpoofing.Value = (int)ac.Probability;
                pbSpoofing.Maximum = 100;

            }

            lblICAO.ForeColor = (ac.Probability > 50) ? Color.Red : Color.Lime;
        }

        private void CreateAircrafts()
        {
            aircrafts.Clear();
            markersOverlay.Markers.Clear();

            // Losujemy ile samolotów ma być "atakiem" (od 1 do 3)
            int numSpoofed = rnd.Next(1, 4);

            for (int i = 0; i < 8; i++)
            {
                var ac = new Aircraft
                {
                    ID = "FLIGHT-" + rnd.Next(100, 999),
                    Lat = 52.23 + (rnd.NextDouble() - 0.5) * 1.5,
                    Lon = 21.01 + (rnd.NextDouble() - 0.5) * 1.5,
                    Heading = rnd.Next(0, 360),

                    // LOSOWE PARAMETRY:
                    Velocity = rnd.Next(180, 270),    // od 180 do 270 m/s
                    Alt = rnd.Next(7000, 12000),      // od 7 do 12 km
                    VertRate = rnd.Next(-2, 3),       // lekkie wznoszenie/opadanie

                    IsSpoofed = (i < numSpoofed)      // Pierwsze 1-3 samoloty będą atakami
                };

                if (ac.IsSpoofed)
                {
                    ac.AttackType = rnd.Next(0, 3); // Losujemy jeden z 3 rodzajów ataku
                }

                ac.Marker = new GMarkerGoogle(new PointLatLng(ac.Lat, ac.Lon), iconGreen);
                ac.Marker.ToolTipMode = MarkerTooltipMode.Never;
                markersOverlay.Markers.Add(ac.Marker);
                aircrafts.Add(ac);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timerRadar.Enabled = !timerRadar.Enabled;
            btnStart.Text = timerRadar.Enabled ? "STOP RADAR" : "START RADAR";
        }

        private void timerRadar_Tick(object sender, EventArgs e)
        {
            try
            {
                foreach (var ac in aircrafts)
                {
                    // 1. Ruch (fizyka)
                    UpdateAircraftPosition(ac);

                    // 2. Sieć neuronowa
                    float mse = RunInference(ac);

                    // 3. Kolorowanie
                    if (ac.Probability > 50) // CZERWONY: Spoofing
                    {
                        ac.Marker.Bitmap = RotateImage(iconRed, ac.Heading);
                        ac.Marker.ToolTipText = $"[CRITICAL]\nICAO: {ac.ID}\nProb: {ac.Probability:0}%\nIssue: {ac.SuspectedParameter}";
                        ac.Marker.ToolTipMode = MarkerTooltipMode.Never;
                    }
                    else if (ac.Probability > 25) // ŻÓŁTY: Niespójność (burza/awaria)
                    {
                        ac.Marker.Bitmap = RotateImage(iconYellow, ac.Heading);
                        ac.Marker.ToolTipText = $"[WARNING]\nICAO: {ac.ID}\nLow Integrity";
                        ac.Marker.ToolTipMode = MarkerTooltipMode.Never;
                    }
                    else // ZIELONY: OK
                    {
                        ac.Marker.Bitmap = RotateImage(iconGreen, ac.Heading);
                        ac.Marker.ToolTipText = $"ICAO: {ac.ID}\nStatus: Normal";
                        ac.Marker.ToolTipMode = MarkerTooltipMode.Never;
                    }
                }
                gMapControl1.Refresh();

            }
            catch (Exception ex)
            {
                timerRadar.Stop();
                MessageBox.Show("Błąd w ruchu: " + ex.Message);
            }
        }

        private float RunInference(Aircraft ac)
        {
            if (session == null) return 0;

            // Przygotowanie danych (Skalowanie 0-1)
            float[] inputs = new float[] {
            (float)((ac.Lat - minVals[0]) / (maxVals[0] - minVals[0])),
            (float)((ac.Lon - minVals[1]) / (maxVals[1] - minVals[1])),
            (ac.Velocity - minVals[2]) / (maxVals[2] - minVals[2]),
            (ac.Heading - minVals[3]) / (maxVals[3] - minVals[3]),
            (ac.VertRate - minVals[4]) / (maxVals[4] - minVals[4]),
            (ac.Alt - minVals[5]) / (maxVals[5] - minVals[5])
        };

            var inputTensor = new DenseTensor<float>(inputs, new int[] { 1, 6 });
            var inputNames = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor("input", inputTensor) };

            using (var results = session.Run(inputNames))
            {
                var output = results.First().AsEnumerable<float>().ToArray();
                string[] paramNames = { "Latitude", "Longitude", "Velocity", "Heading", "Vertical Rate", "Altitude" };

                float totalMse = 0;
                // Lista na wszystkie wykryte błędy
                List<string> issues = new List<string>();

                for (int i = 0; i < inputs.Length; i++)
                {
                    float error = (float)Math.Pow(inputs[i] - output[i], 2);
                    totalMse += error;

                    // Sprawdzamy, czy błąd konkretnego parametru jest duży (np. > 0.01)
                    // To pozwoli wykryć wiele błędów naraz
                    if (error > 0.01f)
                    {
                        issues.Add(paramNames[i]);
                    }
                }

                float finalMse = totalMse / inputs.Length;

                // CZUŁOŚĆ
                ac.Probability = Math.Min(100, finalMse * 20000);

                // Jeśli lista issues ma elementy, łączymy je po przecinku
                if (issues.Count > 0)
                {
                    ac.SuspectedParameter = string.Join(", ", issues);
                }
                else
                {
                    ac.SuspectedParameter = "None";
                }

                return finalMse;
            }
        }

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            timerRadar.Enabled = !timerRadar.Enabled;
            btnStart.Text = timerRadar.Enabled ? "STOP" : "START";
        }

        private Bitmap RotateImage(Bitmap b, float angle)
        {
            // Tworzenie pustego obrazka o tym samym rozmiarze
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                // Ustawienie punktu obrotu na środek obrazka
                g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
                // Obracanie
                g.RotateTransform(angle);
                // Przesuwanie z powrotem
                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
                // Rysowanie starego obrazka na nowym (już obróconym) miejscu
                g.DrawImage(b, new Point(0, 0));
            }
            return returnBitmap;
        }


        private void UpdateAircraftPosition(Aircraft ac)
        {
            try
            {
                // 1. RUCH PODSTAWOWY - musi wykonać się dla każdego samolotu
                // Bez tego samoloty stoją w miejscu.
                double rad = ac.Heading * Math.PI / 180.0;
                double moveStep = 0.005; // prędkość przesuwania się ikonki na mapie

                ac.Lat += Math.Cos(rad) * moveStep;
                ac.Lon += Math.Sin(rad) * moveStep;

                // 2. LOGIKA SPOOFINGU - tylko dla zaznaczonych jako IsSpoofed
                if (ac.IsSpoofed)
                {
                    if (ac.AttackType == 0) // Atak na prędkość
                    {
                        ac.Velocity -= 1.5f;
                        if (ac.Velocity < 10) ac.Velocity = 10;
                    }
                    else if (ac.AttackType == 1) // Atak na wysokość
                    {
                        ac.Alt += 80;
                        if (ac.Alt > 40000) ac.Alt = 40000;
                    }
                    else // Combo - typ 2
                    {
                        ac.Alt += 50;
                        ac.Velocity += 2;
                        ac.Heading += 1; // Samolot zaczyna powoli skręcać w kółko
                    }
                }

                // 3. AKTUALIZACJA MARKERA - też musi być dla każdego
                ac.Marker.Position = new PointLatLng(ac.Lat, ac.Lon);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Błąd w UpdateAircraftPosition: " + ex.Message);
            }
        }

    }

    public class Aircraft
    {
        public string ID { get; set; }
        public string Callsign { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public float Heading { get; set; }
        public float Velocity { get; set; }
        public float Alt { get; set; }
        public float VertRate { get; set; }

        public float Probability { get; set; } 
        public string SuspectedParameter { get; set; }
        public GMarkerGoogle Marker { get; set; }

        public bool IsSpoofed { get; set; }

        public int AttackType { get; set; } // 0 - prędkość, 1 - wysokość, 2 - combo
    }
}