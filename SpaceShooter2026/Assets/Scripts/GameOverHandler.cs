using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    public void onPlayAgainPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void onMainMenuPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
