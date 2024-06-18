using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST } //Maquina de Estados

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject enemyAiPrefab;

    public MainCharacter playerCharacter; // Declarar como variable de nivel de clase
    public EnemyAI enemyAI;
    public List<Goblin> enemies = new List<Goblin>();

    private GridManager gridManager;
    public string nameState;
    public Transform[] enemySpawns;
    public Transform playerSpawn;
    public bool canMove = false;
    public bool reset = false;
    public bool victoria = false;
    public bool playerAttack = false;

    public BattleState state;

    void Start()
    {
        gridManager = GetComponent<GridManager>();
        GameObject playerObject = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        playerCharacter = playerObject.GetComponent<MainCharacter>();

        int numEnemies = Random.Range(1, 2); // cuantos enemigos quieres?

        for (int j = 0; j < numEnemies; j++) //establece los enemigos en los puntos de spawn
        {
            if (j < enemySpawns.Length)
            {
                GameObject enemyObject = Instantiate(enemyPrefab, enemySpawns[j].position, Quaternion.identity);
                Goblin enemyCharacter = enemyObject.GetComponent<Goblin>();
                enemies.Add(enemyCharacter);
            }
        }
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        enemyAI = new GameObject("EnemyAI").AddComponent<EnemyAI>();
        enemyAI.Initialize(gridManager, this, enemies[0]); // Aqu� debes pasar los par�metros correctos seg�n tu implementaci�n
        enemyAI.StartEnemyAI();
    }

    void Update()
    {
        if (state == BattleState.PLAYERTURN) //Depende de que estado este activo va comprobando uno u otro
        {
            PlayerTurn();
        }
        if (state == BattleState.WON)
        {
            StartCoroutine(EndBattle());
        }
        if (state == BattleState.LOST)
        {
            StartCoroutine(LostBattle());
        }
    }

    IEnumerator SetupBattle()
    {
        nameState = "SetupBattle";
        yield return new WaitForSeconds(2f);
        Debug.Log("Iniciativa Jugador: " + playerCharacter.GetInitiative()); // esto iria en el ui
        bool playerFirst = true;
        foreach (Goblin enemy in enemies)
        {
            if (enemy.GetInitiative() > playerCharacter.GetInitiative())
            {
                playerFirst = false;
                break;
            }
        }
        if (playerFirst)
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        else
        {
            state = BattleState.ENEMYTURN;
        }
    }

    IEnumerator LostBattle()
    {
        nameState = "Battle Lost";
        canMove = false;
        yield return new WaitForSeconds(2f);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }

    IEnumerator EndBattle()
    {
        nameState = "Battle Won";
        canMove = true;
        yield return new WaitForSeconds(2f);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }

    void PlayerTurn()
    {
        nameState = "Player";
        reset = false;
        canMove = true;
        StartCoroutine(PlayerAttackCheck()); //Se ocupa de comprobar que al atacar con el click del raton este a rango y lo ejecuta si asi es
        if (Input.GetKeyDown(KeyCode.T) && !victoria) //Pulsando la tecla T pasas el turno al enemigo
        {
            state = BattleState.ENEMYTURN;
        }
    }

    IEnumerator PlayerAttackCheck()
    {
        if (Input.GetMouseButtonDown(0) && !playerAttack) // Verifica si el jugador hizo clic y no ha atacado a�n en este turno
        {
            Vector2 playerPos = playerCharacter.transform.position;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Convierte la posici�n del clic del rat�n a coordenadas del mundo
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero); // Realiza un rayo desde la posici�n del rat�n
            if (hit.collider != null && hit.collider.CompareTag("Enemy")) // Si el rayo choca con un colisionador de enemigo
            {
                GameObject enemyGameObject = hit.collider.gameObject;
                Goblin enemy = enemyGameObject.GetComponent<Goblin>();
                if (enemy != null) // Verifica si el componente Goblin est� presente en el enemigo clicado
                {
                    Vector2 enemyPos = enemy.transform.position;
                    float distanceToEnemy = Vector3.Distance(playerCharacter.transform.position, enemyGameObject.transform.position);
                    if (distanceToEnemy <= 1.5f && IsDiagonalOrAdjacent(playerPos, enemyPos)) // Verificar si est� en rango y en posici�n diagonal o adyacente
                    {
                        if (enemyPos.x > playerPos.x) //animaciones de ataque
                        {
                            playerCharacter.AttackRight();
                            yield return new WaitForSeconds(0.4f);
                            playerCharacter.NoAttackRight();
                        }
                        else if (enemyPos.x == playerPos.x)
                        {
                            if (enemyPos.y > playerPos.y)
                            {
                                playerCharacter.AttackUp();
                            }
                            else
                            {
                                playerCharacter.AttackDown();
                            }
                            yield return new WaitForSeconds(0.4f);
                            playerCharacter.NoAttackUp();
                            playerCharacter.NoAttackDown();
                        }
                        else if (enemyPos.x < playerPos.x)
                        {
                            playerCharacter.transform.Rotate(new Vector3(0, 180, 0));
                            playerCharacter.AttackRight();
                            yield return new WaitForSeconds(0.4f);
                            playerCharacter.NoAttackRight();
                            playerCharacter.transform.Rotate(new Vector3(0, 180, 0));
                        }

                        // Ataca al enemigo
                        AttackEnemy(enemy);
                    }
                }
            }
        }
    }

    void AttackEnemy(Goblin enemy)
    {
        if (playerCharacter.GetDexterity() >= playerCharacter.GetIntelligence())
        {
            playerCharacter.attackDex(enemy);
        }
        else
        {
            playerCharacter.attackInt(enemy);
        }
        playerAttack = true; // Establece playerAttack en true para indicar que el jugador ha atacado en este turno
        victoria = true;
        if (enemy.GetHealth() < 1)
        {
            enemies.Remove(enemy);
        }
        Alive();
        if (victoria == true)
        {
            state = BattleState.WON; // si ha matado al enemigo gana
        }
    }

    bool IsDiagonalOrAdjacent(Vector2 pos1, Vector2 pos2)// Verifica si dos posiciones est�n a una distancia de 1 unidad en cualquier direcci�n, incluyendo diagonales.
    {
        Vector2 diff = pos1 - pos2;
        return Mathf.Abs(diff.x) <= 1 && Mathf.Abs(diff.y) <= 1;
    }

    public void Alive()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.GetHealth() > 0)
            {
                victoria = false;
                break;
            }
        }
    }
    public Vector2 GetPlayerPosition() { return playerCharacter.transform.position; }

    public bool GetVictoria()
    {
        return victoria;
    }
    public bool GetCanMove()
    {
        return canMove;
    }
    public bool GetReset()
    {
        return reset;
    }
    public BattleState GetState() {  return state; }
}