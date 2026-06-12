using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // variables
    public static GameManager Instance; // makes sure theres only one

    private int wavesCompleted = 0;
    private int currentWave = 1;
    private int playerPP = 0;
    private WaveManager waveManager;
    private PlayerHealth playerHealth;


    private void Awake() 
    {
        if (Instance == null) Instance = this; // make sure theres only one of these fellas
        else Destroy(gameObject);
    }

    private void Start()
    {
        waveManager = FindAnyObjectByType<WaveManager>(); // looks for the managers
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        waveManager.StartWave(1);
    }

    public void WaveComplete() // when wave complies
    {
        wavesCompleted++; // add one to wave 
        if (playerHealth != null) playerHealth.HealFull(); // heal to fyull hp
        if (wavesCompleted % 10 == 0)
            OpenShop(); // openms shop at 10 wave intervals
        else
            Invoke(nameof(StartNextWave), 3f); 
    }

    private void StartNextWave() // go to next way, increase counter
    {
        currentWave = wavesCompleted + 1;
        waveManager.StartWave(currentWave);
    }

    public void CloseShop() // bye bye shop
    {
        currentWave = wavesCompleted + 1;
        waveManager.StartWave(currentWave);
    }

    private void OpenShop() // need to do, currently debug log
    {
        Debug.Log("Shop opened! Waves completed: " + wavesCompleted);
    }

    public void GameOver() // restart 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddPP(int amount) // add money 
    {
        playerPP += amount;
    }

    public int GetPP() => playerPP;
    public int GetWavesCompleted() => wavesCompleted;
    public int GetCurrentWave() => currentWave;
}