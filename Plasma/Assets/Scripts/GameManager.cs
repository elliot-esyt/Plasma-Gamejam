using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int wavesCompleted = 0;
    private int currentWave = 1;
    private int playerPP = 0;
    private WaveManager waveManager;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        waveManager = FindAnyObjectByType<WaveManager>();
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        waveManager.StartWave(1);
    }

    public void WaveComplete()
    {
        wavesCompleted++;
        if (playerHealth != null) playerHealth.HealFull();
        if (wavesCompleted % 10 == 0)
            OpenShop();
        else
            Invoke(nameof(StartNextWave), 3f);
    }

    private void StartNextWave()
    {
        currentWave = wavesCompleted + 1;
        waveManager.StartWave(currentWave);
    }

    public void CloseShop()
    {
        currentWave = wavesCompleted + 1;
        waveManager.StartWave(currentWave);
    }

    private void OpenShop()
    {
        Debug.Log("Shop opened! Waves completed: " + wavesCompleted);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddPP(int amount)
    {
        playerPP += amount;
    }

    public int GetPP() => playerPP;
    public int GetWavesCompleted() => wavesCompleted;
    public int GetCurrentWave() => currentWave;
}