using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIController : MonoBehaviour
{

    public TextMeshProUGUI movText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI hpText2;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI armorText2;
    public TextMeshProUGUI dexText;
    public TextMeshProUGUI intText;
    public TextMeshProUGUI initText;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI healthCount;
    public TextMeshProUGUI armorCount;
    public TextMeshProUGUI speedCount;
    public TextMeshProUGUI attackCount;
    private int attacksCount = 0;

    public MainCharacter player; // Referencia al objeto jugador
    public PlayerGridController movimiento; // Referencia al objeto jugador
    public BattleSystem turno;

    void Start()
    {
        // Buscar al jugador en la escena al iniciar
        player = FindObjectOfType<MainCharacter>();
        movimiento = FindObjectOfType<PlayerGridController>();
    }

    void Update()
    {
        if (player != null)
        {
            // Actualizar los textos con los valores de las estadísticas del jugador
            attacksCount = player.GetAttacks() - turno.playerAttack;

            turnText.text = "CURRENT TURN: " + turno.state;
            hpText.text = "HEALTH: " + player.GetHealth().ToString();
            hpText2.text = "HP: " + player.GetHealth().ToString();
            armorText.text = "ARMOR: " + player.GetArmor().ToString();
            armorText2.text = "ARMOR: " + player.GetArmor().ToString();
            dexText.text = "DEXTERITY: " + player.GetDexterity().ToString();
            intText.text = "INTELIGENCE: " + player.GetIntelligence().ToString();
            initText.text = "INITIATIVE: " + player.GetInitiative().ToString();

            //Inventory
            healthCount.text = player.GetItemCount("HealthPotion").ToString();
            armorCount.text = player.GetItemCount("ArmorPotion").ToString();
            speedCount.text = player.GetItemCount("SpeedPotion").ToString();
            attackCount.text = player.GetItemCount("AttackPotion").ToString();
        }
        else
        {
            // Intentar encontrar al jugador si aún no se ha encontrado
            player = FindObjectOfType<MainCharacter>();
        }

        if (movimiento != null)
        {
            if (turno.state != BattleState.PLAYERTURN)
            {
                movText.text = "MOVEMENT: 0";
                attackText.text = "ATTACKS: 0";
            }
            else
            {
                movText.text = "MOVEMENT: " + (player.GetSpeed() - movimiento.numMovimiento).ToString();
                attackText.text = "ATTACKS: " + attacksCount;
            }
        }
        else
        {
            movimiento = FindObjectOfType<PlayerGridController>();
        }
    }

    public void UseHealthPotion()
    {
        player.UseItem("HealthPotion");
    }
    public void UseArmorPotion()
    {
        player.UseItem("ArmorPotion");
    }
    public void UseAttackPotion()
    {
        player.UseItem("AttackPotion");
    }
    public void UseSpeedPotion()
    {
        player.UseItem("SpeedPotion");
    }
}
