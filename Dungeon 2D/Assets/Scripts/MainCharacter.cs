using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MainCharacter : Character
{
    
    public MainCharacter() 
    {
        vida = Random.Range(15, 21);
        armadura = 10;
        destreza = 10;
        inteligencia = 10;
        iniciativa = destreza / 2 + tirarD6(); //posible cambio (6~12)
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
