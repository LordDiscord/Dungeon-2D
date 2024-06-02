using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public string playScene; // Nombre de la escena a cargar
    public void PlayGame()
    {
        SceneManager.LoadScene(playScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
