using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 45.0f; // Velocidad de rotación en grados por segundo
    [SerializeField] private float moveSpeed = 0.8f; // Velocidad del movimiento vertical
    [SerializeField] private float moveHeight = 0.1f; // Amplitud del movimiento vertical

    private float originalY; // Posición Y original del objeto

    void Start()
    {
        originalY = transform.position.y; // Guarda la posición Y original al inicio
    }

    void Update()
    {
        // Rota el objeto en el eje Y
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Mueve el objeto hacia arriba y abajo
        float newY = originalY + Mathf.Sin(Time.time * moveSpeed) * moveHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
