using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PortalCombat : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto en contacto es el jugador
        if (other.CompareTag("Player"))
        {
            // Cargar la escena especificada
            if (GameManager.instance.level == 10)
            {
                GameManager.instance.LoadScene("Win scene");
            }
            else
            {
                GameManager.instance.LoadScene("Sala Combate niv1");
            }
        }
    }
}
