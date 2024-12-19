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
        // Initialize with cloudsWindow active and others inactive
        lightningWindow.SetActive(false);
        cloudsWindow.SetActive(true);
        oilWindow.SetActive(false);
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.gameObject.SetActive(false);

        // Initialize button child states
        SetButtonChildState(lightningButton, false);
        SetButtonChildState(cloudsButton, true);
        SetButtonChildState(oilButton, false);
    }

    public void OpenLightningWindow()
    {
        StartCoroutine(SwitchWindow(lightningWindow, lightningButton));
    }

    public void OpenCloudsWindow()
    {
        StartCoroutine(SwitchWindow(cloudsWindow, cloudsButton));
    }

    public void OpenOilWindow()
    {
        StartCoroutine(SwitchWindow(oilWindow, oilButton));
    }

    private IEnumerator SwitchWindow(GameObject targetWindow, Button targetButton)
    {
        // Fade out
        fadeImage.gameObject.SetActive(true);
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);

        // Switch windows
        lightningWindow.SetActive(false);
        cloudsWindow.SetActive(false);
        oilWindow.SetActive(false);
        targetWindow.SetActive(true);

        // Update button child states
        SetButtonChildState(lightningButton, false);
        SetButtonChildState(cloudsButton, false);
        SetButtonChildState(oilButton, false);
        SetButtonChildState(targetButton, true);

        // Fade in
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
