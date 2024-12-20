using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Text timerText;
    private float gameTime = 30f;
    private bool isRunning = false;

    private VictoryDefeatWindow _victoryDefeatWindow;

    private const string CloudPointsKey = "CloudPoints";

    private AudioController _audioController;

    private void Start()
    {
        Time.timeScale = 1;
        _audioController = GetComponent<AudioController>();
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
        _audioController.SoundLose();
        _victoryDefeatWindow.ShowDefeatWindow();
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
            _audioController.SoundWin();
            _victoryDefeatWindow.ShowVictoryWindow();
            AddCloudPoints(8);
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