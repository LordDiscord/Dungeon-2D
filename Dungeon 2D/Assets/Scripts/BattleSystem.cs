using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST } //Maquina de Estados

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject goblinPrefab;
    public GameObject skeletonPrefab;
    public GameObject enemyAiPrefab;

    public MainCharacter playerCharacter; // Declarar como variable de nivel de clase
    public EnemyAI enemyAI;
    public List<Character> enemies = new List<Character>();

    private bool endScene = false;
    public int maxNum;
    public GameObject portal;
    private GridManager gridManager;
    public string nameState;
    public Transform[] enemySpawns;
    public Transform playerSpawn;
    public bool canMove = false;
    public bool reset = false;
    public bool victoria = false;
    public int playerAttack = 0;
    public BattleState state;
    private PlayerGridController playerGridController;

    void Awake()
    {
        gridManager = GetComponent<GridManager>();
        if (MainCharacter.instance == null)
        {
            GameObject playerObject = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
            playerCharacter = playerObject.GetComponent<MainCharacter>();
            DontDestroyOnLoad(playerObject);
            MainCharacter.instance = playerCharacter; // Asignar la instancia
            playerGridController = playerObject.GetComponent<PlayerGridController>();
            playerGridController.SetMovePointToSpawn(playerSpawn.position);
        }
        else
        {
            playerCharacter = MainCharacter.instance;
            playerCharacter.RespawnAt(playerSpawn.position);
            playerGridController = playerCharacter.GetComponent<PlayerGridController>();
            playerGridController.SetMovePointToSpawn(playerSpawn.position);
        }
    }

    void Start()
    {
        int minNum = 1 + GameManager.instance.level / 3;
        maxNum = 1 + GameManager.instance.level;
        int numEnemies = Random.Range(minNum, maxNum);
        Debug.Log("minNum: " + minNum);
        Debug.Log("maxNum: " + maxNum);

        List<int> spawnIndices = new List<int>(); // Lista para guardar los índices de spawn ya utilizados
        for (int j = 0; j < numEnemies; j++)
        {
            // Genera un índice aleatorio que no haya sido utilizado aún
            int randomIndex = Random.Range(0, enemySpawns.Length);
            while (spawnIndices.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, enemySpawns.Length);
            }
            spawnIndices.Add(randomIndex); // Agrega el índice a la lista de utilizados

            // Elige aleatoriamente entre goblin y skeleton
            int randomValue = Random.Range(0, 100);
            int increasePercentadge = 110 - ((maxNum - 1) * 10); //por cada nivel la probabilidad bajara un 10%
            GameObject enemyPrefab = (randomValue < increasePercentadge) ? goblinPrefab : skeletonPrefab; // 66% para goblin, 34% para skeleton

            // Instancia el enemigo en la posición aleatoria y guarda la referencia en la lista de enemigos
            GameObject enemyObject = Instantiate(enemyPrefab, enemySpawns[randomIndex].position, Quaternion.identity);
            Character enemyCharacter = enemyObject.GetComponent<Character>();
            enemies.Add(enemyCharacter);
        }
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        enemyAI = new GameObject("EnemyAI").AddComponent<EnemyAI>();
        enemyAI.Initialize(gridManager, this, enemies[0]);
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
            EndBattle();
        }
        if (state == BattleState.LOST)
        {
            StartCoroutine(LostBattle());
        }
    }

    IEnumerator SetupBattle()
    {
        nameState = "Setup Battle";
        yield return new WaitForSeconds(2f);
        Debug.Log("Iniciativa Jugador: " + playerCharacter.GetInitiative()); // esto iria en el ui
        bool playerFirst = true;
        foreach (Character enemy in enemies)
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
        yield return new WaitForSeconds(1.5f);
        if (endScene == false)
        {
            GameManager.instance.LoadScene("GameOver");
            endScene = true;    
        }
    }

    void EndBattle()
    {
        nameState = "Battle Won";
        if (portal != null)
        {
            portal.SetActive(true); // Activar el portal
        }
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
        if (Input.GetMouseButtonDown(0) && playerAttack != playerCharacter.GetAttacks()) // Verifica si el jugador hizo clic y no ha atacado aún en este turno
        {
            Vector2 playerPos = playerCharacter.transform.position;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Convierte la posición del clic del ratón a coordenadas del mundo
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero); // Realiza un rayo desde la posición del ratón
            if (hit.collider != null && hit.collider.CompareTag("Enemy")) // Si el rayo choca con un colisionador de enemigo
            {
                GameObject enemyGameObject = hit.collider.gameObject;
                Character enemy = enemyGameObject.GetComponent<Character>();
                if (enemy != null) // Verifica si el componente Character está presente en el enemigo clicado
                {
                    Vector2 enemyPos = enemy.transform.position;
                    float distanceToEnemy = Vector3.Distance(playerCharacter.transform.position, enemyGameObject.transform.position);
                    if (distanceToEnemy <= 1.5f && IsDiagonalOrAdjacent(playerPos, enemyPos)) // Verificar si está en rango y en posición diagonal o adyacente
                    {
                        AttackEnemy(enemy);
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
                    }
                }
            }
        }
    }

    void AttackEnemy(Character enemy)
    {
        if (playerCharacter.GetDexterity() >= playerCharacter.GetIntelligence())
        {
            playerCharacter.attackDex(enemy);
        }
        else
        {
            playerCharacter.attackInt(enemy);
        }
        playerAttack++; // Realiza un Ataque y se suma uno al contador
        victoria = true;
        if (enemy.GetHealth() < 1)
        {
            enemies.Remove(enemy);
        }
        Alive();
        if (victoria == true)
        {
            canMove = true;
            state = BattleState.WON; // si ha matado al enemigo gana
        }
    }

    bool IsDiagonalOrAdjacent(Vector2 pos1, Vector2 pos2)// Verifica si dos posiciones están a una distancia de 1 unidad en cualquier dirección, incluyendo diagonales.
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