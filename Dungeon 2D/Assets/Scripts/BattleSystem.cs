using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    MainCharacter playerCharacter; // Declarar como variable de nivel de clase
    List<Goblin> enemies = new List<Goblin>();

    private GridManager gridManager;

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

        HandlePlayerRespawn();

        int numEnemies = Random.Range(2, 4); // cuantos enemigos quieres?

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
        Debug.Log("EL COMBATE EMPIEZA, MUCHA SUERTE"); // esto iria en el ui
        yield return new WaitForSeconds(2f);
        Debug.Log("Vida Jugador: " +playerCharacter.GetVida()); // esto iria en el ui
        foreach (Goblin enemy in enemies)
        {
            Debug.Log("Vida Enemigo: " + enemy.GetVida()); // esto iria en el ui
        }
        yield return new WaitForSeconds(2f);
        Debug.Log("Iniciativa Jugador: "+playerCharacter.GetIniciativa()); // esto iria en el ui
        foreach (Goblin enemy in enemies)
        {
            Debug.Log("Iniciativa Enemigo: " + enemy.GetIniciativa()); // esto iría en el ui
        }
        bool playerFirst = true;
        foreach (Goblin enemy in enemies)
        {
            if (enemy.GetIniciativa() > playerCharacter.GetIniciativa())
            {
                playerFirst = false;
                break;
            }
        }
        if (playerFirst)
        {
            Debug.Log("TURNO JUGADOR"); // esto iria en el ui
            Debug.Log("RECUERDA QUE SOLO TE PUEDES MOVER 6 CASILLAS"); // esto iria en el ui
            Debug.Log("TAMBIEN RECUERDA QUE PARA ATACAR HAS DE CLICAR ENCIMA DEL GOBLIN"); // esto iria en el ui
            Debug.Log("Y POR ÚLTIMO RECUERDA QUE PARA PASAR TURNO HAS DE PRESIONAR LA 'T'"); // esto iria en el ui
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        else
        {
            Debug.Log("TURNO ENEMIGO"); // esto iria en el ui
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }


    }

    IEnumerator LostBattle()
    {
        canMove = false;
        yield return new WaitForSeconds(2f);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }

    IEnumerator EndBattle()
    {
        canMove = true;
        yield return new WaitForSeconds(2f);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }

    void PlayerTurn()
    {
        reset = false;
        canMove = true;
        StartCoroutine(PlayerAttackCheck()); //Se ocupa de comprobar que al atacar con el click del raton este a rango y lo ejecuta si asi es
        if (Input.GetKeyDown(KeyCode.T) && !victoria) //Pulsando la tecla T pasas el turno al enemigo
        {
            Debug.Log("TURNO ENEMIGO"); // esto iria en el ui
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    void EnemyTurn()
    {
        canMove = false; //hace que el jugador no pueda moverse fuera de su turno
        reset = true; //resetea los movimientos que lleva el jugador a 0 para cuando sea su turno de nuevo
        playerAttack = false; //resetea el ataque del jugador para su turno
        StartCoroutine(EnemyMoveAndAttack());
    }

    IEnumerator PlayerAttackCheck()
    {
        if (Input.GetMouseButtonDown(0) && !playerAttack) // Verifica si el jugador hizo clic y no ha atacado aún en este turno
        {
            Vector2 playerPos = playerCharacter.transform.position;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Convierte la posición del clic del ratón a coordenadas del mundo
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero); // Realiza un rayo desde la posición del ratón
            if (hit.collider != null && hit.collider.CompareTag("Enemy")) // Si el rayo choca con un colisionador de enemigo
            {
                GameObject enemyGameObject = hit.collider.gameObject;
                Goblin enemy = enemyGameObject.GetComponent<Goblin>();
                if (enemy != null) // Verifica si el componente Goblin está presente en el enemigo clicado
                {
                    Vector2 enemyPos = enemy.transform.position;
                    float distanceToEnemy = Vector3.Distance(playerCharacter.transform.position, enemyGameObject.transform.position);
                    if (distanceToEnemy <= 1.5f && IsDiagonalOrAdjacent(playerPos, enemyPos)) // Verificar si está en rango y en posición diagonal o adyacente
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
        if (playerCharacter.GetDestreza() >= playerCharacter.GetInteligencia())
        {
            playerCharacter.atacarDestreza(enemy);
        }
        else
        {
            playerCharacter.atacarInteligencia(enemy);
        }
        playerAttack = true; // Establece playerAttack en true para indicar que el jugador ha atacado en este turno
        victoria = true;
        if(enemy.GetVida() < 1)
        {
            enemies.Remove(enemy);
        }
        EstanVivos();
        if (victoria == true)
        {
            state = BattleState.WON; // si ha matado al enemigo gana
            Debug.Log("FELICIDADES POR COMPLETAR ESTE NIVEL!");
        }
    }

    IEnumerator EnemyMoveAndAttack() //Mueve al enemigo hacia el jugador y lo ataca si está cerca.
    {
        Vector2 playerPos = playerCharacter.transform.position;
        foreach (Goblin enemy in enemies) // Moverá por cada enemigo
        {
            Vector2 enemyPos = enemy.transform.position;
            bool hasAttacked = false;

            float distanceToPlayerInitial = Vector2.Distance(enemyPos, playerPos); //Comprovamos si el jugador esta a rango antes de que el enemigo mueva
            if (distanceToPlayerInitial <= 1.5f && IsDiagonalOrAdjacent(enemyPos, playerPos))
            {
                if (playerPos.x < enemyPos.x && enemy.check == false)
                {
                    MueveIzquierda(enemy);
                }
                else if (playerPos.x > enemyPos.x && enemy.check == true)
                {
                    MueveDerecha(enemy);
                }
                EnemyAttackAnim(enemy);
                yield return new WaitForSeconds(1f);
                EnemyNotAttack(enemy);
                enemy.atacarDestreza(playerCharacter);
                hasAttacked = true;
            }
            if (!hasAttacked)// Si el enemigo no ha atacado, realizar movimientos y chequeo de ataque
            {
                for (int i = 0; i < 6; i++) //lo comprueba 6 veces ya que se va a mover 6 veces
                {
                    if (!hasAttacked) // Si el enemigo no ha atacado, realizar movimientos y chequeo de ataque
                    {
                        Vector2 nextPos = GetNextPositionTowards(playerPos, enemyPos, enemy);

                        if (gridManager.IsWalkable(nextPos))
                        {
                            EnemyMoves(enemy);
                            yield return StartCoroutine(MoveToPosition(enemy.transform, nextPos, 0.5f)); // Mueve al enemigo gradualmente
                            EnemyNotMoves(enemy);
                            enemyPos = nextPos;

                            float distanceToPlayerNew = Vector2.Distance(enemyPos, playerPos);
                            if (distanceToPlayerNew <= 1.5f && IsDiagonalOrAdjacent(enemyPos, playerPos)) // Verificar si está en rango y en posición diagonal o adyacente
                            {
                                EnemyAttackAnim(enemy);
                                yield return new WaitForSeconds(0.5f);
                                EnemyNotAttack(enemy);
                                enemy.atacarDestreza(playerCharacter);
                                hasAttacked = true;
                            }
                        }
                        else
                        {
                            // Si la próxima posición no es transitable, intenta rodear el obstáculo
                            Vector2 upPos = enemyPos + new Vector2(0, 1);
                            Vector2 downPos = enemyPos + new Vector2(0, -1);
                            Vector2 leftPos = enemyPos + new Vector2(-1, 0);
                            Vector2 rightPos = enemyPos + new Vector2(1, 0);
                            Vector2 newPos = enemyPos;

                            // Comprobar si el jugador y el enemigo están en el mismo eje X
                            if (Mathf.Approximately(playerPos.x, enemyPos.x))
                            {
                                if (playerPos.y > enemyPos.y && gridManager.IsWalkable(rightPos))
                                {
                                    newPos = rightPos;
                                }
                                else if (gridManager.IsWalkable(leftPos))
                                {
                                    newPos = leftPos;
                                }
                            }
                            else if (Mathf.Approximately(playerPos.y, enemyPos.y)) // Comprobar si el jugador y el enemigo están en el mismo eje Y
                            {
                                if (playerPos.x > enemyPos.x && gridManager.IsWalkable(upPos))
                                {
                                    newPos = upPos;
                                }
                                else if (gridManager.IsWalkable(downPos))
                                {
                                    newPos = downPos;
                                }
                            }
                            else
                            {
                                // Intenta moverse hacia la izquierda, derecha, arriba o abajo para rodear el obstáculo
                                float distanceToLeft = Vector2.Distance(leftPos, playerPos);
                                float distanceToRight = Vector2.Distance(rightPos, playerPos);
                                float distanceToUp = Vector2.Distance(upPos, playerPos);
                                float distanceToDown = Vector2.Distance(downPos, playerPos);

                                if (distanceToLeft < distanceToRight && gridManager.IsWalkable(leftPos))
                                {
                                    newPos = leftPos;
                                }
                                else if (distanceToLeft > distanceToRight && gridManager.IsWalkable(rightPos))
                                {
                                    newPos = rightPos;
                                }
                                else if (distanceToUp < distanceToDown && gridManager.IsWalkable(upPos))
                                {
                                    newPos = upPos;
                                }
                                else if (distanceToUp > distanceToDown && gridManager.IsWalkable(downPos))
                                {
                                    newPos = downPos;
                                }
                                else
                                {
                                    newPos = enemyPos; // Si no se puede mover hacia arriba o abajo, permanece en la misma posición
                                }

                            }
                            EnemyMoves(enemy);
                            yield return StartCoroutine(MoveToPosition(enemy.transform, newPos, 0.5f)); // Mueve al enemigo gradualmente
                            EnemyNotMoves(enemy);
                            enemyPos = newPos; // Actualiza la posición del enemigo

                            float distanceToPlayerAfterMove = Vector2.Distance(enemyPos, playerPos);
                            if (distanceToPlayerAfterMove <= 1.5f && IsDiagonalOrAdjacent(enemyPos, playerPos))
                            {
                                EnemyAttackAnim(enemy);
                                EnemyNotAttack(enemy);
                                enemy.atacarDestreza(playerCharacter);
                                yield return new WaitForSeconds(1f);
                                hasAttacked = true;
                            }
                        }
                    }
                }
            }
            if (playerCharacter.GetVida() < 1)
            {
                Debug.Log("Perdiste");
                state = BattleState.LOST;
            }
            else
            {
                Debug.Log("TU TURNO"); // esto iria en el ui
            }
        }
        state = BattleState.PLAYERTURN;
    }

    Vector2 GetNextPositionTowards(Vector2 targetPos, Vector2 currentPos, Goblin enemy)
    {
        Vector2 direction = (targetPos - currentPos).normalized;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction.y = 0;   
        }
        else
        {
            direction.x = 0;   
        }
        Vector2 nextPos = currentPos + new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        if (targetPos.x < currentPos.x && enemy.check == false)
        {
            MueveIzquierda(enemy);
        }
        else if(targetPos.x > currentPos.x && enemy.check == true)
        {
            MueveDerecha(enemy);
        }
        return nextPos;
    }

    IEnumerator MoveToPosition(Transform transform, Vector2 target, float duration)
    {
        Vector2 start = transform.position;
        float elapsed = 0;

        while (elapsed < duration)
        {
            transform.position = Vector2.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
    }

    bool IsDiagonalOrAdjacent(Vector2 pos1, Vector2 pos2)// Verifica si dos posiciones están a una distancia de 1 unidad en cualquier dirección, incluyendo diagonales.
    {
        Vector2 diff = pos1 - pos2;
        return Mathf.Abs(diff.x) <= 1 && Mathf.Abs(diff.y) <= 1;
    }

    public void EstanVivos()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.GetVida() > 0)
            {
                victoria = false;
                break;
            }
        }
    }

    private void HandlePlayerRespawn()
    {
        MainCharacter existingPlayer = MainCharacter.Instance;

        if (existingPlayer != null)
        {
            // Guardar estadísticas antes de destruir el jugador existente solo si la vida es mayor a 0
            if (existingPlayer.GetVida() > 0)
            {
                existingPlayer.SaveStats();
            }
            Destroy(existingPlayer.gameObject);
        }
        // Crear nuevo jugador en el punto de spawn
        GameObject playerObject = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);

        // Cargar estadísticas en el nuevo jugador o generar nuevas si no hay estadísticas guardadas
        playerCharacter = playerObject.GetComponent<MainCharacter>();
        if (playerCharacter != null)
        {
            playerCharacter.LoadStats();
            if (playerCharacter.GetVida() <= 0)
            {
                playerCharacter.GenerateNewStats();
            }
        }
    }

    public void MueveIzquierda(Goblin enemy) // se encarga de flipear el sprite
    {
        enemy.transform.Rotate(new Vector3(0, 180, 0));
        enemy.check = true;
    }
    public void MueveDerecha(Goblin enemy)
    {
        enemy.transform.Rotate(new Vector3(0, 180, 0));
        enemy.check = false;
    }
    public void EnemyMoves(Goblin enemy)
    {
        enemy.anim.SetBool("Running", true);
    }
    public void EnemyNotMoves(Goblin enemy)
    {
        enemy.anim.SetBool("Running", false);
    }
    public void EnemyAttackAnim(Goblin enemy)
    {
        enemy.anim.SetBool("Attacking", true);
    }
    public void EnemyNotAttack(Goblin enemy)
    {
        enemy.anim.SetBool("Attacking", false);
    }
    public void EnemyDamageAnim(Goblin enemy)
    {
        enemy.anim.SetBool("IsDamaged", true);
    }
    public void EnemyNotDamage(Goblin enemy)
    {
        enemy.anim.SetBool("IsDamaged", false);
    }

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
}
