using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private GameObject _cloudPanel;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;
    private int _firstEnter;

    private void Start()
    {
        _firstEnter = PlayerPrefs.GetInt("FirstEnterKey", 0);
        if (_firstEnter == 0) _tutorialPanel.SetActive(true);
        else _cloudPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        StartCoroutine(SwitchWindow(_cloudPanel));
        PlayerPrefs.SetInt("FirstEnterKey", 1);
    }

    private IEnumerator SwitchWindow(GameObject targetWindow)
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

        _tutorialPanel.SetActive(false);
        _cloudPanel.SetActive(true);


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
}