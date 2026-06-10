using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float currentHealth;
    private SpriteRenderer sr;

    private void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("taken damage, new hp:" + currentHealth);
        if (sr != null) StartCoroutine(DamageFlash());
        if (currentHealth <= 0)
            GameManager.Instance.GameOver();
    }

    public void HealFull()
    {
        currentHealth = maxHealth;
    }

    private IEnumerator DamageFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sr.color = Color.white;
    }

    public float GetHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
}