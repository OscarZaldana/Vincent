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
		
	}

    void AnimateMoveMent()
    {
        if(!pc.collisions.left && !pc.collisions.right && !pc.collisions.below && !gs.pivotAttached && !player.wallSliding)
        {
            if (player.velocity.y > 0)
            {
                anim.Play("JumpingPlayer");
            }
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
