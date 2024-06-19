using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject menu; // Referencia al objeto del men�

    void Update()
    {
        // Abrir o cerrar el men� al presionar la tecla "Esc"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menu.activeSelf)
            {
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }
        }
    }

    void OpenMenu()
    {
        // Activar el men�
        menu.SetActive(true);
        // Detener el tiempo del juego
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        // Desactivar el men�
        menu.SetActive(false);
        // Reanudar el tiempo del juego
        Time.timeScale = 1f;
    }
}