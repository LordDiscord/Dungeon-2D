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
            if(turno.playerAttack == false && turno.nameState == "Player")
            {
                attackText.text = "ATTACK: 1";
            }
            else
            {
                attackText.text = "ATTACK: 0";
            }
            turnText.text = "CURRENT TURN: " + turno.nameState;
            hpText.text = "HEALTH: " + jugador.GetVida().ToString();
            hpText2.text = "HP: " + jugador.GetVida().ToString();
            armorText.text = "ARMOR: " + jugador.GetArmadura().ToString();
            armorText2.text = "ARMOR: " + jugador.GetArmadura().ToString();
            dexText.text = "DEXTERITY: " + jugador.GetDestreza().ToString();
            intText.text = "INTELIGENCE: " + jugador.GetInteligencia().ToString();
            initText.text = "INITIATIVE: " + jugador.GetIniciativa().ToString();
        }
        else
        {
            // Intentar encontrar al jugador si aún no se ha encontrado
            jugador = FindObjectOfType<MainCharacter>();
        }

        if (movimiento != null)
        {
            if (turno.nameState != "Player")
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
