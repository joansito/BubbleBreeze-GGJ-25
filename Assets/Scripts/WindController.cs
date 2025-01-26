using UnityEngine;

public class WindController : MonoBehaviour
{
    public float minSpeed = 2f;         // Velocidad m�nima del viento
    public float maxSpeed = 5f;         // Velocidad m�xima del viento
    public float minMoveTime = 2f;      // Tiempo m�nimo de movimiento en una direcci�n
    public float maxMoveTime = 5f;      // Tiempo m�ximo de movimiento en una direcci�n

    private float currentSpeed;         // Velocidad actual del viento
    private float moveTimer;            // Temporizador para cambiar de direcci�n
    private bool movingRight = true;    // Direcci�n del movimiento (true = derecha, false = izquierda)

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

        // Temporizador para cambiar de direcci�n
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            ToggleDirection();          // Cambiar de direcci�n
            SetRandomSpeed();           // Cambiar velocidad
            SetRandomMoveTime();        // Cambiar duraci�n del movimiento
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Limits"))
        {
            // Cambiar de direcci�n al chocar con un l�mite
            movingRight = !movingRight;

            // Reiniciar velocidad y temporizador
            SetRandomSpeed();
            SetRandomMoveTime();

            // Ajustar posici�n para que no atraviese el l�mite
            float clampedX = Mathf.Clamp(transform.position.x, other.bounds.min.x, other.bounds.max.x);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
    }

    private void ToggleDirection()
    {
        movingRight = !movingRight; // Cambiar direcci�n
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
