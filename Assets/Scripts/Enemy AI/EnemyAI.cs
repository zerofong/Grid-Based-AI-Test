using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAI : MonoBehaviour, IBot
{
    private void Update()
    {
        if (GameManager.gameState == GameManager.GameState.ENEMY)
        {
            if (newTurn)
            {
                ChangeState(AIState.Idle);
            }

            switch (state)
            {
                case AIState.Idle:
                    Idling();
                    break;
                case AIState.Move:
                    Moving();
                    break;
                case AIState.Action:
                    PerformAction();
                    break;
                default:
                    break;
            }
        }
    }

    #region AI State
    public enum AIState { Idle, Move, Action }
    [HideInInspector]
    public AIState state = AIState.Idle;
    bool newTurn = true;


    void ChangeState(AIState newState)
    {
        state = newState;
        //Debug.Log($"<color=red>{state}</color>");
        switch (newState)
        {
            case AIState.Idle:
                GameManager.instance.UpdateTurnText("Thinking");
                newTurn = false;
                ChooseMoveTarget();
                break;
            case AIState.Move:
                GameManager.instance.UpdateTurnText("Moving");
                break;
            case AIState.Action:
                break;
            default:
                break;
        }
    }
    #endregion

    #region Idle
    [Header("Idle")]
    public float idleTime = 1f;
    float startIdleTime = 0f;

    public void Idling()
    {
        if (startIdleTime < idleTime)
        {
            startIdleTime += Time.deltaTime;
        }
        else
        {
            ChangeState(AIState.Move);
            startIdleTime = 0f;
        }
    }
    #endregion

    #region Move
    [Header("Move")]
    public float moveSpeed = 5f;
    private Vector3 moveOffset = new Vector3(0, 1.5f, 0);

    public void Moving()
    {
        if (!ReachTargetNode())
        {
            var target = targetNodes[currentNode].transform.position + moveOffset;
            var dirNormalized = (target - transform.position).normalized;
            transform.position = transform.position + dirNormalized * moveSpeed * Time.deltaTime;

            EnemyFacing(dirNormalized);
        }
    }

    void EnemyFacing(Vector3 dir)
    {
        if (dir.x >= 0.5)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (dir.x <= -0.5)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (dir.z >= 0.5)
        {
            transform.eulerAngles = new Vector3(0, 270, 0);
        }
        else if (dir.z <= -0.5)
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            Debug.LogError("Unexpected facing");
        }
    }

    #region Pathfinding
    List<NodeBase> targetNodes;
    int currentNode = 0;

    public void ChooseMoveTarget()
    {
        var neighbors = GameManager.instance.tileGenerator.GetPlayerTile().Neighbors;

        List<NodeBase> targets = new List<NodeBase>();
        foreach (var item in neighbors.Where(t => t.Walkable))
        {
            targets.Add(item);
        }

        int rand = Random.Range(0, targets.Count);
        var targetTile = targets[rand];

        SetTargetNodes(Pathfinder.FindPath(GameManager.instance.tileGenerator.GetEnemyTile(), targetTile));
    }

    public void SetTargetNodes(List<NodeBase> nodes)
    {
        if (nodes == null)
        {
            Debug.LogWarning("No path found");
            return;
        }

        targetNodes = nodes;
        targetNodes.Reverse();
    }

    bool ReachTargetNode()
    {
        if(targetNodes.Count == 0 || targetNodes == null)
        {
            // Facing player without moving
            var target = targetNodes[currentNode].transform.position + moveOffset;
            var dirNormalized = (target - transform.position).normalized;
            EnemyFacing(dirNormalized);

            ChangeState(AIState.Action);
        }
        if ((targetNodes[currentNode].transform.position + new Vector3(0, 1.5f, 0) - transform.position).magnitude < .1f)
        {
            if (currentNode < targetNodes.Count - 1)
            {
                currentNode++;
            }
            else
            {
                GameManager.instance.tileGenerator.UpdateTile(NodeBase.TileType.Enemy, targetNodes[currentNode]);
                currentNode = 0;

                var dirNormalized = (GameManager.instance.tileGenerator.GetPlayerTile().transform.position - transform.position).normalized;
                EnemyFacing(dirNormalized);

                ChangeState(AIState.Action);
            }
            return true;
        }
        return false;
    }
    #endregion
    #endregion

    #region Action
    [Header("Action")]
    public float actionTime = 1f;
    private float currentActionTime = 0f;
    private bool hasAction = false;
    public string[] actionStrings;

    public void PerformAction()
    {
        if (!hasAction)
        {
            var rand = Random.Range(0, actionStrings.Length);
            GameManager.instance.UpdateTurnText(actionStrings[rand]);
            hasAction = true;
        }

        if (currentActionTime < actionTime)
        {
            currentActionTime += Time.deltaTime;
        }
        else
        {
            GameManager.ChangeState(GameManager.GameState.PLAYER);
            GameManager.instance.UpdateTurnText();
            currentActionTime = 0f;
            hasAction = false;
            newTurn = true;
        }
    }
    #endregion
}
