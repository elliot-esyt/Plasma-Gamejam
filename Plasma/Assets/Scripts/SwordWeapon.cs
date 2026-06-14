using UnityEngine;

public class SwordWeapon : MonoBehaviour
{

    // variables
    public float damage = 5f;
    public float radius = 2f;
    public float attackRate = 0.3f;
    private float cooldown = 0f;

    private void Update()
    {
        if (cooldown > 0f) cooldown -= Time.deltaTime; // cooldown management
    }

    public void TryAttack() // atckcing
    {
        if (cooldown > 0f) return; // cooldown 
        cooldown = attackRate;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius); // detect objects in a circle
        foreach (Collider2D hit in hits) // for each one hit
        {
            Enemy enemy = hit.GetComponent<Enemy>();// check for enemy
            if (enemy != null) enemy.TakeDamage(damage); // if its an enemy take damage
        }
    }
}