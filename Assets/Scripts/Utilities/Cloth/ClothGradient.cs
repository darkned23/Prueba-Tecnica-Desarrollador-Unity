using UnityEngine;

public class ClothGradientEditor : MonoBehaviour
{
    [Range(0f, 1f)] public float minDistance = 0f; // Movimiento mínimo permitido
    [Range(0f, 1f)] public float maxDistance = 0.5f; // Movimiento máximo permitido

    [Range(0f, 1f)] public float minCollisionDistance = 0f; // Colisión mínima
    [Range(0f, 1f)] public float maxCollisionDistance = 0.1f; // Colisión máxima

    public float unaffectedHeightRange = 0.2f; // Rango donde los vértices NO se ven afectados

    private void Start()
    {
        Cloth cloth = GetComponent<Cloth>();
        if (cloth == null)
        {
            Debug.LogWarning("No hay componente Cloth en este objeto.");
            return;
        }

        ClothSkinningCoefficient[] constraints = cloth.coefficients;
        Vector3[] vertices = cloth.vertices;

        // Obtener el rango de altura de los vértices en el espacio mundial
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        foreach (var vert in vertices)
        {
            float worldY = transform.TransformPoint(vert).y; // Convertir a coordenadas globales

            if (worldY < minY) minY = worldY;
            if (worldY > maxY) maxY = worldY;
        }

        float unaffectedY = maxY - unaffectedHeightRange * (maxY - minY); // Altura hasta donde los vértices no se ven afectados

        // Aplicar gradiente de pesos basado en la altura mundial
        for (int i = 0; i < constraints.Length; i++)
        {
            float worldY = transform.TransformPoint(vertices[i]).y; // Convertir vértice a coordenadas globales

            if (worldY >= unaffectedY)
            {
                // Los vértices en la zona superior no se ven afectados
                constraints[i].maxDistance = minDistance;
                constraints[i].collisionSphereDistance = minCollisionDistance;
            }
            else
            {
                // Los vértices fuera del rango de protección se ven afectados progresivamente
                float normalizedHeight = Mathf.InverseLerp(unaffectedY, minY, worldY); // Normaliza desde el umbral hacia abajo

                constraints[i].maxDistance = Mathf.Lerp(minDistance, maxDistance, normalizedHeight);
                constraints[i].collisionSphereDistance = Mathf.Lerp(minCollisionDistance, maxCollisionDistance, normalizedHeight);
            }
        }

        cloth.coefficients = constraints; // Aplicar cambios al Cloth
    }
}
