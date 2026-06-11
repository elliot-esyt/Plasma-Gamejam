using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
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
        if (playerHealth != null)
        {
            float ratio = Mathf.Clamp01(playerHealth.GetHealth() / playerHealth.GetMaxHealth());
            Vector2 size = healthSlider.sizeDelta;
            size.x = ratio * 125f;
            healthSlider.sizeDelta = size;
        }

        if (waveText != null)
            waveText.text = "WAVE " + GameManager.Instance.GetCurrentWave();

        if (ppText != null)
            ppText.text = GameManager.Instance.GetPP().ToString();
    }
}