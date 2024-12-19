using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstacles;
    public float spawnInterval = 2f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 0f, spawnInterval);
    }

    private void SpawnObstacle()
    {
        if (obstacles.Length == 0) return;

        int randomIndex = Random.Range(0, obstacles.Length);
        GameObject obstacle = obstacles[randomIndex];

        Instantiate(obstacle, obstacle.transform.position, obstacle.transform.rotation);
    }
}