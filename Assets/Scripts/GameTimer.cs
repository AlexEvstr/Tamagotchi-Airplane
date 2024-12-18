using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Text timerText; // Text для вывода времени
    private float gameTime = 6f; // Таймер игры в секундах
    private bool isRunning = false; // Флаг для проверки, работает ли таймер

    private void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        isRunning = true;
        StartCoroutine(TimerRoutine());
    }

    public void StopTimer()
    {
        isRunning = false;
        Debug.Log("Lose");
    }

    private IEnumerator TimerRoutine()
    {
        float remainingTime = gameTime;

        while (remainingTime > 0 && isRunning)
        {
            UpdateTimerText(remainingTime);
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }
        UpdateTimerText(remainingTime);

        if (isRunning)
        {
            Debug.Log("Win");
        }

        isRunning = false;
    }

    private void UpdateTimerText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = $"{minutes}:{seconds:00}";
    }
}
