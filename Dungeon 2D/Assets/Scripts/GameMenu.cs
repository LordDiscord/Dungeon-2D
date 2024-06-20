using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameMenu : MonoBehaviour
{
    public void OnQuitButtonClicked()
    {
        GameManager.instance.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }
}
