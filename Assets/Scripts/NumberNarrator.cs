using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class NumberNarrator : MonoBehaviour
{
    [Header("FMOD Settings")]
    [SerializeField] private string fmodEventPath = "event:/Narrator"; // Ruta del evento principal para los n�meros
    [SerializeField] private string preNumberEventPath = "event:/PreNumber"; // Evento antes de narrar el n�mero
    [SerializeField] private string postNumberEventPath = "event:/PostNumber"; // Evento despu�s de narrar el n�mero

    [Header("Scene Settings")]
    [SerializeField] private string nextSceneName = ""; // Nombre de la siguiente escena (dejar vac�o para no cambiar de escena)
    [SerializeField] private bool exitGameAfter = false; // Salir del juego despu�s de reproducir el n�mero
    [SerializeField] private float exitDelay = 0f; // Retraso en segundos antes de salir del juego

    public void NarrateNumber(int number)
    {
        if (number < 0 || number > 5000 || number % 10 != 0)
        {
            Debug.LogError("El n�mero debe estar entre 0 y 5000, y ser m�ltiplo de 10.");
            return;
        }

        // Descompone el n�mero y obtiene la secuencia de clips necesarios
        List<float> audioSequence = DecomposeNumber(number);

        // Inicia la secuencia de reproducci�n
        StartCoroutine(PlayCompleteSequence(audioSequence));
    }

    private List<float> DecomposeNumber(int number)
    {
        List<float> sequence = new List<float>();

        if (number == 0)
        {
            sequence.Add(0); // "Cero"
            return sequence;
        }

        if (number >= 1000)
        {
            int thousands = number / 1000;
            sequence.Add(thousands); // "Mil", "Dos mil", etc.
            number %= 1000;
        }

        if (number >= 100)
        {
            int hundreds = number / 100;
            sequence.Add(hundreds * 100); // "Cien", "Doscientos", etc.
            number %= 100;
        }

        if (number >= 10)
        {
            sequence.Add(number); // "Diez", "Veinte", etc.
        }

        return sequence;
    }

    private System.Collections.IEnumerator PlayCompleteSequence(List<float> sequence)
    {
        // Verifica conflictos entre cambio de escena y salir del juego
        if (!string.IsNullOrEmpty(nextSceneName) && exitGameAfter)
        {
            Debug.LogError("No se puede cambiar de escena y salir del juego al mismo tiempo. Verifica las configuraciones.");
            yield break;
        }

        // 1. Reproduce el evento previo al n�mero
        if (!string.IsNullOrEmpty(preNumberEventPath))
        {
            yield return PlayEvent(preNumberEventPath);
        }

        // 2. Reproduce la secuencia del n�mero
        foreach (float clipValue in sequence)
        {
            yield return PlayEventWithParameter(fmodEventPath, "ClipName", clipValue);
        }

        // 3. Reproduce el evento posterior al n�mero
        if (!string.IsNullOrEmpty(postNumberEventPath))
        {
            yield return PlayEvent(postNumberEventPath);
        }

        // 4. Limpia los datos (opcional)
        PlayerPrefs.DeleteKey("Score");
        Debug.Log("Puntaje borrado.");

        // 5. Realiza la acci�n final (cambio de escena o salida del juego)
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"Cambiando a la escena: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
        else if (exitGameAfter)
        {
            Debug.Log($"Saliendo del juego en {exitDelay} segundos...");
            yield return new WaitForSeconds(exitDelay);
            Application.Quit();

            // Necesario para detener el modo de juego en el editor
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        else
        {
            Debug.Log("No se defini� ninguna acci�n posterior.");
        }
    }

    private System.Collections.IEnumerator PlayEvent(string eventPath)
    {
        // Crear una instancia del evento
        FMOD.Studio.EventInstance instance = RuntimeManager.CreateInstance(eventPath);

        bool isPlaying = true;

        // Callback para detectar cu�ndo el evento finaliza
        instance.setCallback((FMOD.Studio.EVENT_CALLBACK_TYPE type, System.IntPtr eventInstance, System.IntPtr parameters) =>
        {
            if (type == FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED)
            {
                isPlaying = false;
            }
            return FMOD.RESULT.OK;
        }, FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED);

        // Iniciar el evento
        instance.start();

        // Esperar a que termine
        while (isPlaying)
        {
            yield return null;
        }

        // Liberar la instancia
        instance.release();
    }

    private System.Collections.IEnumerator PlayEventWithParameter(string eventPath, string parameterName, float parameterValue)
    {
        // Crear una instancia del evento
        FMOD.Studio.EventInstance instance = RuntimeManager.CreateInstance(eventPath);

        // Configurar el par�metro
        instance.setParameterByName(parameterName, parameterValue);

        bool isPlaying = true;

        // Callback para detectar cu�ndo el evento finaliza
        instance.setCallback((FMOD.Studio.EVENT_CALLBACK_TYPE type, System.IntPtr eventInstance, System.IntPtr parameters) =>
        {
            if (type == FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED)
            {
                isPlaying = false;
            }
            return FMOD.RESULT.OK;
        }, FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED);

        // Iniciar el evento
        instance.start();

        // Esperar a que termine
        while (isPlaying)
        {
            yield return null;
        }

        // Liberar la instancia
        instance.release();
    }
}
