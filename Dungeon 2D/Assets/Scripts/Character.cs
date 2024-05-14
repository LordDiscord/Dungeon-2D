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

}
