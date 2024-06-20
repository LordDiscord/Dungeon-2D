using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPotion : Item
{
    public int AttackAmount = 1; // Cantidad de salud que restaura esta poción

    // Constructor para inicializar el tipo y nombre de la poción de salud
    public AttackPotion()
    {
        Type = "AttackPotion";
        Name = "AttackPotion";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MainCharacter player = other.GetComponent<MainCharacter>();
            if (player != null)
            {
                player.AddItem(this); // Llamar al método correcto para añadir la poción al inventario
                Destroy(gameObject); // Destruir la poción de vida después de ser recogida
            }
        }
    }
}
