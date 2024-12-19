using UnityEngine;

public class DynamicSkyboxRotator : MonoBehaviour
{
    [SerializeField] private float skyboxRotationSpeed = 1.5f;

    private float currentRotationAngle = 0f;

    void Update()
    {
        currentRotationAngle += skyboxRotationSpeed * Time.deltaTime;
        RenderSettings.skybox.SetFloat("_Rotation", currentRotationAngle);
    }
}