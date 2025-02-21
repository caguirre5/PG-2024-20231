using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 45.0f;
    [SerializeField] private float moveSpeed = 0.8f;
    [SerializeField] private float moveHeight = 0.1f;
    [SerializeField] private float followSpeed = 5.0f;
    [SerializeField] private float followDistance = 10.0f;

    private float originalY;
    private Transform playerTransform;

    void Start()
    {
        originalY = transform.position.y;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        RotateAndMoveVertical();
        FollowPlayer();
    }

    private void RotateAndMoveVertical()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        float newY = originalY + Mathf.Sin(Time.time * moveSpeed) * moveHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void FollowPlayer()
    {
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) < followDistance)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            directionToPlayer.y = 0; // Ignore vertical movement for horizontal follow
            transform.position += directionToPlayer * followSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Asume que quieres quitar 10 de vida
                Destroy(gameObject); // Destruye este objeto enemigo
            }
        }
    }
}
