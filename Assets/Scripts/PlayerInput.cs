using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    GrappleScript gs;
    Player player;

    void Start()
    {
        gs = GetComponent<GrappleScript>();
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (!gs.pivotAttached)
        {
            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            player.SetDirectionalInput(directionalInput);

            if (Input.GetKeyDown(KeyCode.W))
            {
                player.OnJumpInputDown();
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                player.OnJumpInputUp();
            }
        }
    }
}
