using UnityEngine;

public class SwordWeapon : MonoBehaviour
{
    public float damage = 5f;
    public float radius = 2f;
    public float attackRate = 0.3f;
    private float cooldown = 0f;

    private void Update()
    {
        if (cooldown > 0f) cooldown -= Time.deltaTime;
    }

    public void TryAttack()
    {
        if (cooldown > 0f) return;
        cooldown = attackRate;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null) enemy.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}