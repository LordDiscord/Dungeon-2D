using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Goblin : Character
{
    public Animator anim;
    public bool check = false;

    protected virtual void Awake()
    {
        vida = 5 + tirarD4();
        armadura = 10 + tirarD4();
        destreza = 10 + tirarD6();
        iniciativa = destreza / 2 + tirarD6();
        inteligencia = 4 + tirarD6();
     }

    void Start()
    {
        vidaActual = vida;
        StartCoroutine(IsDead());
    }

    IEnumerator IsDead()
    {
        while (true)
        {
            if (vida <= 0)
            {
                anim.SetBool("Dead", true);
                yield return new WaitForSeconds(1f);
                Debug.Log("HaMuerto");
                Destroy(gameObject);
                yield break;
            }
            else
            {
                anim.SetBool("Dead", false);
                if(vida < vidaActual)
                {
                    anim.SetBool("IsDamaged", true);
                    yield return new WaitForSeconds(0.2f);
                    anim.SetBool("IsDamaged", false);

                    vidaActual = vida;
                }
            }
            yield return null;
        }
    }
}
