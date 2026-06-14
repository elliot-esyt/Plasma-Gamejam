using UnityEngine;
using UnityEngine.InputSystem;

public enum WeaponType { Gun, Sword, ElectricStaff }

public class WeaponManager : MonoBehaviour
{
    // varialbes
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
        cam = Camera.main; // get the main cam 
        if (weaponSpriteRenderer != null)
            defaultWeaponLocalPos = weaponSpriteRenderer.transform.localPosition; // get defeault possition
        EquipWeapon(WeaponType.Gun); // player starts with a gun before sohp
    }

    private void Update()
    {
        var kb = Keyboard.current; // gets kb
        var mouse = Mouse.current; // gets mouse
        if (kb == null || mouse == null) return; // if the keyboard and mouse isnt here then what the hell

        if (kb.digit1Key.wasPressedThisFrame) EquipWeapon(WeaponType.Gun); // handles controls 
        if (kb.digit2Key.wasPressedThisFrame) EquipWeapon(WeaponType.Sword);
        if (kb.digit3Key.wasPressedThisFrame) EquipWeapon(WeaponType.ElectricStaff);

        if (mouse.leftButton.isPressed) // if left click 
        {
            switch (currentWeapon) // switch case handling the wewapon for 123
            {
                case WeaponType.Gun: 
                    Vector2 mouseWorld = cam.ScreenToWorldPoint(mouse.position.ReadValue()); // gets the mouse screen position into world position
                    Vector2 fireDir = (mouseWorld - (Vector2)transform.position).normalized; // finds direction
                    SnapWeaponToDir(fireDir); // points it there
                    gun.TryAttack(fireDir); // attacks using gun script
                    break;

                case WeaponType.Sword:
                    swordSpinAngle += swordSpinSpeed * Time.deltaTime; // increaes rotation
                    if (weaponPivot != null) 
                        weaponPivot.rotation = Quaternion.Euler(0f, 0f, swordSpinAngle); // rotates sword
                    sword.TryAttack(); // attacks using sword script
                    break;

                case WeaponType.ElectricStaff:
                    Vector2 staffDir = staff.TryAttack(); // direction to enemy hit
                    if (staffDir != Vector2.zero) SnapWeaponToDir(staffDir);  // rotate stuaff , refreences SnapWeaponToDir
                    break;
            }
        }
    }

    private void SnapWeaponToDir(Vector2 dir) // ok hi we are at SnapWeaponToDir
    {
        if (weaponPivot == null) return; 
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + weaponAngleOffset; // calculate angle 
        weaponPivot.rotation = Quaternion.Euler(0f, 0f, angle); // rotates 
        if (weaponSpriteRenderer != null) 
            weaponSpriteRenderer.flipY = dir.x < 0f; // flip sprite if it needs to be flipped
    }

    private void EquipWeapon(WeaponType type) 
    {
        currentWeapon = type; // store weapon 

        if (weaponSpriteRenderer != null) 
        {
            weaponSpriteRenderer.transform.localPosition = type == WeaponType.Sword // position adjustment depending on weapon 
                ? Vector3.zero
                : defaultWeaponLocalPos; 

            switch (type) // handles sprite changing 
            {
                case WeaponType.Gun:           weaponSpriteRenderer.sprite = gunSprite;   break;
                case WeaponType.Sword:         weaponSpriteRenderer.sprite = swordSprite; break;
                case WeaponType.ElectricStaff: weaponSpriteRenderer.sprite = staffSprite; break;
            }
        }
    }

    public WeaponType GetCurrentWeapon() => currentWeapon; // returns the equpped weapon
}