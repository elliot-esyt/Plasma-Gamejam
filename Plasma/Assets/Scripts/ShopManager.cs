using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Weapon Icons")]
    [SerializeField] private Transform gunIcon;
    [SerializeField] private Transform swordIcon;
    [SerializeField] private Transform staffIcon;

    [Header("Upgrade Buttons")]
    [SerializeField] private Button gunUpgradeButton;
    [SerializeField] private Button swordUpgradeButton;
    [SerializeField] private Button staffUpgradeButton;

    [Header("Button Label Text")]
    [SerializeField] private TMP_Text gunButtonText;
    [SerializeField] private TMP_Text swordButtonText;
    [SerializeField] private TMP_Text staffButtonText;

    [Header("Upgrade Info Text")]
    [SerializeField] private TMP_Text gunInfoText;
    [SerializeField] private TMP_Text swordInfoText;
    [SerializeField] private TMP_Text staffInfoText;

    [Header("PP Display")]
    [SerializeField] private TMP_Text ppText;

    [Header("Continue Button")]
    [SerializeField] private Button continueButton;

    [Header("Bobbing Settings")]
    [SerializeField] private float bobHeight = 0.3f;
    [SerializeField] private float bobSpeed = 2f;

    private Vector3 gunBasePos;
    private Vector3 swordBasePos;
    private Vector3 staffBasePos;

    private int[] upgradeCosts = { 20, 40, 70, 110, 160, 220, 290, 370, 460, 560 };

    private void Start()
    {
        if (gunIcon != null)   gunBasePos   = gunIcon.localPosition;
        if (swordIcon != null) swordBasePos = swordIcon.localPosition;
        if (staffIcon != null) staffBasePos = staffIcon.localPosition;

        if (gunUpgradeButton != null)   gunUpgradeButton.onClick.AddListener(UpgradeGun);
        if (swordUpgradeButton != null) swordUpgradeButton.onClick.AddListener(UpgradeSword);
        if (staffUpgradeButton != null) staffUpgradeButton.onClick.AddListener(UpgradeStaff);
        if (continueButton != null)     continueButton.onClick.AddListener(Continue);

        AddClickSound(gunUpgradeButton);
        AddClickSound(swordUpgradeButton);
        AddClickSound(staffUpgradeButton);
        AddClickSound(continueButton);

        RefreshUI();
    }

    private void AddClickSound(Button btn)
    {
        if (btn == null) return;
        btn.onClick.AddListener(() =>
        {
            if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonClick();
        });
    }

    private void Update()
    {
        float t = Time.time;
        if (gunIcon != null)
            gunIcon.localPosition   = gunBasePos   + Vector3.up * Mathf.Sin(t * bobSpeed)      * bobHeight;
        if (swordIcon != null)
            swordIcon.localPosition = swordBasePos + Vector3.up * Mathf.Sin(t * bobSpeed + 1f) * bobHeight;
        if (staffIcon != null)
            staffIcon.localPosition = staffBasePos + Vector3.up * Mathf.Sin(t * bobSpeed + 2f) * bobHeight;

        if (ppText != null && GameManager.Instance != null)
            ppText.text = "PP: " + GameManager.Instance.GetPP();
    }

    private void UpgradeGun()
    {
        if (GameManager.Instance == null) return;
        int cost = GetCost(GameManager.Instance.gunUpgradeLevel);
        if (!GameManager.Instance.SpendPP(cost)) return;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayBuy();
        GameManager.Instance.gunUpgradeLevel++;
        GameManager.Instance.gunDamage   += 1f;
        GameManager.Instance.gunFireRate  = Mathf.Max(0.05f, GameManager.Instance.gunFireRate - 0.01f);
        RefreshUI();
    }

    private void UpgradeSword()
    {
        if (GameManager.Instance == null) return;
        int cost = GetCost(GameManager.Instance.swordUpgradeLevel);
        if (!GameManager.Instance.SpendPP(cost)) return;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayBuy();
        GameManager.Instance.swordUpgradeLevel++;
        GameManager.Instance.swordDamage      += 1f;
        GameManager.Instance.swordAttackRate   = Mathf.Max(0.05f, GameManager.Instance.swordAttackRate - 0.02f);
        RefreshUI();
    }

    private void UpgradeStaff()
    {
        if (GameManager.Instance == null) return;
        int cost = GetCost(GameManager.Instance.staffUpgradeLevel);
        if (!GameManager.Instance.SpendPP(cost)) return;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayBuy();
        GameManager.Instance.staffUpgradeLevel++;
        GameManager.Instance.staffDamage += 2f;
        if (GameManager.Instance.staffUpgradeLevel % 2 == 0)
            GameManager.Instance.staffChainCount += 1;
        RefreshUI();
    }

    private void Continue()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.CloseShop();
    }

    private void RefreshUI()
    {
        if (GameManager.Instance == null) return;

        if (gunInfoText != null)
            gunInfoText.text   = "DMG: " + GameManager.Instance.gunDamage
                               + "  RATE: " + GameManager.Instance.gunFireRate.ToString("F2") + "s";

        if (swordInfoText != null)
            swordInfoText.text = "DMG: " + GameManager.Instance.swordDamage
                               + "  RATE: " + GameManager.Instance.swordAttackRate.ToString("F2") + "s";

        if (staffInfoText != null)
            staffInfoText.text = "DMG: " + GameManager.Instance.staffDamage
                               + "  CHAINS: " + (GameManager.Instance.staffChainCount + 1);

        int pp = GameManager.Instance.GetPP();
        SetButton(gunUpgradeButton,   gunButtonText,   GameManager.Instance.gunUpgradeLevel,   pp);
        SetButton(swordUpgradeButton, swordButtonText, GameManager.Instance.swordUpgradeLevel, pp);
        SetButton(staffUpgradeButton, staffButtonText, GameManager.Instance.staffUpgradeLevel, pp);
    }

    private void SetButton(Button btn, TMP_Text label, int level, int pp)
    {
        int cost = GetCost(level);
        if (label != null) label.text = "UPGRADE\n" + cost + " PP";
        if (btn != null)   btn.interactable = pp >= cost;
    }

    private int GetCost(int level)
    {
        if (level < upgradeCosts.Length) return upgradeCosts[level];
        return upgradeCosts[upgradeCosts.Length - 1] + (level - upgradeCosts.Length + 1) * 100;
    }
}