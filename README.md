# Aviation Security AI Monitor ✈️🛡️

System do monitorowania integralności danych lotniczych standardu ADS-B oraz wykrywania ataków typu spoofing przy użyciu sztucznej inteligencji.

## 📋 O Projekcie
Projekt został zrealizowany jako część pracy dyplomowej na kierunku **Organizacja i Sterowanie Ruchem Lotniczym**. Głównym celem jest automatyczna identyfikacja niefizycznych parametrów lotu, które mogą sugerować awarię czujników lub celową manipulację danymi (cyberataki).

## 🚀 Funkcjonalności
- **Wizualizacja Mapowa:** Dynamiczny radar lotów oparty na bibliotece GMap.NET.
- **Detekcja AI:** Wykorzystanie sieci neuronowej typu **Autoencoder** do analizy błędu rekonstrukcji (MSE).
- **Klasyfikacja Anomalii:** System identyfikuje konkretne parametry, które naruszają model fizyczny (Wysokość, Prędkość, Kurs).
- **Symulacja Ataków:** Możliwość wstrzykiwania syntetycznych błędów w celu walidacji skuteczności sieci.

## 🛠️ Technologie
- **Język:** C# (WinForms) dla interfejsu użytkownika.
- **AI/ML:** Python (TensorFlow/Keras) do trenowania modelu.
- **Standard danych:** ONNX (Open Neural Network Exchange) do integracji modelu AI z aplikacją C#.
- **Źródło danych:** Dane historyczne z serwisu The OpenSky Network.

## 📂 Struktura projektu
- `/Python_AI`: Skrypty do treningu modelu, normalizacji danych i eksportu do formatu .onnx.
- `/ADSB_Monitor_CSharp`: Główna aplikacja radarowa w C#.
- `model.onnx`: Wytrenowany model gotowy do pracy.

## ⚙️ Jak uruchomić?
1. Sklonuj repozytorium.
2. Upewnij się, że masz zainstalowany **.NET 6.0/8.0** oraz biblioteki NuGet: `GMap.NET` i `Microsoft.ML.OnnxRuntime`.
3. Skopiuj plik `model.onnx` do folderu `bin/Debug`.
4. Uruchom projekt w Visual Studio.

---
*Autor: Veronika Manokhina*
