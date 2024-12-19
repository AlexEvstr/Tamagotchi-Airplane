using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Text timerText; // Text для вывода времени
    private float gameTime = 10f; // Таймер игры в секундах
    private bool isRunning = false; // Флаг для проверки, работает ли таймер

    private VictoryDefeatWindow _victoryDefeatWindow;

    private const string CloudPointsKey = "CloudPoints";

    private void Start()
    {
        Time.timeScale = 1;
        _victoryDefeatWindow = GetComponent<VictoryDefeatWindow>();
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
        _victoryDefeatWindow.ShowDefeatWindow();
    }

    private IEnumerator TimerRoutine()
    {
        float remainingTime = gameTime;

        //while (remainingTime > 0 && isRunning && !_victoryDefeatWindow.defeatWindow.activeInHierarchy)
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
            _victoryDefeatWindow.ShowVictoryWindow();
            AddCloudPoints(6);
        }

        isRunning = false;
    }

    private void UpdateTimerText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = $"{minutes}:{seconds:00}";
    }

    private void AddCloudPoints(int points)
    {
        int currentPoints = PlayerPrefs.GetInt(CloudPointsKey, 25);
        currentPoints += points;
        PlayerPrefs.SetInt(CloudPointsKey, currentPoints);
        PlayerPrefs.Save();
    }
}
