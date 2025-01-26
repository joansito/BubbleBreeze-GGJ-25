using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [Header("Posiciones posibles")]
    public Vector3 position1;
    public Vector3 position2;
    public Vector3 position3;

    [Header("Configuraci�n de movimiento")]
    public float minInterval = 1f; // Intervalo m�nimo
    public float maxInterval = 5f; // Intervalo m�ximo

    private float timer = 0f;
    private float currentInterval; // Intervalo actual
    private Vector3 lastPosition;  // �ltima posici�n seleccionada

    void Start()
    {
        SetRandomInterval();
        lastPosition = transform.position; // Inicia la �ltima posici�n en la actual
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentInterval)
        {
            MoveToRandomPosition();
            SetRandomInterval(); // Cambia el intervalo para la pr�xima vez
            timer = 0f;
        }
    }

    void MoveToRandomPosition()
    {
        // Crear un array con las posiciones posibles
        Vector3[] positions = { position1, position2, position3 };

        // Filtrar las posiciones para excluir la �ltima
        Vector3 newPosition;
        do
        {
            int randomIndex = Random.Range(0, positions.Length);
            newPosition = positions[randomIndex];
        } while (newPosition == lastPosition); // Repetir si la nueva posici�n es igual a la anterior

        // Mover al objeto a la nueva posici�n
        transform.position = newPosition;

        // Actualizar la �ltima posici�n
        lastPosition = newPosition;
    }

    void SetRandomInterval()
    {
        currentInterval = Random.Range(minInterval, maxInterval);
    }
}
