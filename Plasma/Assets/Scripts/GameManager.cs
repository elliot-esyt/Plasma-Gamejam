using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private int wavesCompleted = 0;
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
        waveManager.StartWave(wavesCompleted + 1);
    }

    public void CloseShop()
    {
        waveManager.StartWave(wavesCompleted + 1);
    }

    private void OpenShop()
    {
        Debug.Log("Shop opened! Waves completed: " + wavesCompleted);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetWavesCompleted() => wavesCompleted;
}