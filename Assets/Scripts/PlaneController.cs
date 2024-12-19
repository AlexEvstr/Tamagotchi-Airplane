using UnityEngine;

public class PlaneController : MonoBehaviour
{
    private float minX = -1.5f; // Минимальное значение X
    private float maxX = 1.5f;  // Максимальное значение X
    private float moveSpeed = 5f; // Скорость перемещения

    private Vector3 touchStartPosition; // Начальная позиция касания
    private Vector3 planeStartPosition; // Начальная позиция самолета

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Сохраняем начальные позиции касания и самолета
                    touchStartPosition = touch.position;
                    planeStartPosition = transform.position;
                    break;

                case TouchPhase.Moved:
                    // Рассчитываем смещение пальца по X относительно экрана
                    float deltaX = touch.position.x - touchStartPosition.x;

                    // Преобразуем смещение в экранных координатах в мировые координаты
                    float screenToWorldFactor = Camera.main.pixelWidth;
                    deltaX = deltaX / screenToWorldFactor * moveSpeed;

                    // Новая позиция самолета
                    float newX = Mathf.Clamp(planeStartPosition.x + deltaX, minX, maxX);
                    transform.position = new Vector3(newX, transform.position.y, transform.position.z);
                    break;
            }
        }
    }
}
