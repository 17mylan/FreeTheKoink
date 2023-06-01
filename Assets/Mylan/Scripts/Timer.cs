using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private GameManager gameManager;
    public float timerDuration = 180f; // Durée totale du timer en secondes
    public float currentTimerValue;
    public bool isRunning;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        isRunning = false;
        currentTimerValue = timerDuration;
        UpdateTimerText();
    }

    private void Update()
    {
        if (isRunning)
        {
            currentTimerValue -= Time.deltaTime;
            if (currentTimerValue <= 0f)
            {
                currentTimerValue = 0f;
                isRunning = false;
            }

            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTimerValue / 60f);
        int seconds = Mathf.FloorToInt(currentTimerValue % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void ResetTimer()
    {
        gameManager.currentPlayerDollar = gameManager.currentPlayerDollar - 500;
        gameManager.playerDollar.text = gameManager.currentPlayerDollar.ToString() + "$";
        gameManager.dollarSlider.value = (float)gameManager.currentPlayerDollar / gameManager.maxPlayerDollar;
        currentTimerValue = timerDuration;
        isRunning = true;
        UpdateTimerText();
        print("Timer Reset");
    }
}
