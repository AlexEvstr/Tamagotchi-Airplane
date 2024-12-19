using System.Collections;
using UnityEngine;

public class VictoryDefeatWindow : MonoBehaviour
{
    public GameObject victoryWindow;
    public GameObject defeatWindow;
    private float animationDuration = 0.5f;

    public void ShowVictoryWindow()
    {
        StartCoroutine(ShowWindow(victoryWindow));
    }

    public void ShowDefeatWindow()
    {
        StartCoroutine(ShowWindow(defeatWindow));
    }

    private IEnumerator ShowWindow(GameObject window)
    {
        // Enable the window
        window.SetActive(true);

        // Scale the child object from 0,0 to 1,1
        Transform childTransform = window.transform.GetChild(0);
        childTransform.localScale = Vector3.zero;

        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float scale = Mathf.Lerp(0, 1, elapsedTime / animationDuration);
            childTransform.localScale = new Vector3(scale, scale, scale);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        childTransform.localScale = Vector3.one;
    }
}
