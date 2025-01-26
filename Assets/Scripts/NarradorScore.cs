using UnityEngine;

public class NarradorScore : MonoBehaviour
{
    [SerializeField] private NumberNarrator narrator; // Referencia al componente del narrador

    void Start()
    {
        // Recuperar el puntaje desde PlayerPrefs
        int score = PlayerPrefs.GetInt("Score", 0); // Si no hay puntaje guardado, devuelve 0

        Debug.Log("Puntaje recuperado: " + score);

        // Llamar al narrador para leer el puntaje
        if (narrator != null)
        {
            narrator.NarrateNumber(score);
        }
        else
        {
            Debug.LogError("No se asignó el componente NumberNarrator.");
        }
    }
}
