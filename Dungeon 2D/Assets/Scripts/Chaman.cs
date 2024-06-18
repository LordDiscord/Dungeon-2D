using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaman : Goblin
{

    protected override void Awake()
    {
        base.Awake();
        health -= 2;
        armor -= 4;
        dexterity -= 3;
        intelligence += 5;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
