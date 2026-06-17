using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // variables
    public static GameManager Instance; // makes sure theres only one

    [SerializeField] private int wavesCompleted = 0;
    [SerializeField] private int currentWave = 1;
    private int playerPP = 0;
    private WaveManager waveManager;
    private PlayerHealth playerHealth;

    public float gunDamage       = 1f;
    public float gunFireRate     = 0.15f;
    public float swordDamage     = 2f;
    public float swordAttackRate = 0.3f;
    public float staffDamage     = 3f;
    public int   staffChainCount = 1;

    public int gunUpgradeLevel   = 0;
    public int swordUpgradeLevel = 0;
    public int staffUpgradeLevel = 0;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        FindRefs();
        SyncStatsToWeapons();
        waveManager.StartWave(1);
    }

    private void FindRefs()
    {
        waveManager  = FindAnyObjectByType<WaveManager>();
        playerHealth = FindAnyObjectByType<PlayerHealth>();
    }

    private void SyncStatsToWeapons()
    {
        GunWeapon gun = FindAnyObjectByType<GunWeapon>();
        if (gun != null)
        {
            gun.damage   = gunDamage;
            gun.fireRate = gunFireRate;
        }

        SwordWeapon sword = FindAnyObjectByType<SwordWeapon>();
        if (sword != null)
        {
            sword.damage     = swordDamage;
            sword.attackRate = swordAttackRate;
        }

        ElectricStaffWeapon staff = FindAnyObjectByType<ElectricStaffWeapon>();
        if (staff != null)
        {
            staff.damage     = staffDamage;
            staff.chainCount = staffChainCount;
        }
    }

    public void WaveComplete() // when wave complies
    {
        wavesCompleted++; // add one to wave 
        if (AudioManager.Instance != null) AudioManager.Instance.PlayWaveClear(); 
        if (playerHealth != null) playerHealth.HealFull(); // heal to fyull hp
        if (wavesCompleted % 10 == 0)
            OpenShop(); // openms shop at 10 wave intervals
        else
            Invoke(nameof(StartNextWave), 3f);
    }

    private void StartNextWave() // go to next way, increase counter
    {
        if (waveManager == null) waveManager = FindAnyObjectByType<WaveManager>();
        if (waveManager == null) return;
        currentWave = wavesCompleted + 1;
        waveManager.StartWave(currentWave);
    }

    private void OpenShop()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public void CloseShop() // bye bye shop
    {
        SceneManager.sceneLoaded += OnGameSceneLoaded;
        SceneManager.LoadScene("GameScene");
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnGameSceneLoaded;
        FindRefs();
        SyncStatsToWeapons();
        currentWave = wavesCompleted + 1;
        waveManager.StartWave(currentWave);
    }

    public void GameOver() // restart 
    {
        Instance = null;
        Destroy(gameObject);
        SceneManager.LoadScene("GameScene");
    }

    public void AddPP(int amount) { playerPP += amount; }  // add money 

    public bool SpendPP(int amount)
    {
        if (playerPP < amount) return false;
        playerPP -= amount;
        return true;
    }

    public int GetPP()             => playerPP;
    public int GetWavesCompleted() => wavesCompleted;
    public int GetCurrentWave()    => currentWave;
}