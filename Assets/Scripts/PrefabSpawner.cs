using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [Header("Prefab y configuraci�n")]
    public GameObject prefab; // Prefab que se spawnea
    public float spawnInterval = 0.5f; // Intervalo entre spawns (en segundos)
    public float spawnRadius = 5f; // Radio alrededor del objeto

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnPrefab();
            timer = 0f;
        }
    }

    void SpawnPrefab()
    {
        // Genera una posici�n aleatoria dentro del radio
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPosition.y = transform.position.y; // Mant�n la misma altura (opcional)

        // Instancia el prefab en la posici�n calculada
        Instantiate(prefab, randomPosition, Quaternion.identity);
    }
}
