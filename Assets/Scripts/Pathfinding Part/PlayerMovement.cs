using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    
    List<NodeBase> targetNodes;
    int currentNode = 0;

    public static bool movedThisTurn = false;
    public static bool isMoving = false;
    public float moveSpeed = 5f;
    private Vector3 moveOffset = new Vector3(0, 1.5f, 0);

    public void SetTargetNodes(List<NodeBase> nodes)
    {
        targetNodes = nodes;
        targetNodes.Reverse();
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            Move();
        }
    }

    void Move()
    {
        if (!ReachTargetNode())
        {
            MoveTowardsTarget(targetNodes[currentNode].transform.position + moveOffset);
        }
    }

    bool ReachTargetNode()
    {
        if ((targetNodes[currentNode].transform.position + new Vector3(0, 1.5f, 0) - transform.position).magnitude < .1f)
        {
            if (currentNode < targetNodes.Count - 1)
            {
                currentNode++;
            }
            else
            {
                GameManager.instance.tileGenerator.UpdateTile(NodeBase.TileType.Player, targetNodes[currentNode]);
                currentNode = 0;
                StartCoroutine(IdleUntilEnemyTurn());
                isMoving = false;
            }
            return true;
        }
        return false;
    }

    IEnumerator IdleUntilEnemyTurn()
    {
        // Update UI to End Turn
        GameManager.instance.UpdateTurnText("End Turn");
        yield return new WaitForSeconds(1f);
        GameManager.ChangeState(GameManager.GameState.ENEMY);
        GameManager.instance.UpdateTurnText();
        movedThisTurn = false;
    }

    // the closer toward target, the slower it move
    void MoveTowardsTargetSlow(Vector3 target)
    {
        //Get the difference.
        var offset = target - transform.position;

        //If we're further away than .1 unit, move towards the target.
        //The minimum allowable tolerance varies with the speed of the object and the framerate. 
        // 2 * tolerance must be >= moveSpeed / framerate or the object will jump right over the stop.
        if (offset.magnitude > .1f)
        {
            //normalize it and account for movement speed.
            offset = offset.normalized * moveSpeed;
            //actually move the character.
            characterController.Move(offset * Time.deltaTime);
            PlayerLooking(offset.normalized);
        }
    }

    // Constant speed moving
    void MoveTowardsTarget(Vector3 target)
    {
        var dirNormalized = (target - transform.position).normalized;
        transform.position = transform.position + dirNormalized * moveSpeed * Time.deltaTime;
        PlayerLooking(dirNormalized);
    }

    // Look forward when moving
    void PlayerLooking(Vector3 dir)
    {
        if(dir.x >= 0.5)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if(dir.x <= -0.5)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if(dir.z >= 0.5)
        {
            transform.eulerAngles = new Vector3(0, 270, 0);
        }
        else if(dir.z <= -0.5)
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            Debug.LogError("Unexpected facing");
        }
    }
}
