using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guerrero : MainCharacter
{
    protected override void Awake()
    {
        base.Awake();

        armadura += 4;
        destreza += 2;
        inteligencia -= 4;
    }

    protected override void Start()
    {
        base.Start();
        vida += 5;
        iniciativa -= 3;
    }
}