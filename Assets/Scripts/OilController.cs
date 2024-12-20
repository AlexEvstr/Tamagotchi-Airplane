using UnityEngine;
using UnityEngine.UI;

public class OilController : MonoBehaviour
{
    [SerializeField] private Image oilBar;
    [SerializeField] private Text chargesText;
    [SerializeField] private Text chargesCloudWindowText;
    [SerializeField] private Text fillPercentageText;
    [SerializeField] private Text fillPercentageBtnText;
    private int maxCharges = 5;
    private int oilPerCharge = 100;

    private int currentCharges = 0;
    private int currentOil = 0;

    private const string ChargesKey = "OilCurrentCharges";
    private const string OilKey = "OilCurrentAmount";

    private void Start()
    {
        LoadState();
        UpdateUI();
    }

    public void FarmOil()
    {
        if (currentCharges < maxCharges)
        {
            currentOil++;
            if (currentOil >= oilPerCharge)
            {
                currentOil -= oilPerCharge;
                currentCharges++;
            }

            SaveState();
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        float totalFill = ((float)currentCharges + (float)currentOil / oilPerCharge) / maxCharges;
        oilBar.fillAmount = totalFill;
        //oilBar.fillAmount = (float)currentOil / oilPerCharge;
        chargesText.text = $"{currentCharges}";
        chargesCloudWindowText.text = $"{currentCharges}";
        if (currentCharges < maxCharges)
        {
            int percentage = Mathf.FloorToInt(totalFill * 100);
            fillPercentageText.text = $"{percentage}%";
            fillPercentageBtnText.text = $"{percentage}%";
            fillPercentageText.text = $"{Mathf.FloorToInt((float)currentOil / oilPerCharge * 100)}%";
            //fillPercentageBtnText.text = $"{Mathf.FloorToInt((float)currentOil / oilPerCharge * 100)}%";
        }
        else
        {
            oilBar.fillAmount = 1;
            fillPercentageText.text = "Fully charged";
            fillPercentageBtnText.text = "100%";
        }
    }

    public void UseCharge(int count)
    {
        if (currentCharges > 0)
        {
            currentCharges -= count;
            SaveState();
            UpdateUI();
        }
    }

    private void SaveState()
    {
        PlayerPrefs.SetInt(ChargesKey, currentCharges);
        PlayerPrefs.SetInt(OilKey, currentOil);
        PlayerPrefs.Save();
    }

    private void LoadState()
    {
        currentCharges = PlayerPrefs.GetInt(ChargesKey, 4);
        currentOil = PlayerPrefs.GetInt(OilKey, 0);
        if (currentCharges >= maxCharges)
        {
            currentOil = 0;
        }
    }

    public int ReturnCharges()
    {
        return currentCharges;
    }
}