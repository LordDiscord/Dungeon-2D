using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MainCharacter : Character
{

    public static MainCharacter instance; // Singleton instance

    public Animator anim;
    private bool fighter = true; // Esto es la clase default para esta prueba, luego se podrán escoger

    protected virtual void Awake()
    {
        // Implementación del Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Destruir duplicados
            return;
        }
    }

    protected virtual void Start()
    {
        if (instance == this) // Solo inicializar si este objeto es la instancia
        {
            GenerateNewStats();
            currentHealth = health;
            StartCoroutine(IsDead());
        }
    }

    IEnumerator IsDead()
    {
        while (true)
        {
            if (health <= 0)
            {
                popUpText.text = (currentHealth - health).ToString();
                Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                Instantiate(popUpDamagePrefab, newPosition, Quaternion.identity); anim.SetBool("Dead", true);
                Debug.Log("HAS MUERTO");
                yield return new WaitForSeconds(2f);
                Destroy(gameObject);
                yield break;
            }
            else
            {
                anim.SetBool("Dead", false);
                if (health < currentHealth)
                {
                    popUpText.text = (currentHealth - health).ToString();
                    Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                    Instantiate(popUpDamagePrefab, newPosition, Quaternion.identity); anim.SetBool("IsDamaged", true);
                    yield return new WaitForSeconds(0.2f);
                    anim.SetBool("IsDamaged", false);
                    currentHealth = health;
                }
            }
            yield return null;
        }
    }
    public void GenerateNewStats()
    {
        // Inicializa las estadísticas básicas
        health = Random.Range(15, 21);
        armor = 10;
        dexterity = 10;
        intelligence = 10;
        initiative = dexterity / 2 + throwD6(); // Posible cambio (6~12)
        name = "player";
        if (fighter)
        {
            armor += 4;
            dexterity += 2;
            intelligence -= 4;
            health += 5;
            initiative -= 3;
        }
        currentHealth = health;
        Debug.Log("GENERANDO");
    }

    public void RespawnAt(Vector3 position)
    {
        transform.position = position;
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
    }
    public void NoAttackRight()
    {
        anim.SetBool("AttackRight", false);
    }
}