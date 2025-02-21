using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private float damageRate = 1.0f; // Tiempo en segundos entre daños consecutivos

    private CharacterController characterController;
    private float lastDamageTime = -1f;

    [SerializeField] private AudioSource DamageSound;
    [SerializeField] private AudioSource HealSound;

    [SerializeField] private AudioSource FailSound;

    [SerializeField] private GameObject failedScreen;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (failedScreen != null)
        {
            failedScreen.SetActive(false);
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Damager" && CanTakeDamage())
        {
            TakeDamage(10);
            lastDamageTime = Time.time;
        }
        else if (hit.gameObject.tag == "Heals")
        {
            Heal(20);
            Destroy(hit.gameObject);
        }
    }

    private bool CanTakeDamage()
    {
        return (Time.time - lastDamageTime >= damageRate);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        DamageSound?.Play();
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        HealSound?.Play();
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }
    private IEnumerator LoadSceneAfterDefeat()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Menu");
    }


    private void Die()
    {
        if (failedScreen != null)
        {
            failedScreen.SetActive(true);
        }

        FailSound?.Play();

        StartCoroutine(LoadSceneAfterDefeat());
    }

}
