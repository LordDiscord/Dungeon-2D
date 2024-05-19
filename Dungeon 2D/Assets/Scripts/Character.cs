using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected string nombre;
    // stats
    protected int vida;
    protected int vidaActual;
    protected int armadura;
    protected int destreza;
    protected int inteligencia;
    protected int iniciativa;
    // items
    protected int monedasDeOro;
    // otros
    protected int dado;
    protected int dadoExtra;
    protected bool ventaja;
    protected int daño;

    // Método para tirar un D20
    public int tirarD20()
    {
        return Random.Range(1, 21);
    }

    // Método para tirar un D6
    public int tirarD6()
    {
        return Random.Range(1, 7);
    }

    // Método para tirar un D4
    public int tirarD4()
    {
        return Random.Range(1, 5);
    }

    // Método para atacar usando destreza
    public void atacarDestreza(Character target)
    {
        dado = tirarD20();
        if (ventaja)
        {
            dadoExtra = tirarD20();
            if (dado < dadoExtra)
            {
                dado = dadoExtra;
            }
        }
        dado += (destreza-10) / 2; //segun las normas de D&D
        if (target.armadura <= dado)
        {
            recibirDaño(target, destreza);
            // mostrar tirada por pantalla
        }
        else
        {
            if(target.nombre != "player") //Todo esto es temporal y se cambiara cuando se haga una interfaz
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
    public void atacarInteligencia(Character target)
    {
        dado = tirarD20();
        if (ventaja)
        {
            dadoExtra = tirarD20();
            if (dado < dadoExtra)
            {
                dado = dadoExtra;
            }
        }
        dado += (inteligencia-10) / 2; //segun las normas de D&D
        if (target.armadura <= dado)
        {
            recibirDaño(target, inteligencia);
            // mostrar tirada por pantalla
        }
    }

    // Método virtual para recibir daño
    public virtual void recibirDaño(Character target, int tipo)
    {
        daño = tirarD6() + (tipo - 10) / 2;
        target.vida -= daño;
        if(target.nombre != "player") //Todo esto es temporal y se cambiara cuando se haga una interfaz
        {
            Debug.Log("EL ATAQUE A FUNCIONADO, HAS SASCADO UN " + dado + " Y A SUPERADO LOS " + target.armadura + " DE ARMADURA DEL ENEMIGO HACIENDOLE " + daño + " PUNTOS DE DAÑO, SU VIDA ACTUAL ES DE " + target.vida);
        }
        else
        {
            Debug.Log("EL ENEMIGO TE HA HERIDO SACANDO UN  "+ dado + " Y A SUPERADO TUS " + target.armadura + " DE ARMADURA HACIENDOTE " + daño + " PUNTOS DE DAÑO, TU VIDA ACTUAL ES DE " + target.vida);
        }
    }

    public int GetVida()
    {
        return vida;
    }

    public int GetDestreza()
    {
        return destreza;
    }

    public int GetInteligencia()
    {
        return inteligencia;
    }

    public int GetIniciativa()
    {
        return iniciativa;
    }

    public int GetArmadura()
    {
        return armadura;
    }
}