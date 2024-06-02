using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PortalCombat : MonoBehaviour
{
    public string sceneToLoad; // Nombre de la escena a cargar

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto en contacto es el jugador
        if (other.CompareTag("Player"))
        {
            // Cargar la escena especificada
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
