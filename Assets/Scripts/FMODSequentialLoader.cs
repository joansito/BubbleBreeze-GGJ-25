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

        [Tooltip("Tiempo en milisegundos despu�s de que termine el evento antes de cargar el siguiente.")]
        public int delayAfterEvent = 1000;
    }

    [Header("Configuraci�n de Eventos FMOD")]
    [Tooltip("Lista de eventos FMOD con sus tiempos personalizados.")]
    public List<FMODEvent> fmodEvents;

    [Header("Configuraci�n Final")]
    [Tooltip("Nombre de la escena a cargar despu�s del �ltimo evento.")]
    public string sceneToLoad;

    [Tooltip("Si est� marcado, el juego saldr� en lugar de cargar otra escena.")]
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
                Debug.LogWarning("Se detect� un EventReference nulo. Saltando este evento.");
                continue;
            }

            // Crear el evento FMOD
            EventInstance eventInstance = RuntimeManager.CreateInstance(fmodEvent.eventReference);

            // Configurar la posici�n 3D del evento (en este caso, en la posici�n del objeto actual)
            var attributes = RuntimeUtils.To3DAttributes(transform.position);
            eventInstance.set3DAttributes(attributes);

            // Comienza a reproducir el evento
            eventInstance.start();

            // Obtiene la duraci�n del evento
            eventInstance.getDescription(out EventDescription eventDescription);
            eventDescription.getLength(out int eventLength);

            // Espera hasta que termine el evento
            yield return new WaitForSeconds(eventLength / 1000f);

            // Libera los recursos del evento
            eventInstance.release();

            // Espera el tiempo de retraso personalizado despu�s del evento
            yield return new WaitForSeconds(fmodEvent.delayAfterEvent / 1000f);
        }

        // Acci�n al terminar los eventos
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
                Debug.LogWarning("No se especific� un nombre de escena para cargar.");
            }
        }
    }
}
