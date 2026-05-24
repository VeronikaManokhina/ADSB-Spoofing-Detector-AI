import tensorflow as tf
import tf2onnx
import joblib
import numpy as np

# 1. Ładujemy model i skaler
model = tf.keras.models.load_model('model_lotow.h5', compile=False)
scaler = joblib.load('skaler.pkl')

print("\n--- LICZBY DLA C# ---")
print(f"MIN: {list(scaler.data_min_)}")
print(f"MAX: {list(scaler.data_max_)}")
print("-------------------------------\n")

# 2. Omijamy błąd 'keras_tensor' przez stworzenie czystej funkcji TensorFlow
@tf.function(input_signature=[tf.TensorSpec([None, 6], tf.float32, name="input")])
def serving_default(input_tensor):
    return model(input_tensor, training=False)

# 3. Konwersja z funkcji
model_proto, _ = tf2onnx.convert.from_function(
    serving_default, 
    input_signature=[tf.TensorSpec([None, 6], tf.float32, name="input")],
    output_path="model.onnx"
)

print("SUKCES! Plik model.onnx jest gotowy.")