using System.Collections;
using UnityEngine;
using Cinemachine;


public class PlayerHealth : MonoBehaviour
{
    // variables
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float currentHealth;
    private SpriteRenderer sr;
    private CinemachineImpulseSource impulseSource;


    private void Start()
    {
        currentHealth = maxHealth; // on the start set their current hp to the max hp
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>(); 

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void TakeDamage(float amount) // take damage
    {
        currentHealth -= amount; // remove damage from their health 
        // Debug.Log("taken damage, new hp:" + currentHealth); // wait this is usefull
        CameraShakeManager.instance.CameraShake(impulseSource); // shake the camera
        if (sr != null) StartCoroutine(DamageFlash()); // set it red rq 
        if (currentHealth <= 0) // game over! heh... bye bye!
            GameManager.Instance.GameOver(); // kill player if hp = 0
    }

    public void HealFull() // heal full on wave complete
    {
        currentHealth = maxHealth;
        // currentHealth++;
    }

    private IEnumerator DamageFlash() // set red
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sr.color = Color.white;
    }

    public float GetHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
}