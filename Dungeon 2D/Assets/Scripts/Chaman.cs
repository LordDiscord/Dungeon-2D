using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaman : Goblin
{

    protected override void Awake()
    {
        base.Awake();
        vida -= 2;
        armadura -= 4;
        destreza -= 3;
        inteligencia += 5;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
