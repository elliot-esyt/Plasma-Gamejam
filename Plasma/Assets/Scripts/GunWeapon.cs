using UnityEngine;

public class GunWeapon : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 12f;

    public float damage = 1f;
    public float fireRate = 0.15f;
    private float cooldown = 0f;

    private void Update()
    {
        if (cooldown > 0f) cooldown -= Time.deltaTime;
    }

    public void TryAttack(Vector2 direction)
    {
        if (cooldown > 0f || bulletPrefab == null) return;
        cooldown = fireRate;
        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bullet = b.GetComponent<Bullet>();
        if (bullet != null) bullet.Init(direction, bulletSpeed, damage);
    }
}