using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilFunctions
{
    // Definici�n de una matriz de transformaci�n est�tica _isoMatrix
    // Esta matriz aplica una rotaci�n para convertir coordenadas de un sistema
    // de coordenadas est�ndar a un sistema de coordenadas isom�tricas
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    // Definici�n de un m�todo de extensi�n est�tico para el tipo Vector3
    // El modificador 'this' indica que este m�todo se aplicar� a instancias de Vector3
    public static Vector3 toIso(this Vector3 input) =>
        // Se multiplica el vector de entrada (input) por la matriz de transformaci�n isom�trica (_isoMatrix)
        _isoMatrix.MultiplyPoint3x4(input);
}
