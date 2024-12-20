using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnergyController : MonoBehaviour
{
    [SerializeField] private Image energyBar;
    [SerializeField] private Text chargesText;
    [SerializeField] private Text chargesCloudWindowText;
    [SerializeField] private Text fillPercentageBtnText;
    [SerializeField] private Text timerText;
    private int maxCharges = 5;
    private float chargeTime = 30f;

    private int currentCharges = 4;
    private float fillAmount = 0f;
    private bool isFilling = true;
    private const string ChargesKey = "CurrentCharges";
    private const string LastTimeKey = "LastEnergyTime";

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
                //energyBar.fillAmount = fillAmount;
                UpdateEnergyBar();
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
                        UpdateEnergyBar();
                        //energyBar.fillAmount = 1f;
                        timerText.text = "Fully charged";
                        fillPercentageBtnText.text = "100%";
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
        float totalFill = ((currentCharges) + fillAmount) / maxCharges;
        //fillPercentageBtnText.text = $"{Mathf.FloorToInt(fillAmount * 100)}%";
        fillPercentageBtnText.text = $"{Mathf.FloorToInt(totalFill * 100)}%";
    }

    private void UpdateEnergyBar()
    {
        energyBar.fillAmount = ((currentCharges) + fillAmount) / maxCharges;
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
        currentCharges = PlayerPrefs.GetInt(ChargesKey, 4);
        fillAmount = 0f;
        //energyBar.fillAmount = fillAmount;

        if (currentCharges >= maxCharges)
        {
            isFilling = false;
            energyBar.fillAmount = 1f;
        }
        else
        {
            UpdateEnergyBar();
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
                //energyBar.fillAmount = fillAmount;
                UpdateEnergyBar();
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
            //energyBar.fillAmount = fillAmount;
            UpdateEnergyBar();
        }

        SaveState();
    }

    public int ReturnCharges()
    {
        return currentCharges;
    }
}