using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] private Text loadingText;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private string nextSceneName = "MenuScene";

    private void Start()
    {
        StartCoroutine(LoadingSequence());
    }

    private IEnumerator LoadingSequence()
    {
        for (int i = 0; i <= 100; i++)
        {
            loadingText.text = i + "%";
            yield return new WaitForSeconds(0.03f);
        }

        yield return StartCoroutine(FadeToBlack());

        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator FadeToBlack()
    {
        Color color = fadeImage.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }
}