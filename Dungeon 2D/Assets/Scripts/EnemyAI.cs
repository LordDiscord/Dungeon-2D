using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    private GridManager gridManager;
    private BattleSystem battleSystem;
    private Character enemyCharacter;

    public void Initialize(GridManager gridManager, BattleSystem battleSystem, Character enemy)
    {
        this.gridManager = gridManager;
        this.battleSystem = battleSystem;
        this.enemyCharacter = enemy;
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
            battleSystem.canMove = false; // hace que el jugador no pueda moverse fuera de su turno
            battleSystem.reset = true; // resetea los movimientos que lleva el jugador a 0 para cuando sea su turno de nuevo
            battleSystem.playerAttack = 0; // resetea el ataque del jugador para su turno
            yield return StartCoroutine(MoveAndAttack());
            yield return null;
        }
    }

    private IEnumerator MoveAndAttack()
    {
        Vector2 playerPos = battleSystem.GetPlayerPosition();

        foreach (Character enemy in battleSystem.enemies)
        {
            Vector2 enemyPos = enemy.transform.position;
            int hasAttacked = 0;
            if (IsPlayerInRange(enemyPos, playerPos))
            {
                if (playerPos.x < enemyPos.x && enemy.check == false)
                {
                    MueveIzquierda(enemy);
                }
                else if (playerPos.x > enemyPos.x && enemy.check == true)
                {
                    MueveDerecha(enemy);
                }
                for(hasAttacked = 0; hasAttacked < enemy.GetAttacks(); hasAttacked++)
                {
                    EnemyAttackAnim(enemy);
                    yield return new WaitForSeconds(0.5f);
                    EnemyNotAttack(enemy);
                    enemy.attackDex(battleSystem.playerCharacter);
                }
            }
            for (int i = 0; i < enemy.GetSpeed(); i++)
            {
                if (hasAttacked < enemy.GetAttacks())
                {
                    if (IsPlayerInRange(enemyPos, playerPos))
                    {
                        if (playerPos.x < enemyPos.x && enemy.check == false)
                        {
                            MueveIzquierda(enemy);
                        }
                        else if (playerPos.x > enemyPos.x && enemy.check == true)
                        {
                            MueveDerecha(enemy);
                        }
                        for (hasAttacked = 0; hasAttacked < enemy.GetAttacks(); hasAttacked++)
                        {
                            EnemyAttackAnim(enemy);
                            yield return new WaitForSeconds(0.5f);
                            EnemyNotAttack(enemy);
                            enemy.attackDex(battleSystem.playerCharacter);
                        }
                        break;
                    }

                    Vector2 nextPos = GetNextPositionTowards(playerPos, enemyPos, enemy);

                    if (gridManager.IsWalkable(nextPos))
                    {
                        EnemyMoves(enemy);
                        yield return StartCoroutine(MoveToPosition(enemy.transform, nextPos, 0.5f));
                        EnemyNotMoves(enemy);
                        enemyPos = nextPos;

                        if (IsPlayerInRange(enemyPos, playerPos))
                        {
                            for (hasAttacked = 0; hasAttacked < enemy.GetAttacks(); hasAttacked++)
                            {
                                EnemyAttackAnim(enemy);
                                yield return new WaitForSeconds(0.5f);
                                EnemyNotAttack(enemy);
                                enemy.attackDex(battleSystem.playerCharacter);
                            }
                            break;
                        }
                    }
                    else
                    {
                        // Si la próxima posición no es transitable, intenta rodear el obstáculo
                        Vector2 newPos = GetAlternatePosition(enemyPos, playerPos);

                        EnemyMoves(enemy);
                        yield return StartCoroutine(MoveToPosition(enemy.transform, newPos, 0.5f));
                        EnemyNotMoves(enemy);
                        enemyPos = newPos;

                        if (IsPlayerInRange(enemyPos, playerPos))
                        {
                            EnemyAttackAnim(enemy);
                            yield return new WaitForSeconds(1f);
                            EnemyNotAttack(enemy);
                            enemy.attackDex(battleSystem.playerCharacter);
                            hasAttacked++;
                            break;
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
            Debug.Log("TU TURNO");
            battleSystem.state = BattleState.PLAYERTURN;
        }

        yield return null;
    }

    private bool IsPlayerInRange(Vector2 enemyPos, Vector2 playerPos)
    {
        return Vector2.Distance(enemyPos, playerPos) <= 1.5f && IsDiagonalOrAdjacent(enemyPos, playerPos);
    }

    private Vector2 GetNextPositionTowards(Vector2 targetPos, Vector2 currentPos, Character enemy)
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

    private Vector2 GetAlternatePosition(Vector2 enemyPos, Vector2 playerPos)
    {
        Vector2 upPos = enemyPos + new Vector2(0, 1);
        Vector2 downPos = enemyPos + new Vector2(0, -1);
        Vector2 leftPos = enemyPos + new Vector2(-1, 0);
        Vector2 rightPos = enemyPos + new Vector2(1, 0);
        Vector2 newPos = enemyPos;

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
        else if (Mathf.Approximately(playerPos.y, enemyPos.y))
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
        }
        return newPos;
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

    bool IsDiagonalOrAdjacent(Vector2 pos1, Vector2 pos2)
    {
        Vector2 diff = pos1 - pos2;
        return Mathf.Abs(diff.x) <= 1 && Mathf.Abs(diff.y) <= 1;
    }

    public void MueveIzquierda(Character enemy)
    {
        enemy.transform.Rotate(new Vector3(0, 180, 0));
        enemy.check = true;
    }

    public void MueveDerecha(Character enemy)
    {
        enemy.transform.Rotate(new Vector3(0, 180, 0));
        enemy.check = false;
    }

    public void EnemyMoves(Character enemy)
    {
        enemy.anim.SetBool("Running", true);
    }

    public void EnemyNotMoves(Character enemy)
    {
        enemy.anim.SetBool("Running", false);
    }

    public void EnemyAttackAnim(Character enemy)
    {
        enemy.anim.SetBool("Attacking", true);
    }

    public void EnemyNotAttack(Character enemy)
    {
        enemy.anim.SetBool("Attacking", false);
    }

    public void EnemyDamageAnim(Character enemy)
    {
        enemy.anim.SetBool("IsDamaged", true);
    }

    public void EnemyNotDamage(Character enemy)
    {
        enemy.anim.SetBool("IsDamaged", false);
    }
}