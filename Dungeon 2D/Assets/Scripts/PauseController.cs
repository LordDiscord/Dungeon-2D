using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject menu; 
    public GameObject controlsMenu;
    public GameObject inventoryMenu;
    void Update()
    {
        // Abrir o cerrar el menú al presionar la tecla "Esc"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menu.activeSelf && !controlsMenu.activeSelf && !inventoryMenu.activeSelf)
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
        // Activar el menú
        menu.SetActive(true);
        // Detener el tiempo del juego
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
        }
        else if (controlsMenu.activeSelf)
        {
            controlsMenu.SetActive(false);
        }
        else if (inventoryMenu.activeSelf)
        {
            inventoryMenu.SetActive(false);
        }
        // Reanudar el tiempo del juego
        Time.timeScale = 1f;
    }
}