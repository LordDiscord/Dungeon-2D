using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject menu; // Referencia al objeto del men�
    private bool menuOpen = false; // Estado del men�

    void Update()
    {
        // Abrir o cerrar el men� al presionar la tecla "Esc"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuOpen)
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
        // Activar el men� y actualizar el estado
        menu.SetActive(true);
        menuOpen = true;
    }

    void CloseMenu()
    {
        // Desactivar el men� y actualizar el estado
        menu.SetActive(false);
        menuOpen = false;
    }
}
