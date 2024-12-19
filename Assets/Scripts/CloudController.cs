using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CloudController : MonoBehaviour
{
    [SerializeField] private Image cloudBar;
    [SerializeField] private Text cloudPointsText;
    [SerializeField] private int maxPoints = 50;
    [SerializeField] private float pointDecayRate = 2f;
    private OilController _oilController;
    private EnergyController _energyController;
    private SceneFadeManager _sceneFadeManager;

    private int currentPoints = 0;
    private const string CloudPointsKey = "CloudPoints";
    private const string LastTimeKey = "CloudLastTime";

    [SerializeField] private GameObject _notEnoughOil;
    [SerializeField] private GameObject _notEnoughEnergy;

    private AudioController _audioController;

    private void Start()
    {
        _audioController = GetComponent<AudioController>();
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
            yield return new WaitForSeconds(30f);
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
            _audioController.SoundClick();
        }
        else
        {
            if (_oilController.ReturnCharges() < 2) StartCoroutine(ShowNotEnoughOil());
            if (_energyController.ReturnCharges() < 2) StartCoroutine(ShowNotEnoughEnergy());
            _audioController.SoundError();
        }
    }

    private IEnumerator ShowNotEnoughOil()
    {
        _notEnoughOil.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        _notEnoughOil.SetActive(false);
    }

    private IEnumerator ShowNotEnoughEnergy()
    {
        _notEnoughEnergy.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        _notEnoughEnergy.SetActive(false);
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