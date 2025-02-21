using UnityEngine;

public class ForestSensor : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip newClip;
    private AudioClip originalClip;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Guardar el clip original al iniciar
        if (audioSource != null)
        {
            originalClip = audioSource.clip;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.clip = originalClip;
            audioSource.Play();
        }
    }
}
