using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    protected string nombre;
    //stats
    protected int vida;
    protected int armadura;
    protected int destreza;
    protected int inteligencia;
    protected int iniciativa;
    //items
    protected int monedasDeOro;
    //otros
    protected int dado;
    protected int dadoExtra;
    protected bool ventaja;


    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public int tirarD20()
    {
        return Random.Range(1, 21);     
    }

    public int tirarD6()
    {
        return Random.Range(1, 7);
    }

    public int tirarD4 ()
    {
        return Random.Range(1, 5);
    }

    public void atacarDestreza(Character target)
    {
        if (ventaja)
        {
            dado = tirarD20();
            dadoExtra = tirarD20();
            if (dado < dadoExtra)
            {
                dado = dadoExtra;
            }
        }
        dado += destreza;
        if (target.armadura <= dado)
        {
            recibirDaño(target);
            //mostrar tirada por pantalla
        }
    }
    public void atacarInteligencia(Character target)
    {
        if (ventaja)
        {
            dado = tirarD20();
            dadoExtra = tirarD20();
            if (dado < dadoExtra)
            {
                dado = dadoExtra;
            }
        }
        dado += inteligencia / 4;
        if (target.armadura <= dado)
        {
            recibirDaño(target);
            //mostrar tirada por pantalla
        }
    }
    public virtual void recibirDaño(Character target)
    {
        int daño = tirarD6() + destreza / 4;
        target.vida = vida - daño;
    }

}
