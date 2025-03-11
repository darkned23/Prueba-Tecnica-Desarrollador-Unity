using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationAmount = 180f;
    [SerializeField] private float duration = .5f;

    [Header("Curva de Animación")]
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool isRotating = false;
    private Quaternion originalRotation;

    private void Awake()
    {
        originalRotation = transform.rotation;
    }

    // Llama a este método para iniciar la rotación
    public void Rotate()
    {
        if (!isRotating)
        {
            Quaternion initialRotation = transform.rotation;
            Quaternion finalRotation = Quaternion.AngleAxis(rotationAmount, rotationAxis) * initialRotation;
            StartCoroutine(RotateTo(initialRotation, finalRotation));
        }
    }

    // Llama a este método para restablecer la rotación original
    public void ResetRotation()
    {
        if (!isRotating)
        {
            Quaternion currentRotation = transform.rotation;
            StartCoroutine(RotateTo(currentRotation, originalRotation));
        }
    }

    // Método común para interpolar la rotación entre dos estados
    public IEnumerator RotateTo(Quaternion from, Quaternion to)
    {
        isRotating = true;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float t = animationCurve.Evaluate(timer / duration);
            transform.rotation = Quaternion.Slerp(from, to, t);
            yield return new WaitForSecondsRealtime(0);
        }
        transform.rotation = to;
        isRotating = false;
    }
}