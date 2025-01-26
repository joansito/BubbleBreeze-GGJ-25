using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int points)
    {
        score += points;
        Debug.Log("Puntos totales: " + score);

        // Guardar el puntaje en PlayerPrefs
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }

    public int GetScore()
    {
        return score;
    }
}
