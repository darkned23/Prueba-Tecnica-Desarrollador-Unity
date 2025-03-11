using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // Eje de rotación (por defecto: eje Y)
    [SerializeField] private float rotationAmount = 180f; // Cantidad de rotación en grados
    [SerializeField] private float duration = 1f; // Duración en segundos de la rotación

    [Header("Curva de Animación")]
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool isRotating = false;

    // Llama a este método para iniciar la rotación
    public void Rotate()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateCoroutine());
        }
    }

    private IEnumerator RotateCoroutine()
    {
        isRotating = true;
        Quaternion initialRotation = transform.rotation;
        // Calcula la rotación final aplicando el ángulo sobre el eje elegido.
        Quaternion finalRotation = Quaternion.AngleAxis(rotationAmount, rotationAxis) * initialRotation;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime; // se ignora el TimeScale
            // Evalúa la curva para obtener el interpolado
            float t = animationCurve.Evaluate(timer / duration);
            transform.rotation = Quaternion.Slerp(initialRotation, finalRotation, t);
            yield return new WaitForSecondsRealtime(0);
        }
        // Asegura que se asigne la rotación final
        transform.rotation = finalRotation;
        isRotating = false;
    }
}