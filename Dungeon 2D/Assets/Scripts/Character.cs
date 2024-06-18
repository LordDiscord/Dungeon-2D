using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected new string name;
    // stats
    protected int health;
    protected int currentHealth;
    protected int armor;
    protected int dexterity;
    protected int intelligence;
    protected int initiative;
    // items
    protected int goldCoins;
    // otros
    protected int dice;
    protected int extraDice;
    protected bool advantadge;
    protected int damage;

    // Método para tirar un D20
    public int throwD20()
    {
        return Random.Range(1, 21);
    }

    // Método para tirar un D6
    public int throwD6()
    {
        return Random.Range(1, 7);
    }

    // Método para tirar un D4
    public int throwD4()
    {
        return Random.Range(1, 5);
    }

    // Método para atacar usando destreza
    public void attackDex(Character target)
    {
        dice = throwD20();
        if (advantadge)
        {
            extraDice = throwD20();
            if (dice < extraDice)
            {
                dice = extraDice;
            }
        }
        dice += (dexterity-10) / 2; //segun las normas de D&D
        if (target.armor <= dice)
        {
            takeDamage(target, dexterity);
            // mostrar tirada por pantalla
        }
        else
        {
            if(target.name != "player") //Todo esto es temporal y se cambiara cuando se haga una interfaz
            {
                Debug.Log("OH NO EL ATAQUE A FALLADO!");
            }
            else
            {
                Debug.Log("HAS ESQUIVADO EL ATAQUE");
            }
        }
    }

    // Método para atacar usando inteligencia
    public void attackInt(Character target)
    {
        dice = throwD20();
        if (advantadge)
        {
            extraDice = throwD20();
            if (dice < extraDice)
            {
                dice = extraDice;
            }
        }
        dice += (intelligence-10) / 2; //segun las normas de D&D
        if (target.armor <= dice)
        {
            takeDamage(target, intelligence);
            // mostrar tirada por pantalla
        }
    }

    // Método virtual para recibir daño
    public virtual void takeDamage(Character target, int type)
    {
        damage = throwD6() + (type - 10) / 2;
        target.health -= damage;
        if(target.name != "player") //Todo esto es temporal y se cambiara cuando se haga una interfaz
        {
            Debug.Log("EL ATAQUE A FUNCIONADO, HAS SASCADO UN " + dice + " Y A SUPERADO LOS " + target.armor + " DE ARMADURA DEL ENEMIGO HACIENDOLE " + damage + " PUNTOS DE DAÑO, SU VIDA ACTUAL ES DE " + target.health);
        }
        else
        {
            Debug.Log("EL ENEMIGO TE HA HERIDO SACANDO UN  "+ dice + " Y A SUPERADO TUS " + target.armor + " DE ARMADURA HACIENDOTE " + damage + " PUNTOS DE DAÑO, TU VIDA ACTUAL ES DE " + target.health);
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetDexterity()
    {
        return dexterity;
    }

    public int GetIntelligence()
    {
        return intelligence;
    }

    public int GetInitiative()
    {
        return initiative;
    }

    public int GetArmor()
    {
        return armor;
    }
}