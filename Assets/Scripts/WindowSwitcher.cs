using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WindowSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject lightningWindow;
    [SerializeField] private GameObject cloudsWindow;
    [SerializeField] private GameObject oilWindow;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    [SerializeField] private Button lightningButton;
    [SerializeField] private Button cloudsButton;
    [SerializeField] private Button oilButton;

    private void Start()
    {
        lightningWindow.SetActive(false);
        cloudsWindow.SetActive(true);
        oilWindow.SetActive(false);
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.gameObject.SetActive(false);

        SetButtonChildState(lightningButton, false);
        SetButtonChildState(cloudsButton, true);
        SetButtonChildState(oilButton, false);
    }

    public void OpenLightningWindow()
    {
        if (!lightningWindow.activeInHierarchy)
            StartCoroutine(SwitchWindow(lightningWindow, lightningButton));
    }

    public void OpenCloudsWindow()
    {
        if (!cloudsWindow.activeInHierarchy)
            StartCoroutine(SwitchWindow(cloudsWindow, cloudsButton));
    }

    public void OpenOilWindow()
    {
        if (!oilWindow.activeInHierarchy)
            StartCoroutine(SwitchWindow(oilWindow, oilButton));
    }

    private IEnumerator SwitchWindow(GameObject targetWindow, Button targetButton)
    {
        fadeImage.gameObject.SetActive(true);
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);

        lightningWindow.SetActive(false);
        cloudsWindow.SetActive(false);
        oilWindow.SetActive(false);
        targetWindow.SetActive(true);

        SetButtonChildState(lightningButton, false);
        SetButtonChildState(cloudsButton, false);
        SetButtonChildState(oilButton, false);
        SetButtonChildState(targetButton, true);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = 1 - (t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.gameObject.SetActive(false);
    }

    private void SetButtonChildState(Button button, bool state)
    {
        if (button.transform.childCount > 0)
        {
            button.transform.GetChild(2).gameObject.SetActive(state);
        }
    }
}