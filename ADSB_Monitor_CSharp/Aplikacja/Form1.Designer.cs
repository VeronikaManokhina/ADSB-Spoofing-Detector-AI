namespace Aplikacja
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            btnStart = new Button();
            timerRadar = new System.Windows.Forms.Timer(components);
            panel1 = new Panel();
            pbSpoofing = new ProgressBar();
            lblProb = new Label();
            lblICAO = new Label();
            lblIssue = new Label();
            lblSpeed = new Label();
            lblAlt = new Label();
            panel2 = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // gMapControl1
            // 
            gMapControl1.Bearing = 0F;
            gMapControl1.CanDragMap = true;
            gMapControl1.Dock = DockStyle.Fill;
            gMapControl1.EmptyTileColor = Color.Navy;
            gMapControl1.GrayScaleMode = false;
            gMapControl1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            gMapControl1.LevelsKeepInMemory = 5;
            gMapControl1.Location = new Point(0, 0);
            gMapControl1.MarkersEnabled = true;
            gMapControl1.MaxZoom = 2;
            gMapControl1.MinZoom = 2;
            gMapControl1.MouseWheelZoomEnabled = true;
            gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            gMapControl1.Name = "gMapControl1";
            gMapControl1.NegativeMode = false;
            gMapControl1.PolygonsEnabled = true;
            gMapControl1.RetryLoadTile = 0;
            gMapControl1.RoutesEnabled = true;
            gMapControl1.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            gMapControl1.SelectedAreaFillColor = Color.FromArgb(33, 65, 105, 225);
            gMapControl1.ShowTileGridLines = false;
            gMapControl1.Size = new Size(1078, 652);
            gMapControl1.TabIndex = 0;
            gMapControl1.Zoom = 0D;
            // 
            // btnStart
            // 
            btnStart.Font = new Font("BankGothic Lt BT", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnStart.Location = new Point(35, 254);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 75);
            btnStart.TabIndex = 1;
            btnStart.Text = "START";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click_1;
            // 
            // timerRadar
            // 
            timerRadar.Interval = 50;
            timerRadar.Tick += timerRadar_Tick;
            // 
            // panel1
            // 
            panel1.Controls.Add(pbSpoofing);
            panel1.Controls.Add(lblProb);
            panel1.Controls.Add(lblICAO);
            panel1.Controls.Add(lblIssue);
            panel1.Controls.Add(lblSpeed);
            panel1.Controls.Add(lblAlt);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(780, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(298, 652);
            panel1.TabIndex = 2;
            // 
            // pbSpoofing
            // 
            pbSpoofing.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pbSpoofing.Location = new Point(95, 268);
            pbSpoofing.Name = "pbSpoofing";
            pbSpoofing.Size = new Size(125, 44);
            pbSpoofing.TabIndex = 6;
            // 
            // lblProb
            // 
            lblProb.AutoSize = true;
            lblProb.Font = new Font("BankGothic Lt BT", 13.8F);
            lblProb.Location = new Point(14, 212);
            lblProb.Name = "lblProb";
            lblProb.Size = new Size(93, 24);
            lblProb.TabIndex = 8;
            lblProb.Text = "label2";
            // 
            // lblICAO
            // 
            lblICAO.AutoSize = true;
            lblICAO.Font = new Font("BankGothic Lt BT", 13.8F);
            lblICAO.Location = new Point(14, 22);
            lblICAO.Name = "lblICAO";
            lblICAO.Size = new Size(93, 24);
            lblICAO.TabIndex = 3;
            lblICAO.Text = "label1";
            // 
            // lblIssue
            // 
            lblIssue.AutoSize = true;
            lblIssue.Font = new Font("BankGothic Lt BT", 13.8F);
            lblIssue.Location = new Point(14, 162);
            lblIssue.Name = "lblIssue";
            lblIssue.Size = new Size(93, 24);
            lblIssue.TabIndex = 7;
            lblIssue.Text = "label1";
            // 
            // lblSpeed
            // 
            lblSpeed.AutoSize = true;
            lblSpeed.Font = new Font("BankGothic Lt BT", 13.8F);
            lblSpeed.Location = new Point(14, 64);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(93, 24);
            lblSpeed.TabIndex = 4;
            lblSpeed.Text = "label2";
            // 
            // lblAlt
            // 
            lblAlt.AutoSize = true;
            lblAlt.Font = new Font("BankGothic Lt BT", 13.8F);
            lblAlt.Location = new Point(14, 117);
            lblAlt.Name = "lblAlt";
            lblAlt.Size = new Size(93, 24);
            lblAlt.TabIndex = 5;
            lblAlt.Text = "label3";
            // 
            // panel2
            // 
            panel2.Controls.Add(btnStart);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(165, 652);
            panel2.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1078, 652);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(gMapControl1);
            Name = "Form1";
            Text = "Spoofing detection";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gMapControl1;
        private Button btnStart;
        private System.Windows.Forms.Timer timerRadar;
        private Panel panel1;
        private Label lblICAO;
        private Label lblSpeed;
        private Label lblAlt;
        private ProgressBar pbSpoofing;
        private Label lblIssue;
        private Label lblProb;
        private Panel panel2;
    }
}
