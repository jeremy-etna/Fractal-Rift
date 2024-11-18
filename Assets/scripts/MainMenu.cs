using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGameMulti()
    {
        // Changer le comportement pour le multi ici !
        SceneManager.LoadSceneAsync("BattleScene");
    }
    public void PlayGameTuto()
    {
        // changer le comportement pour le tuto
        SceneManager.LoadSceneAsync("BattleScene");
    }
    public void GoToMainMenuScene()
    {
        SceneManager.LoadSceneAsync("MainMenuScene");
    }
    public void GoToTeamBuilder()
    {
        SceneManager.LoadSceneAsync("TeamBuilder");
    }
    public void GoToOptions()
    {
        SceneManager.LoadSceneAsync("Options");
    }
    public void GoToPlayerStats()
    {
        SceneManager.LoadSceneAsync("PlayerStats");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}