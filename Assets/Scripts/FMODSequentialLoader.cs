using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class FMODSequentialLoader : MonoBehaviour
{
    [System.Serializable]
    public class FMODEvent
    {
        [Tooltip("Referencia del evento FMOD (EventReference).")]
        public EventReference eventReference;

        [Tooltip("Tiempo en milisegundos después de que termine el evento antes de cargar el siguiente.")]
        public int delayAfterEvent = 1000;
    }

    [Header("Configuración de Eventos FMOD")]
    [Tooltip("Lista de eventos FMOD con sus tiempos personalizados.")]
    public List<FMODEvent> fmodEvents;

    [Header("Configuración Final")]
    [Tooltip("Nombre de la escena a cargar después del último evento.")]
    public string sceneToLoad;

    [Tooltip("Si está marcado, el juego saldrá en lugar de cargar otra escena.")]
    public bool exitGameOnFinish = false;

    private void Start()
    {
        StartCoroutine(LoadFMODEventsSequentially());
    }

    private IEnumerator LoadFMODEventsSequentially()
    {
        foreach (var fmodEvent in fmodEvents)
        {
            if (fmodEvent.eventReference.IsNull)
            {
                Debug.LogWarning("Se detectó un EventReference nulo. Saltando este evento.");
                continue;
            }

            // Crear el evento FMOD
            EventInstance eventInstance = RuntimeManager.CreateInstance(fmodEvent.eventReference);

            // Configurar la posición 3D del evento (en este caso, en la posición del objeto actual)
            var attributes = RuntimeUtils.To3DAttributes(transform.position);
            eventInstance.set3DAttributes(attributes);

            // Comienza a reproducir el evento
            eventInstance.start();

            // Obtiene la duración del evento
            eventInstance.getDescription(out EventDescription eventDescription);
            eventDescription.getLength(out int eventLength);

            // Espera hasta que termine el evento
            yield return new WaitForSeconds(eventLength / 1000f);

            // Libera los recursos del evento
            eventInstance.release();

            // Espera el tiempo de retraso personalizado después del evento
            yield return new WaitForSeconds(fmodEvent.delayAfterEvent / 1000f);
        }

        // Acción al terminar los eventos
        if (exitGameOnFinish)
        {
            Debug.Log("Saliendo del juego...");
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        else
        {
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogWarning("No se especificó un nombre de escena para cargar.");
            }
        }
    }
}
