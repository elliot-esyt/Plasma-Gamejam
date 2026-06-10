using UnityEngine;

public enum EnemyType { Light, Normal, Tank, Boss }

public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyType enemyType;

    private float moveSpeed;
    private float currentHealth;
    private float damage;
    private Transform player;

    private void Start()
    {
        switch (enemyType)
        {
            case EnemyType.Light:  moveSpeed = 4f;   currentHealth = 2f;  damage = 1f; break;
            case EnemyType.Normal: moveSpeed = 3f;   currentHealth = 5f;  damage = 2f; break;
            case EnemyType.Tank:   moveSpeed = 1.5f; currentHealth = 15f; damage = 1f; break;
            case EnemyType.Boss:   moveSpeed = 1f;   currentHealth = 50f; damage = 5f; break;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null) return;
        Vector3 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            WaveManager wm = FindObjectOfType<WaveManager>();
            if (wm != null) wm.EnemyKilled();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Damage attemtpeda");
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(damage);
        }
    }
}