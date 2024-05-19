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
    protected int da�o;

    // M�todo para tirar un D20
    public int tirarD20()
    {
        return Random.Range(1, 21);
    }

    // M�todo para tirar un D6
    public int tirarD6()
    {
        return Random.Range(1, 7);
    }

    // M�todo para tirar un D4
    public int tirarD4()
    {
        return Random.Range(1, 5);
    }

    // M�todo para atacar usando destreza
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
            recibirDa�o(target, destreza);
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

    // M�todo para atacar usando inteligencia
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
            recibirDa�o(target, inteligencia);
            // mostrar tirada por pantalla
        }
    }

    // M�todo virtual para recibir da�o
    public virtual void recibirDa�o(Character target, int tipo)
    {
        da�o = tirarD6() + (tipo - 10) / 2;
        target.vida -= da�o;
        if(target.nombre != "player") //Todo esto es temporal y se cambiara cuando se haga una interfaz
        {
            Debug.Log("EL ATAQUE A FUNCIONADO, HAS SASCADO UN " + dado + " Y A SUPERADO LOS " + target.armadura + " DE ARMADURA DEL ENEMIGO HACIENDOLE " + da�o + " PUNTOS DE DA�O, SU VIDA ACTUAL ES DE " + target.vida);
        }
        else
        {
            Debug.Log("EL ENEMIGO TE HA HERIDO SACANDO UN  "+ dado + " Y A SUPERADO TUS " + target.armadura + " DE ARMADURA HACIENDOTE " + da�o + " PUNTOS DE DA�O, TU VIDA ACTUAL ES DE " + target.vida);
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