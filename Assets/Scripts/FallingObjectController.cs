using UnityEngine;
using FMODUnity;

public class FallingObjectController : MonoBehaviour
{
    public float fallSpeed = 2f;  // Velocidad de caída del objeto
    public int points = 10;      // Puntos que el objeto otorga al colisionar con el jugador

    [Header("FMOD Events")]
    public EventReference collisionSoundEvent;  // Evento de sonido al colisionar con el jugador

    private bool hasCollidedWithPlayer = false; // Para evitar múltiples colisiones al mismo tiempo

    void Update()
    {
        // Mover el objeto hacia abajo a la velocidad definida
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (hasCollidedWithPlayer)
                return; // Evitar múltiples colisiones con el jugador

            hasCollidedWithPlayer = true;

            // Reproducir el sonido de colisión
            RuntimeManager.PlayOneShot(collisionSoundEvent);

            // Sumar puntos al jugador
            Debug.Log("Colisión con el jugador. Puntos otorgados: " + points);
            GameManager.Instance.AddPoints(points);

            // Destruir el objeto después de reproducir el sonido
            Destroy(gameObject, 0.1f);
        }
        else if (collision.collider.CompareTag("Delete"))
        {
            // Eliminar el objeto al colisionar con un objeto con tag "Delete"
            Debug.Log("Colisión con objeto 'Delete'. Eliminando objeto.");
            Destroy(gameObject); // Eliminar inmediatamente
        }
        else if (collision.collider.CompareTag("Limits"))
        {
            // Ignorar colisiones con el tag "Limits"
            Debug.Log("Colisión con 'Limits'. Ignorando.");
        }
    }
}
