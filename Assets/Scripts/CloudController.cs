using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CloudController : MonoBehaviour
{
    [SerializeField] private Image cloudBar; // Image для шкалы
    [SerializeField] private Text cloudPointsText; // Text для вывода текущих очков
    [SerializeField] private int maxPoints = 50; // Максимальное количество очков
    [SerializeField] private float pointDecayRate = 2f; // Очков тратится в минуту
    private OilController _oilController;
    private EnergyController _energyController;
    private SceneFadeManager _sceneFadeManager;

    private int currentPoints = 0; // Текущее количество очков
    private const string CloudPointsKey = "CloudPoints"; // Ключ для сохранения очков в PlayerPrefs
    private const string LastTimeKey = "CloudLastTime"; // Ключ для сохранения времени выхода

    private void Start()
    {
        _oilController = GetComponent<OilController>();
        _energyController = GetComponent<EnergyController>();
        _sceneFadeManager = GetComponent<SceneFadeManager>();
        LoadState();
        CalculateOfflineProgress();
        UpdateUI();
        StartCoroutine(DecayPoints());
    }

    private void UpdateUI()
    {
        cloudBar.fillAmount = (float)currentPoints / maxPoints;
        cloudPointsText.text = $"{Mathf.FloorToInt((float)currentPoints / maxPoints * 100)}%";
    }

    private IEnumerator DecayPoints()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f); // 2 очка за минуту, обновляем каждые 30 секунд
            if (currentPoints > 0)
            {
                currentPoints = Mathf.Max(0, currentPoints - 1);
                SaveState();
                UpdateUI();
            }
        }
    }

    public void UseEnergyAndSwitchScene()
    {
        if (_oilController.ReturnCharges() >= 2 && _energyController.ReturnCharges() >= 2)
        {
            _oilController.UseCharge(2);
            _energyController.UseCharge(2);
            _sceneFadeManager.LoadSceneWithFade("GameScene");
        }
        else
        {
            if (_oilController.ReturnCharges() < 2) Debug.Log("Not enough oil");
            if (_energyController.ReturnCharges() < 2) Debug.Log("Not enough energy");
        }
    }

    private void SaveState()
    {
        PlayerPrefs.SetInt(CloudPointsKey, currentPoints);
        PlayerPrefs.SetString(LastTimeKey, DateTime.UtcNow.ToString());
        PlayerPrefs.Save();
    }

    private void LoadState()
    {
        currentPoints = PlayerPrefs.GetInt(CloudPointsKey, 25);
    }

    private void CalculateOfflineProgress()
    {
        if (PlayerPrefs.HasKey(LastTimeKey))
        {
            DateTime lastTime = DateTime.Parse(PlayerPrefs.GetString(LastTimeKey));
            TimeSpan timePassed = DateTime.UtcNow - lastTime;
            float totalSeconds = (float)timePassed.TotalSeconds;

            float pointsLost = (totalSeconds / 60f) * pointDecayRate;
            currentPoints = Mathf.Max(0, Mathf.FloorToInt(currentPoints - pointsLost));
            cloudBar.fillAmount = (float)currentPoints / maxPoints;
        }

        SaveState();
    }
}