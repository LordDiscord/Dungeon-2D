using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Character
{
    // Start is called before the first frame update

    public Goblin(bool mago)
    {
        vida = 5 + tirarD4();
        armadura = 10 + tirarD4();
        destreza = 10 + tirarD6();
        iniciativa = destreza / 2 + tirarD6();
        inteligencia = 4 + tirarD6();
        if (mago)
        {
            vida -= 2;
            armadura -= 4;
            destreza -= 3;
            inteligencia += 5;
        }
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
