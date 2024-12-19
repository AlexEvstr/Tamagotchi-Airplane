using UnityEngine;

public class PlaneCollision : MonoBehaviour
{
    [SerializeField] private VictoryDefeatWindow _victoryDefeatWindow;
    [SerializeField] private GameTimer _gameTimer;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            //_victoryDefeatWindow.ShowDefeatWindow();
            _gameTimer.StopTimer();
            Destroy(gameObject);
        }
    }
}