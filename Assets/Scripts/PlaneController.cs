using UnityEngine;

public class PlaneController : MonoBehaviour
{
    private float minX = -1.5f;
    private float maxX = 1.5f;
    private float moveSpeed = 5f;

    private Vector3 touchStartPosition;
    private Vector3 planeStartPosition;
    [SerializeField] private GameObject _tutorialWindow;

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (_tutorialWindow.activeInHierarchy) _tutorialWindow.SetActive(false);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPosition = touch.position;
                    planeStartPosition = transform.position;
                    break;

                case TouchPhase.Moved:
                    float deltaX = touch.position.x - touchStartPosition.x;

                    float screenToWorldFactor = Camera.main.pixelWidth;
                    deltaX = deltaX / screenToWorldFactor * moveSpeed;

                    float newX = Mathf.Clamp(planeStartPosition.x + deltaX, minX, maxX);
                    transform.position = new Vector3(newX, transform.position.y, transform.position.z);
                    break;
            }
        }
    }
}