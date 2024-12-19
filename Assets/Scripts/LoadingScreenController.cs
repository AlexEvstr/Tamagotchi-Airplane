using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] private Text loadingText; // Ссылка на текст процента загрузки
    [SerializeField] private Image fadeImage; // Ссылка на Image для затемнения
    [SerializeField] private float fadeDuration = 1f; // Длительность затемнения
    [SerializeField] private string nextSceneName = "MenuScene"; // Имя следующей сцены

    private void Start()
    {
        StartCoroutine(LoadingSequence());
    }

    private IEnumerator LoadingSequence()
    {
        // Процесс отображения загрузки
        for (int i = 0; i <= 100; i++)
        {
            loadingText.text = i + "%";
            yield return new WaitForSeconds(0.03f); // Имитируем процесс загрузки
        }

        // Начинаем затемнение
        yield return StartCoroutine(FadeToBlack());

        // Переход на следующую сцену
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
