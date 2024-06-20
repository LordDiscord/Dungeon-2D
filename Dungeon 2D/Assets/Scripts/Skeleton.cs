using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Pool;

public class Skeleton : Character
{
    public GameObject[] potionPrefabs; // Array de prefabs de las pociones

    protected virtual void Awake()
    {
        check = false;
        speed = 4;
        attacks = 2;
        health = 5 + throwD6();
        armor = 9 + throwD4();
        dexterity = 9 + throwD4();
        initiative = dexterity / 2 + throwD4();
        intelligence = throwD6();
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
                popUpText.text = (currentHealth - health).ToString();
                Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                Instantiate(popUpDamagePrefab, newPosition, Quaternion.identity);
                anim.SetBool("Dead", true);
                yield return new WaitForSeconds(1f);

                // Spawnear 1 pocion
                SpawnPotion();

                Destroy(gameObject);
                yield break;
            }
            else
            {
                anim.SetBool("Dead", false);
                if (health < currentHealth)
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

    void SpawnPotion()
    {
        // Elegir aleatoriamente un prefab de poción
        GameObject potionPrefab = potionPrefabs[Random.Range(0, potionPrefabs.Length)];

        // Mantener la misma posicion que el enemigo
        Vector3 spawnPosition = transform.position;

        // Instanciar la poción
        Instantiate(potionPrefab, spawnPosition, Quaternion.identity);
    }
}
