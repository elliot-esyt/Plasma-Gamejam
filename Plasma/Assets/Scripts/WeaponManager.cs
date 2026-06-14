using UnityEngine;
using UnityEngine.InputSystem;

public enum WeaponType { Gun, Sword, ElectricStaff }

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GunWeapon gun;
    [SerializeField] private SwordWeapon sword;
    [SerializeField] private ElectricStaffWeapon staff;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;
    [SerializeField] private Sprite gunSprite;
    [SerializeField] private Sprite swordSprite;
    [SerializeField] private Sprite staffSprite;
    [SerializeField] private float weaponAngleOffset = 0f;
    [SerializeField] private float swordSpinSpeed = 300f;

    private WeaponType currentWeapon = WeaponType.Gun;
    private Camera cam;
    private float swordSpinAngle = 0f;
    private Vector3 defaultWeaponLocalPos;

    private void Start()
    {
        cam = Camera.main;
        if (weaponSpriteRenderer != null)
            defaultWeaponLocalPos = weaponSpriteRenderer.transform.localPosition;
        EquipWeapon(WeaponType.Gun);
    }

    private void Update()
    {
        var kb = Keyboard.current;
        var mouse = Mouse.current;
        if (kb == null || mouse == null) return;

        if (kb.digit1Key.wasPressedThisFrame) EquipWeapon(WeaponType.Gun);
        if (kb.digit2Key.wasPressedThisFrame) EquipWeapon(WeaponType.Sword);
        if (kb.digit3Key.wasPressedThisFrame) EquipWeapon(WeaponType.ElectricStaff);

        if (mouse.leftButton.isPressed)
        {
            switch (currentWeapon)
            {
                case WeaponType.Gun:
                    Vector2 mouseWorld = cam.ScreenToWorldPoint(mouse.position.ReadValue());
                    Vector2 fireDir = (mouseWorld - (Vector2)transform.position).normalized;
                    SnapWeaponToDir(fireDir);
                    gun.TryAttack(fireDir);
                    break;

                case WeaponType.Sword:
                    swordSpinAngle += swordSpinSpeed * Time.deltaTime;
                    if (weaponPivot != null)
                        weaponPivot.rotation = Quaternion.Euler(0f, 0f, swordSpinAngle);
                    sword.TryAttack();
                    break;

                case WeaponType.ElectricStaff:
                    Vector2 staffDir = staff.TryAttack();
                    if (staffDir != Vector2.zero) SnapWeaponToDir(staffDir);
                    break;
            }
        }
    }

    private void SnapWeaponToDir(Vector2 dir)
    {
        if (weaponPivot == null) return;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + weaponAngleOffset;
        weaponPivot.rotation = Quaternion.Euler(0f, 0f, angle);
        if (weaponSpriteRenderer != null)
            weaponSpriteRenderer.flipY = dir.x < 0f;
    }

    private void EquipWeapon(WeaponType type)
    {
        currentWeapon = type;

        if (weaponSpriteRenderer != null)
        {
            weaponSpriteRenderer.transform.localPosition = type == WeaponType.Sword
                ? Vector3.zero
                : defaultWeaponLocalPos;

            switch (type)
            {
                case WeaponType.Gun:           weaponSpriteRenderer.sprite = gunSprite;   break;
                case WeaponType.Sword:         weaponSpriteRenderer.sprite = swordSprite; break;
                case WeaponType.ElectricStaff: weaponSpriteRenderer.sprite = staffSprite; break;
            }
        }
    }

    public WeaponType GetCurrentWeapon() => currentWeapon;
}