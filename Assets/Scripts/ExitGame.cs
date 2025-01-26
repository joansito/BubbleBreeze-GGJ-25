using UnityEngine;

public class ExitGame : MonoBehaviour
{
    void Update()
    {
        // Detecta si la tecla Esc es presionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Salir del juego si estamos en una compilación
            Application.Quit();

            // Mensaje útil para el editor de Unity
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
