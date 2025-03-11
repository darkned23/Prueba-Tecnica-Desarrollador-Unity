using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour
{
    [Tooltip("Tiempo en segundos antes de destruir el objeto.")]
    public float delay = 30.0f;
    public bool cancelDestruction = false;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        if (!cancelDestruction)
            Destroy(gameObject);
    }
}