using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : Character
{
    public static MainCharacter Instance { get; private set; }

    public Animator anim;
    private bool guerrero = true; // esto es la clase default para esta prueba, luego se popdrán escoger

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadStats(); // Cargar estadísticas guardadas
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    protected virtual void Start()
    {
        if (Instance == this)
        {
            LoadStats();
            if (vida <= 0)
            {
                GenerateNewStats();
            }
            vidaActual = vida;
            StartCoroutine(IsDead());
        }
    }

    IEnumerator IsDead()
    {
        while (true)
        {
            if (vida <= 0)
            {
                anim.SetBool("Dead", true);
                Debug.Log("HAS MUERTO");
                yield return new WaitForSeconds(3f);
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
    private void OnDestroy()
    {
        if (Instance == this && vida > 0)
        {
            SaveStats(); // Guardar estadísticas cuando el objeto se destruye
        }
    }

    public void SaveStats()
    {
        PlayerPrefs.SetInt("vida", vida);
        PlayerPrefs.SetInt("armadura", armadura);
        PlayerPrefs.SetInt("destreza", destreza);
        PlayerPrefs.SetInt("inteligencia", inteligencia);
        PlayerPrefs.SetInt("iniciativa", iniciativa);
        PlayerPrefs.SetString("nombre", nombre);
        PlayerPrefs.SetInt("vidaActual", vidaActual);
        PlayerPrefs.Save();
        Debug.Log("GUARDANDO");
    }

    public void LoadStats()
    {
        if (PlayerPrefs.HasKey("vida"))
        {
            vida = PlayerPrefs.GetInt("vida");
            armadura = PlayerPrefs.GetInt("armadura");
            destreza = PlayerPrefs.GetInt("destreza");
            inteligencia = PlayerPrefs.GetInt("inteligencia");
            iniciativa = PlayerPrefs.GetInt("iniciativa");
            nombre = PlayerPrefs.GetString("nombre");
            vidaActual = PlayerPrefs.GetInt("vidaActual");
            Debug.Log("CARGANDO");
        }
    }

    public void GenerateNewStats()
    {
        // Inicializa las estadísticas básicas
        vida = Random.Range(15, 21);
        armadura = 10;
        destreza = 10;
        inteligencia = 10;
        iniciativa = destreza / 2 + tirarD6(); // Posible cambio (6~12)
        nombre = "player";
        if (guerrero)
        {
            armadura += 4;
            destreza += 2;
            inteligencia -= 4;
            vida += 5;
            iniciativa -= 3;
        }
        vidaActual = vida;
        Debug.Log("GENERANDO");
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