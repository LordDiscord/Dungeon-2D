using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void OnStartGameButtonClicked()
    {
        GameManager.instance.LoadScene("3c's scene");
    }

    public void OnQuitButtonClicked()
    {
        GameManager.instance.QuitGame();
    }
}
