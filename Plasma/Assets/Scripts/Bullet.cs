using UnityEngine;

public class Bullet : MonoBehaviour
{

    // variables
    private Vector2 direction;
    private float speed;
    private float damage;
    private float lifetime = 3f;

    public void Init(Vector2 dir, float spd, float dmg) // initialises the bullet
    {
        direction = dir;
        speed = spd;
        damage = dmg;
    }

    private void Update() 
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World); // moves the bullet 
        lifetime -= Time.deltaTime; // each frame the time its alive goes down
        if (lifetime <= 0f) Destroy(gameObject); // if its been alive for more than 3s delete the bul,let
    }

    private void OnTriggerEnter2D(Collider2D other) // if collides
    {
        Enemy enemy = other.GetComponent<Enemy>(); // sees if its an enemy
        if (enemy != null)  // if it is an enemy
        {
            enemy.TakeDamage(damage); // take dmg
            Destroy(gameObject); // kill the bullet sprite
        }
    }
}