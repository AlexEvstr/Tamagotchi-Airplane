using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnergyController : MonoBehaviour
{
    [SerializeField] private Image energyBar; // Image для шкалы
    [SerializeField] private Text chargesText; // Text для вывода текущих зарядов
    [SerializeField] private Text chargesCloudWindowText; // Text для вывода текущих зарядов
    [SerializeField] private Text fillPercentageBtnText; // Text для вывода процента заполнения
    [SerializeField] private Text fillPercentageText; // Text для вывода процента заполнения
    [SerializeField] private Text timerText; // Text для вывода времени таймера
    private int maxCharges = 5; // Максимальное количество зарядов
    private float chargeTime = 30f; // Время, за которое шкала заполняется на 100%

    private int currentCharges = 3; // Текущие заряды
    private float fillAmount = 0f; // Уровень заполнения шкалы
    private bool isFilling = true; // Флаг, заполняется ли шкала
    private const string ChargesKey = "CurrentCharges"; // Ключ для сохранения зарядов в PlayerPrefs
    private const string LastTimeKey = "LastEnergyTime"; // Ключ для сохранения времени выхода

    void Start()
    {
        LoadState();
        CalculateOfflineProgress();
        UpdateUI();
        StartCoroutine(FillEnergy());
    }

    private IEnumerator FillEnergy()
    {
        while (true)
        {
            if (isFilling && currentCharges < maxCharges)
            {
                fillAmount += Time.deltaTime / chargeTime;
                energyBar.fillAmount = fillAmount;
                UpdateFillPercentageText();

                float remainingTime = chargeTime * (1f - fillAmount);
                UpdateTimerText(remainingTime);

                if (fillAmount >= 1f)
                {
                    fillAmount = 0f;
                    currentCharges++;
                    SaveState();
                    UpdateUI();

                    if (currentCharges >= maxCharges)
                    {
                        isFilling = false;
                        energyBar.fillAmount = 1f;
                        timerText.text = "Fully charged";
                        fillPercentageText.text = "";
                        fillPercentageBtnText.text = "";
                    }
                }
            }

            yield return null;
        }
    }

    private void UpdateUI()
    {
        chargesText.text = $"{currentCharges}";
        chargesCloudWindowText.text = $"{currentCharges}";
        if (currentCharges < 5)
            UpdateFillPercentageText();
    }

    private void UpdateFillPercentageText()
    {
        fillPercentageText.text = $"{Mathf.FloorToInt(fillAmount * 100)}%";
        fillPercentageBtnText.text = $"{Mathf.FloorToInt(fillAmount * 100)}%";
    }

    private void UpdateTimerText(float remainingTime)
    {
        if (remainingTime >= 0)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = $"{minutes}:{seconds:00}";
        }
    }

    public void UseCharge(int count)
    {
        if (currentCharges > 0)
        {
            currentCharges -= count;
            isFilling = true;
            SaveState();
            UpdateUI();
        }
    }

    private void SaveState()
    {
        PlayerPrefs.SetInt(ChargesKey, currentCharges);
        PlayerPrefs.SetString(LastTimeKey, DateTime.UtcNow.ToString());
        PlayerPrefs.Save();
    }

    private void LoadState()
    {
        currentCharges = PlayerPrefs.GetInt(ChargesKey, 3);
        fillAmount = 0f; // Начинаем с нуля, пересчитываем при оффлайн-прогрессе
        energyBar.fillAmount = fillAmount;

        if (currentCharges >= maxCharges)
        {
            isFilling = false;
            energyBar.fillAmount = 1f;
        }
    }

    private void CalculateOfflineProgress()
    {
        if (PlayerPrefs.HasKey(LastTimeKey))
        {
            DateTime lastTime = DateTime.Parse(PlayerPrefs.GetString(LastTimeKey));
            TimeSpan timePassed = DateTime.UtcNow - lastTime;
            float totalSeconds = (float)timePassed.TotalSeconds;

            int chargesGained = Mathf.FloorToInt(totalSeconds / chargeTime);
            currentCharges = Mathf.Min(currentCharges + chargesGained, maxCharges);

            if (currentCharges < maxCharges)
            {
                float leftoverTime = totalSeconds % chargeTime;
                fillAmount = leftoverTime / chargeTime;
                energyBar.fillAmount = fillAmount;
            }
            else
            {
                fillAmount = 0f;
                isFilling = false;
                energyBar.fillAmount = 1f;
            }
        }
        else
        {
            fillAmount = 0f;
            energyBar.fillAmount = fillAmount;
        }

        SaveState();
    }

    public int ReturnCharges()
    {
        return currentCharges;
    }
}
