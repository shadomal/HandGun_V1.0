using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public GameObject QuitButton;
    public void Menu()
    {
        SceneManager.LoadScene("HomeMenu");
    }
    public void Options()
    {
        SceneManager.LoadScene("MenuOptions");
    }
    public void QuitGame()
    {
        bool IsQuitGame = false;
        QuitButton.SetActive(true);
        if (IsQuitGame)
        {
            Application.Quit();
        }
    }
    public void IngameScene()
    {
        SceneManager.LoadScene("MapaBlocagem");
    }
}
