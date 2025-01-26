using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escenas

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private float delayBeforeChange = 5f; // Tiempo en segundos antes de cambiar de escena
    [SerializeField] private string sceneToLoad = "SceneName"; // Nombre de la escena que se cargará

    private float timer;

    void Start()
    {
        // Inicializar el temporizador
        timer = 0f;
    }

    void Update()
    {
        // Incrementar el temporizador en función del tiempo transcurrido
        timer += Time.deltaTime;

        // Si el temporizador supera el retraso configurado, cambia de escena
        if (timer >= delayBeforeChange)
        {
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("No se ha asignado un nombre de escena válido.");
        }
    }
}
