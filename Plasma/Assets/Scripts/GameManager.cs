using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int wavesCompleted = 0;
    public WaveManager waveManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        waveManager.StartWave(1);
    }

    public void WaveComplete()
    {
        wavesCompleted++;
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
        Debug.Log("shop opened, waves completed: " + wavesCompleted);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetWavesCompleted() => wavesCompleted;
}