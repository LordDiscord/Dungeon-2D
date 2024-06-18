using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private GridManager gridManager;
    private BattleSystem battleSystem;
    private Goblin goblinEnemy;

    public void Initialize(GridManager gridManager, BattleSystem battleSystem, Goblin goblin)
    {
        this.gridManager = gridManager;
        this.battleSystem = battleSystem;
        this.goblinEnemy = goblin;
    }

    public void StartEnemyAI()
    {
        StartCoroutine(EnemyAIController());
    }

    private IEnumerator EnemyAIController()
    {
        while (true)
        {
            yield return new WaitUntil(() => battleSystem.GetState() == BattleState.ENEMYTURN);

            // Perform enemy actions
            battleSystem.nameState = "Enemy";
            battleSystem.canMove = false; //hace que el jugador no pueda moverse fuera de su turno
            battleSystem.reset = true; //resetea los movimientos que lleva el jugador a 0 para cuando sea su turno de nuevo
            battleSystem.playerAttack = false; //resetea el ataque del jugador para su turno
            yield return StartCoroutine(MoveAndAttack());

            // End turn
            battleSystem.state = BattleState.PLAYERTURN;

            yield return null;
        }
    }

    private IEnumerator MoveAndAttack()
    {
        Vector2 playerPos = battleSystem.GetPlayerPosition();

        // Logic for enemy movement towards player
        // Example: Move towards player and attack if in range
        foreach (Goblin enemy in battleSystem.enemies) // Moverá por cada enemigo
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
                enemy.attackDex(battleSystem.playerCharacter);
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
                                enemy.attackDex(battleSystem.playerCharacter);
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
                                enemy.attackDex(battleSystem.playerCharacter);
                                yield return new WaitForSeconds(1f);
                                hasAttacked = true;
                            }
                        }
                    }
                }
            }
            if (battleSystem.playerCharacter.GetHealth() < 1)
            {
                Debug.Log("Perdiste");
                battleSystem.state = BattleState.LOST;
            }
            else
            {
                Debug.Log("TU TURNO"); // esto iria en el ui
            }
        }
        battleSystem.state = BattleState.PLAYERTURN;

        yield return null;
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
        else if (targetPos.x > currentPos.x && enemy.check == true)
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
}