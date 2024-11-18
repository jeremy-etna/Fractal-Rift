using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private GameObject GameOverVictoryBackground;
    [SerializeField] private GameObject GameOverDefeatBackground;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button restartButton;
    
    public bool isVictory;

    void Start()
    {
        GameOverCanvas.SetActive(false);
        GameOverVictoryBackground.SetActive(false);
        GameOverDefeatBackground.SetActive(false);
        
        quitButton.onClick.AddListener(QuitGame);
        restartButton.onClick.AddListener(RestartGame);
    }
    
    public void ShowGameOver(bool gameState)
    {
        GameOverCanvas.SetActive(true);
        isVictory = gameState;
        if (isVictory)
            ShowVictory();
        else
            ShowDefeat();
    }

    public void ShowVictory()
    {
        GameOverVictoryBackground.SetActive(true);
        GameOverDefeatBackground.SetActive(false);
    }

    public void ShowDefeat()
    {
        GameOverVictoryBackground.SetActive(false);
        GameOverDefeatBackground.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

