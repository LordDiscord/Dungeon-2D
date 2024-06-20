using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPotion : Item
{
    public int ArmorAmount = 2; // Cantidad de salud que restaura esta poci�n

    // Constructor para inicializar el tipo y nombre de la poci�n de salud
    public ArmorPotion()
    {
        Type = "ArmorPotion";
        Name = "ArmorPotion";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MainCharacter player = other.GetComponent<MainCharacter>();
            if (player != null)
            {
                player.AddItem(this); // Llamar al m�todo correcto para a�adir la poci�n al inventario
                Destroy(gameObject); // Destruir la poci�n de vida despu�s de ser recogida
            }
        }
    }
}
