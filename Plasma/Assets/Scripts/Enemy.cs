using UnityEngine;

public enum EnemyType { Light, Normal, Tank, Boss }

public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyType enemyType;

    private float moveSpeed;
    private float currentHealth;
    private float damage;
    private float damageCooldown = 0.5f;
    private float damageTimer = 0f;
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

        if (damageTimer > 0f) damageTimer -= Time.deltaTime;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            int ppReward = 0;
            switch (enemyType)
            {
                case EnemyType.Light:  ppReward = 2;  break;
                case EnemyType.Normal: ppReward = 5;  break;
                case EnemyType.Tank:   ppReward = 12; break;
                case EnemyType.Boss:   ppReward = 30; break;
            }
            GameManager.Instance.AddPP(ppReward);

            WaveManager wm = FindAnyObjectByType<WaveManager>();
            if (wm != null) wm.EnemyKilled();
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && damageTimer <= 0f)
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(damage);
            damageTimer = damageCooldown;
        }
    }
}