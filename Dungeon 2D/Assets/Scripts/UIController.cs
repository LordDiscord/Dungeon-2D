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

    public MainCharacter jugador; // Referencia al objeto jugador
    public PlayerGridController movimiento; // Referencia al objeto jugador
    public BattleSystem turno;

    void Start()
    {
        // Buscar al jugador en la escena al iniciar
        jugador = FindObjectOfType<MainCharacter>();
        movimiento = FindObjectOfType<PlayerGridController>();
    }

    void Update()
    {
        if (jugador != null)
        {
            // Actualizar los textos con los valores de las estadísticas del jugador
            if(turno.playerAttack == false && turno.state == BattleState.PLAYERTURN)
            {
                attackText.text = "ATTACK: 1";
            }
            else
            {
                attackText.text = "ATTACK: 0";
            }
            turnText.text = "CURRENT TURN: " + turno.state;
            hpText.text = "HEALTH: " + jugador.GetHealth().ToString();
            hpText2.text = "HP: " + jugador.GetHealth().ToString();
            armorText.text = "ARMOR: " + jugador.GetArmor().ToString();
            armorText2.text = "ARMOR: " + jugador.GetArmor().ToString();
            dexText.text = "DEXTERITY: " + jugador.GetDexterity().ToString();
            intText.text = "INTELIGENCE: " + jugador.GetIntelligence().ToString();
            initText.text = "INITIATIVE: " + jugador.GetInitiative().ToString();
        }
        else
        {
            // Intentar encontrar al jugador si aún no se ha encontrado
            jugador = FindObjectOfType<MainCharacter>();
        }

        if (movimiento != null)
        {
            if (turno.state != BattleState.PLAYERTURN)
            {
                movText.text = "MOVEMENT: 0";
            }
            else
            {
                movText.text = "MOVEMENT: " + (6 - movimiento.numMovimiento).ToString();
            }
        }
        else
        {
            movimiento = FindObjectOfType<PlayerGridController>();
        }
    }
}
