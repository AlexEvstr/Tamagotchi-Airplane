using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    private float speed = 15f;
    private VictoryDefeatWindow _victoryDefeatWindow;

    private void Start()
    {
        _victoryDefeatWindow = FindObjectOfType<VictoryDefeatWindow>();
    }

    private void Update()
    {
        if (!_victoryDefeatWindow.defeatWindow.activeInHierarchy && !_victoryDefeatWindow.victoryWindow.activeInHierarchy)
            transform.Translate(Vector3.back * speed * Time.deltaTime);
    }
}