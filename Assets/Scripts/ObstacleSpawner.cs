using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstacles; // Массив препятствий
    public float spawnInterval = 2f; // Интервал спавна объектов

    private void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 0f, spawnInterval);
    }

    private void SpawnObstacle()
    {
        if (obstacles.Length == 0) return;

        // Выбор случайного объекта из массива
        int randomIndex = Random.Range(0, obstacles.Length);
        GameObject obstacle = obstacles[randomIndex];

        // Создание объекта на позиции спавнера
        Instantiate(obstacle, obstacle.transform.position, obstacle.transform.rotation);
    }
}
