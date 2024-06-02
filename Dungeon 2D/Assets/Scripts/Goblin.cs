using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Pool;

public class Goblin : Character
{
    public Animator anim;
    public bool check = false;
    public GameObject popUpDamagePrefab;
    public TMP_Text popUpText;

    protected virtual void Awake()
    {
        vida = 1;
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
                popUpText.text = (vidaActual-vida).ToString();
                Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                Instantiate(popUpDamagePrefab, newPosition, Quaternion.identity);
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
                    popUpText.text = (vidaActual - vida).ToString();
                    Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                    Instantiate(popUpDamagePrefab, newPosition, Quaternion.identity); yield return new WaitForSeconds(0.2f);
                    anim.SetBool("IsDamaged", false);

                    vidaActual = vida;
                }
            }
            yield return null;
        }
    }
}
