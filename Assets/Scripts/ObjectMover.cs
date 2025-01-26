using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [Header("Posiciones posibles")]
    public Vector3 position1;
    public Vector3 position2;
    public Vector3 position3;

    [Header("Configuración de movimiento")]
    public float minInterval = 1f; // Intervalo mínimo
    public float maxInterval = 5f; // Intervalo máximo

    private float timer = 0f;
    private float currentInterval; // Intervalo actual
    private Vector3 lastPosition;  // Última posición seleccionada

    void Start()
    {
        SetRandomInterval();
        lastPosition = transform.position; // Inicia la última posición en la actual
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentInterval)
        {
            MoveToRandomPosition();
            SetRandomInterval(); // Cambia el intervalo para la próxima vez
            timer = 0f;
        }
    }

    void MoveToRandomPosition()
    {
        // Crear un array con las posiciones posibles
        Vector3[] positions = { position1, position2, position3 };

        // Filtrar las posiciones para excluir la última
        Vector3 newPosition;
        do
        {
            int randomIndex = Random.Range(0, positions.Length);
            newPosition = positions[randomIndex];
        } while (newPosition == lastPosition); // Repetir si la nueva posición es igual a la anterior

        // Mover al objeto a la nueva posición
        transform.position = newPosition;

        // Actualizar la última posición
        lastPosition = newPosition;
    }

    void SetRandomInterval()
    {
        currentInterval = Random.Range(minInterval, maxInterval);
    }
}
