using UnityEngine;

public class DynamicSkyboxRotator : MonoBehaviour
{
    [SerializeField] private float skyboxRotationSpeed = 1.5f; // Скорость вращения Skybox
    //[SerializeField] private bool useCustomAxis = false; // Вращать вокруг выбранной оси
    //[SerializeField] private Vector3 rotationAxis = Vector3.up; // Ось вращения

    private float currentRotationAngle = 0f; // Текущий угол поворота Skybox

    void Update()
    {
        // Рассчитываем новый угол поворота
        currentRotationAngle += skyboxRotationSpeed * Time.deltaTime;
        RenderSettings.skybox.SetFloat("_Rotation", currentRotationAngle);
    }
}
