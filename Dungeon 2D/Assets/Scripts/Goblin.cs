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
        health = 3 + throwD6();
        armor = 10 + throwD4();
        dexterity = 10 + throwD6();
        initiative = dexterity / 2 + throwD6();
        intelligence = 4 + throwD6();
     }

    void Start()
    {
        currentHealth = health;
        StartCoroutine(IsDead());
    }

    IEnumerator IsDead()
    {
        while (true)
        {
            if (health <= 0)
            {
                popUpText.text = (currentHealth-health).ToString();
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
                if(health < currentHealth)
                {
                    anim.SetBool("IsDamaged", true);
                    popUpText.text = (currentHealth - health).ToString();
                    Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                    Instantiate(popUpDamagePrefab, newPosition, Quaternion.identity); yield return new WaitForSeconds(0.2f);
                    anim.SetBool("IsDamaged", false);

                    currentHealth = health;
                }
            }
            yield return null;
        }
    }
}
