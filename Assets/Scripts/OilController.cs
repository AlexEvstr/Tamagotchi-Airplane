using UnityEngine;
using UnityEngine.UI;

public class OilController : MonoBehaviour
{
    [SerializeField] private Image oilBar; // Image для шкалы
    [SerializeField] private Text chargesText; // Text для вывода текущих зарядов
    [SerializeField] private Text chargesCloudWindowText; // Text для вывода текущих зарядов
    [SerializeField] private Text fillPercentageText; // Text для вывода процента заполнения
    [SerializeField] private Text fillPercentageBtnText; // Text для вывода процента заполнения
    [SerializeField] private GameObject _fullText;
    private int maxCharges = 5; // Максимальное количество зарядов
    private int oilPerCharge = 100; // Количество масла для одного заряда

    private int currentCharges = 0; // Текущие заряды
    private int currentOil = 0; // Текущее количество масла

    private const string ChargesKey = "OilCurrentCharges"; // Ключ для сохранения зарядов в PlayerPrefs
    private const string OilKey = "OilCurrentAmount"; // Ключ для сохранения масла в PlayerPrefs

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
        oilBar.fillAmount = (float)currentOil / oilPerCharge;
        chargesText.text = $"{currentCharges}";
        chargesCloudWindowText.text = $"{currentCharges}";
        if (currentCharges < 5)
        {
            fillPercentageText.text = $"{Mathf.FloorToInt((float)currentOil / oilPerCharge * 100)}%";
            fillPercentageBtnText.text = $"{Mathf.FloorToInt((float)currentOil / oilPerCharge * 100)}%";
        }
        else
        {
            _fullText.SetActive(true);
            fillPercentageText.text = "";
            fillPercentageBtnText.text = "";
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
        currentCharges = PlayerPrefs.GetInt(ChargesKey, 3);
        currentOil = PlayerPrefs.GetInt(OilKey, 0);
    }

    public int ReturnCharges()
    {
        return currentCharges;
    }
}
