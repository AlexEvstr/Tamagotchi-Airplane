using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFadeManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage; // Ссылка на черный Image для затемнения
    [SerializeField] private float fadeDuration = 0.5f; // Длительность затемнения

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        for (float t = fadeDuration; t > 0; t -= Time.deltaTime)
        {
            float alpha = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);
        SceneManager.LoadScene(sceneName);
    }
}
