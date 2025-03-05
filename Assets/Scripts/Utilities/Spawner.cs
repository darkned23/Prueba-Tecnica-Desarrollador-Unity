using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int amount = 1;
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private bool spawnInCircle = true;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnRadius = 100f;
    [SerializeField] private Vector3 spawnRotation;
    [SerializeField] private bool randomRotation = true;
    private GameObject container;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => ApiRawgManager.instance.IsReadyAllGames() == true);

        if (prefab == null)
        {
            Debug.LogError("Prefab no asignado, se detiene la ejecución.");
            yield break;
        }
        container = new GameObject($"Container_{prefab.name}s");


        if (spawnOnStart)
        {
            if (spawnInCircle)
            {
                StartCoroutine(SpawnInCircle());
            }
            else
            {
                StartCoroutine(SpawnAmount());
            }
        }
    }
    private IEnumerator SpawnInCircle()
    {
        while (true)
        {
            StartCoroutine(ApiRawgManager.instance.GenerateDataGame());
            yield return new WaitUntil(() => ApiRawgManager.instance.GetGame() != null && ApiRawgManager.instance.IsReadyDataGame() == true);

            SpawnPrefab(transform.position);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator SpawnAmount()
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnPrefab(transform.position);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnPrefab(Vector3 position)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab no asignado.");
            return;
        }

        Vector3 randomPosition = position + Random.insideUnitSphere * spawnRadius;

        if (randomRotation)
        {
            spawnRotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        }
        Quaternion rotation = Quaternion.Euler(spawnRotation);

        randomPosition.y = position.y;
        GameObject instance = Instantiate(prefab, randomPosition, rotation, container.transform);
    }
}