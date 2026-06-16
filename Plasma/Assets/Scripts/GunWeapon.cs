using UnityEngine;

public class GunWeapon : MonoBehaviour
{
    // variables 
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 12f;

    public float damage = 1f;
    public float fireRate = 0.15f;
    private float cooldown = 0f;

    private void Update()
    {
        if (cooldown > 0f) cooldown -= Time.deltaTime; // cooldown
    }

    public void TryAttack(Vector2 direction) // attacking
    {
        if (cooldown > 0f || bulletPrefab == null) return; // checks cooldown
        cooldown = fireRate; // start coodlwon
        if (AudioManager.Instance != null) AudioManager.Instance.PlayGunShoot();
        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity); // create a bullet
        Bullet bullet = b.GetComponent<Bullet>(); // get the bullet script
        if (bullet != null) bullet.Init(direction, bulletSpeed, damage); // if bullet exists, initiallise it
    }
}