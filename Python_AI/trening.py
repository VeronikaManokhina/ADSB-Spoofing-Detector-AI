import pandas as pd
import numpy as np
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Dense
from sklearn.preprocessing import MinMaxScaler
import joblib # Do zapisania skalera

# 1. Wczytanie danych
print("Wczytywanie danych...")
df = pd.read_csv("states_2022-06-27-23.csv")

# 2. Wybór cech (fizyka lotu)
features = ['lat', 'lon', 'velocity', 'heading', 'vertrate', 'baroaltitude']
data = df[features].dropna() # Usuwamy wiersze z pustymi danymi (NaN)

# 3. Skalowanie danych (sieć musi mieć dane w zakresie 0-1)
scaler = MinMaxScaler()
data_scaled = scaler.fit_transform(data)

# Zapisujemy skaler
joblib.dump(scaler, 'skaler.pkl')

# 4. Budowa prostego Autoencodera (3 warstwy: Wejście -> Środek -> Wyjście)
model = Sequential([
    # Warstwa 1: wejście (6 parametrów) -> Kompresja do 3 (Wąskie gardło)
    Dense(12, activation='relu', input_shape=(len(features),)),
    Dense(3, activation='relu'), # Warstwa 2: sieć ściska wiedzę
    # Warstwa 3: rozprężanie z powrotem do 6 parametrów
    Dense(12, activation='relu'),
    Dense(len(features), activation='linear')
])

model.compile(optimizer='adam', loss='mse')

# 5. Trening (uczymy sieć kopiowania samej siebie)
print("Trening w toku...")
model.fit(data_scaled, data_scaled, epochs=20, batch_size=32, verbose=1)

# 6. Zapisanie modelu
model.save('model_lotow.h5')
print("Sukces! Model 'model_lotow.h5' i 'skaler.pkl' zostały zapisane.")