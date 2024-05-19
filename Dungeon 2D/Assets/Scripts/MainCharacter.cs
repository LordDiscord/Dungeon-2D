using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : Character
{

    public Animator anim;
    private bool guerrero = true; // esto es la clase default para esta prueba, luego se popdrán escoger

    protected virtual void Awake()
    {
        // Inicializa las estadísticas básicas
        vida = Random.Range(15, 21);
        armadura = 10;
        destreza = 10;
        inteligencia = 10;
        iniciativa = destreza / 2 + tirarD6(); // posible cambio (6~12)
        nombre = "player";
        if (guerrero)
        {
            armadura += 4;
            destreza += 2;
            inteligencia -= 4;
            vida += 5;
            iniciativa -= 3;
        }
    }

    protected virtual void Start()
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
                Debug.Log("HAS MUERTO");
                Destroy(gameObject);
                yield break;
            }
            else
            {
                anim.SetBool("Dead", false);
                if (vida < vidaActual)
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

    public void AttackUp() // activar animaciones
    {
        anim.SetBool("AttackUp", true);
    }
    public void NoAttackUp()
    {
        anim.SetBool("AttackUp", false);
    }
    public void AttackDown()
    {
        anim.SetBool("AttackDown", true);
    }
    public void NoAttackDown()
    {
        anim.SetBool("AttackDown", false);
    }
    public void AttackRight()
    {
        anim.SetBool("AttackRight", true);
        Debug.Log("Yes");
    }
    public void NoAttackRight()
    {
        anim.SetBool("AttackRight", false);
        Debug.Log("No");
    }
}