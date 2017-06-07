using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{

    Player player;
    GrappleScript gs;
    Controller2D pc;
    PlayerInput pi;

    SpriteRenderer rend;
    Animator anim;

    // Use this for initialization
    void Start ()
    {
        player = GetComponentInParent<Player>();
        gs = GetComponentInParent<GrappleScript>();
        pc = GetComponentInParent<Controller2D>();
        pi = GetComponentInParent<PlayerInput>();

        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        XDirection();
        AnimateMoveMent();
    }

    void AnimateMoveMent()
    {
        if (player.velocity.x < 0 || player.velocity.x > 0)
            anim.Play("WalkingPlayer");

        /*if (!pc.collisions.left && !pc.collisions.right && !pc.collisions.below && !gs.pivotAttached && !player.wallSliding && player.dashing == false)
        {
            if (player.jumping == true)
            {
                anim.Play("JumpingPlayer");
            }
        }
        else if (!pc.collisions.left && !pc.collisions.right && !pc.collisions.below && !gs.pivotAttached && !player.wallSliding && player.dashing == false)
        {
            if (player.jumping == false)
            {
                anim.Play("Falling");
            }
        }

        else if (!pc.collisions.left && !pc.collisions.right && pc.collisions.below && !gs.pivotAttached && !player.wallSliding && player.dashing == false)
        {
            if (player.velocity.x < 0 || player.velocity.x > 0)
                anim.Play("WalkingPlayer");
        }
        else if (player.velocity.x < 0 || player.velocity.x > 0)
        {
            anim.Play("RunningPlayer");
        }
        else if (player.dashing == true)
        {
            anim.Play("DashingPlayer");
        }
        else if (!pc.collisions.left && !pc.collisions.right && !pc.collisions.below && gs.pivotAttached && !player.wallSliding && player.dashing == false)
        {
            anim.Play("Swinging");
        }
        else if (!pc.collisions.left && !pc.collisions.right && !pc.collisions.below && !gs.pivotAttached && player.wallSliding && player.dashing == true)
        {
            anim.Play("SlidingPlayer");
        }
        */
        else
            anim.Play("PlayerIdle");


    }

    void XDirection()
    {
        if(player.directionalInput.x < 0)
        {
            rend.flipX = true;
        }
        else if(player.directionalInput.x > 0)
        {
            rend.flipX = false;
        }
    }

}


/*if (grounded)
        {
            jumped = false;
              if(currentDir == moveDir.left || currentDir == moveDir.right)
            {
                anim.Play("Walk");
            }
            else if (rocking.radioOn)
            {
                anim.Play("Rocking");
            }
            else
            {
                anim.Play("Idle");
            }
        }*/
