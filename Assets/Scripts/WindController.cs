using UnityEngine;

public class WindController : MonoBehaviour
{
    public float minSpeed = 2f;         // Velocidad mínima del viento
    public float maxSpeed = 5f;         // Velocidad máxima del viento
    public float minMoveTime = 2f;      // Tiempo mínimo de movimiento en una dirección
    public float maxMoveTime = 5f;      // Tiempo máximo de movimiento en una dirección

    private float currentSpeed;         // Velocidad actual del viento
    private float moveTimer;            // Temporizador para cambiar de dirección
    private bool movingRight = true;    // Dirección del movimiento (true = derecha, false = izquierda)

    void Start()
    {
        SetRandomSpeed();
        SetRandomMoveTime();
    }

    void Update()
    {
        // Movimiento en el eje X
        float direction = movingRight ? 1f : -1f; // 1 para derecha, -1 para izquierda
        transform.Translate(Vector2.right * direction * currentSpeed * Time.deltaTime);

        // Temporizador para cambiar de dirección
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            ToggleDirection();          // Cambiar de dirección
            SetRandomSpeed();           // Cambiar velocidad
            SetRandomMoveTime();        // Cambiar duración del movimiento
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Limits"))
        {
            // Cambiar de dirección al chocar con un límite
            movingRight = !movingRight;

            // Reiniciar velocidad y temporizador
            SetRandomSpeed();
            SetRandomMoveTime();

            // Ajustar posición para que no atraviese el límite
            float clampedX = Mathf.Clamp(transform.position.x, other.bounds.min.x, other.bounds.max.x);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
    }

    private void ToggleDirection()
    {
        movingRight = !movingRight; // Cambiar dirección
    }

    private void SetRandomSpeed()
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed); // Seleccionar velocidad aleatoria
    }

    private void SetRandomMoveTime()
    {
        moveTimer = Random.Range(minMoveTime, maxMoveTime); // Seleccionar tiempo aleatorio
    }
}
