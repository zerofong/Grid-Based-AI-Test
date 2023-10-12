using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputHandler : MonoBehaviour
{
    private Camera cam;
    public LayerMask raycastableLayer;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MouseRaycast();
    }

    private void MouseRaycast()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f, raycastableLayer))
        {
            NodeBase tile = hit.transform.GetComponent<NodeBase>();
            if (!tile) return;

            GetTileInformation(tile);
            MoveHere(tile);
        }
        else
        {
            ResetTileInformation();
        }
    }

    #region UI Information
    [Header("UI Information")]
    public GameObject infoGo;
    public TextMeshProUGUI infoText;

    void GetTileInformation(NodeBase tile)
    {
        infoText.text = tile.TileDetails();

        infoGo.SetActive(true);
    }

    void ResetTileInformation()
    {
        infoText.text = "";
        infoGo.SetActive(false);
    }
    #endregion

    #region Pathfinding
    void MoveHere(NodeBase targetTile)
    {
        if (GameManager.gameState == GameManager.GameState.PLAYER)
        {
            if (Input.GetMouseButtonUp(0) && !PlayerMovement.isMoving && !PlayerMovement.movedThisTurn)
            {
                if (targetTile.CanMoveHere())
                {
                    if (!playerMovement)
                    {
                        playerMovement = FindObjectOfType<PlayerMovement>();
                    }

                    var waypoints = Pathfinder.FindPath(GameManager.instance.tileGenerator.GetPlayerTile(), targetTile);
                    if (waypoints != null)
                    {
                        PlayerMovement.movedThisTurn = true;
                        GameManager.instance.UpdateTurnText("Moving");
                        playerMovement.SetTargetNodes(waypoints);
                    }
                    else
                    {
                        GameManager.instance.UpdateTurnTextCustom("Game Over.\nYou're blocked!");
                    }
                }
            }
        }
    }
    #endregion
}
