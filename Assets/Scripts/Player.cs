﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {


    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 10;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    [SerializeField]
    float dashVelocity = 5;
    [SerializeField]
    float extraJumps;
    public float extraDash = 1;
    [SerializeField]
    float runSpeed;
    public float jumpsMade;
    public float dashesMade;
    public Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;
    GrappleScript gs;

    public Vector2 directionalInput;
    public bool wallSliding;
    int wallDirX;
    public bool dashing = false;
    public bool jumping = false;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();
        gs = GetComponent<GrappleScript>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void Update()
    {
        if (dashing)
        {
            StartCoroutine("TurnDashFalse");
        }

        if (!gs.pivotAttached)
        {
                CalculateVelocity();
                HandleWallSliding();

                WalkOrRun();

                if (controller.collisions.above || controller.collisions.below)
                {

                    if (controller.collisions.slidingDownMaxSlope)
                    {
                        velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
                    }
                    else
                    {
                        velocity.y = 0;
                    }
                }

                if (controller.collisions.below)
                {
                  dashesMade = 0;
                   jumpsMade = 0;
                }
                if (controller.collisions.left || controller.collisions.right)
                {
                    jumpsMade = 0;
                    dashesMade = 0;
                }
        }
        else
        {
            velocity.y = 0;
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void WalkOrRun()
    {
        float running = Input.GetAxisRaw("Triggers");

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            controller.Move(velocity * Time.deltaTime * runSpeed, directionalInput);
        }

        else if (running < 0 || running > 0)
        {
            controller.Move(velocity * Time.deltaTime * runSpeed, directionalInput);
        }
        else
        {
            controller.Move(velocity * Time.deltaTime, directionalInput);
        }
    }

    public void OnJumpInputDown()
    {
        jumpsMade++;
        if (wallSliding)
        {
            jumpsMade = 0;
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        if (controller.collisions.below)
        {
            jumpsMade = 0;
            if (controller.collisions.slidingDownMaxSlope)
            {
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                { // not jumping against max slope
                    velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                }
            }
            else
            {
                    velocity.y = maxJumpVelocity;
            }
        }

        if (!controller.collisions.below && jumpsMade <= extraJumps)
        {
            velocity.y = maxJumpVelocity;
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }


    void HandleWallSliding()
    {
            wallDirX = (controller.collisions.left) ? -1 : 1;
            wallSliding = false;
            if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
            {
                wallSliding = true;

                if (velocity.y < -wallSlideSpeedMax)
                {
                    velocity.y = -wallSlideSpeedMax;
                }

                if (timeToWallUnstick > 0)
                {
                    velocityXSmoothing = 0;
                    velocity.x = 0;

                    if (directionalInput.x != wallDirX && directionalInput.x != 0)
                    {
                        timeToWallUnstick -= Time.deltaTime;
                    }
                    else
                    {
                        timeToWallUnstick = wallStickTime;
                    }
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }

            }
    }

    public void Dashing()
    {
        if (!wallSliding)
        {
            if (dashesMade <= extraDash)
            {
                dashesMade++;
                velocity.y = 0;
                velocity.x *= dashVelocity;
                dashing = true;
            }
        } 
    }

    void CalculateVelocity()
    {
            float targetVelocityX = directionalInput.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            velocity.y += gravity * Time.deltaTime;
    }

    IEnumerator TurnDashFalse()
    {
        yield return new WaitForSeconds(0.5f);

        dashing = false;
    }
}
