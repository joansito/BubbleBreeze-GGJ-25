using UnityEngine;
using FMODUnity;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;

    [Header("Collision Timer Settings")]
    public float timeThreshold = 1000f; // Tiempo en milisegundos para activar las burbujas
    public float limitRepeatInterval = 1000f; // Tiempo en milisegundos para repetir el evento de l�mite

    [Header("FMOD Events")]
    public EventReference windEnterEvent;
    public EventReference windExitEvent;
    public EventReference bubblePopEvent;
    public EventReference moveLeftEvent;   // Evento al mover a la izquierda
    public EventReference moveRightEvent;  // Evento al mover a la derecha
    public EventReference idlePlayerEvent; // Evento para inactividad
    public EventReference limitEvent;      // Evento para colisi�n con "Limits"

    private FMOD.Studio.EventInstance bubbleSoundInstance;
    private FMOD.Studio.EventInstance leftSoundInstance;
    private FMOD.Studio.EventInstance rightSoundInstance;
    private FMOD.Studio.EventInstance idleSoundInstance;

    private bool isInWindZone = false;
    private bool isBubbleSoundPlaying = false;
    private bool isMovingRight = false;
    private bool isMovingLeft = false;

    private bool isCollidingWithLimit = false; // Nueva bandera para colisi�n con l�mites
    private float collisionTimer = 0f;
    private float limitEventTimer = 0f; // Temporizador para repetir el evento LimitEvent

    void Start()
    {
        // Crear las instancias de sonido necesarias
        bubbleSoundInstance = RuntimeManager.CreateInstance(bubblePopEvent);
        leftSoundInstance = RuntimeManager.CreateInstance(moveLeftEvent);
        rightSoundInstance = RuntimeManager.CreateInstance(moveRightEvent);
        idleSoundInstance = RuntimeManager.CreateInstance(idlePlayerEvent);

        // Iniciar el sonido de inactividad al inicio del juego
        idleSoundInstance.start();
    }

    void Update()
    {
        // Movimiento izquierda-derecha
        float horizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * horizontal * speed * Time.deltaTime);

        if (horizontal > 0f) // Movimiento hacia la derecha
        {
            if (!isMovingRight)
            {
                Debug.Log("Movi�ndose a la derecha, reproduciendo sonido.");
                rightSoundInstance.start();
                isMovingRight = true;
                isMovingLeft = false; // Asegurarse de que no se reproduce el sonido de izquierda
                leftSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
        else if (horizontal < 0f) // Movimiento hacia la izquierda
        {
            if (!isMovingLeft)
            {
                Debug.Log("Movi�ndose a la izquierda, reproduciendo sonido.");
                leftSoundInstance.start();
                isMovingLeft = true;
                isMovingRight = false; // Asegurarse de que no se reproduce el sonido de derecha
                rightSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
        else
        {
            if (isMovingRight || isMovingLeft)
            {
                Debug.Log("Deteniendo sonidos de movimiento.");
                rightSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                leftSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                isMovingRight = false;
                isMovingLeft = false;
            }
        }

        // Si el jugador est� colisionando con un l�mite, manejar la reproducci�n peri�dica del evento
        if (isCollidingWithLimit)
        {
            limitEventTimer += Time.deltaTime * 1000f; // Convertir a milisegundos

            if (limitEventTimer >= limitRepeatInterval)
            {
                Debug.Log("Reproduciendo evento de l�mite.");
                RuntimeManager.PlayOneShot(limitEvent); // Reproducir el evento
                limitEventTimer = 0f; // Reiniciar el temporizador
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detecta colisi�n con objetos que tengan el tag "Limits"
        if (collision.gameObject.CompareTag("Limits"))
        {
            Debug.Log("El jugador ha colisionado con un l�mite.");
            isCollidingWithLimit = true; // Activar la bandera
            limitEventTimer = 0f; // Reiniciar el temporizador al inicio de la colisi�n
            RuntimeManager.PlayOneShot(limitEvent); // Reproducir el evento inicial
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Detecta cu�ndo se deja de colisionar con objetos que tengan el tag "Limits"
        if (collision.gameObject.CompareTag("Limits"))
        {
            Debug.Log("El jugador ha salido del l�mite.");
            isCollidingWithLimit = false; // Desactivar la bandera
        }
    }

    private void OnDestroy()
    {
        // Liberar las instancias de sonido
        if (bubbleSoundInstance.isValid())
        {
            bubbleSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            bubbleSoundInstance.release();
        }

        if (leftSoundInstance.isValid())
        {
            leftSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            leftSoundInstance.release();
        }

        if (rightSoundInstance.isValid())
        {
            rightSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            rightSoundInstance.release();
        }

        if (idleSoundInstance.isValid())
        {
            idleSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            idleSoundInstance.release();
        }
    }
}
