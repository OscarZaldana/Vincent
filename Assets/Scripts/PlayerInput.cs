using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    GrappleScript gs;
    Player player;

    KeyCode jump;

    void Start()
    {
        gs = GetComponent<GrappleScript>();
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            jump = KeyCode.W;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = KeyCode.Space;
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            jump = KeyCode.Joystick1Button0;
        }

        if (!gs.pivotAttached)
        {
            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            player.SetDirectionalInput(directionalInput);

            if (Input.GetKeyDown(jump))
            {
                player.OnJumpInputDown();
            }
            if (Input.GetKeyUp(jump))
            {
                player.OnJumpInputUp();
            }
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                player.Dashing();
            }

        }
    }
}
