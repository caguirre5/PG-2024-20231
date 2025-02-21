using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilFunctions
{
    // Definición de una matriz de transformación estática _isoMatrix
    // Esta matriz aplica una rotación para convertir coordenadas de un sistema
    // de coordenadas estándar a un sistema de coordenadas isométricas
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    // Definición de un método de extensión estático para el tipo Vector3
    // El modificador 'this' indica que este método se aplicará a instancias de Vector3
    public static Vector3 toIso(this Vector3 input) =>
        // Se multiplica el vector de entrada (input) por la matriz de transformación isométrica (_isoMatrix)
        _isoMatrix.MultiplyPoint3x4(input);
}
