using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float damage;
    private float lifetime = 3f;

    public void Init(Vector2 dir, float spd, float dmg)
    {
        direction = dir;
        speed = spd;
        damage = dmg;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}