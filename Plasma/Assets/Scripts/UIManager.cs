using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    // variables
    [SerializeField] private RectTransform healthSlider;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text ppText;

    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();
    }

    private void Update()
    {
        if (playerHealth != null) // player health ui. honestly no clue how this works i found this online. 
        {
            float ratio = Mathf.Clamp01(playerHealth.GetHealth() / playerHealth.GetMaxHealth()); // i think it makes a ratio of the max health and current health
            Vector2 size = healthSlider.sizeDelta; // then sets the size
            size.x = ratio * 125f;  // based on the ratio, 125 being the max width
            healthSlider.sizeDelta = size; // then does it? idk ur guess is as good as mine 
        }

        if (waveText != null) // if wave text EXISTS
            waveText.text = "WAVE " + GameManager.Instance.GetCurrentWave(); // set the wave text to the wave number

        if (ppText != null) // if the ppText EXISTS
            ppText.text = GameManager.Instance.GetPP().ToString(); // set it ok thank
    }
}