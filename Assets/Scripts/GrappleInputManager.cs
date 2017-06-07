using UnityEngine;
using System.Collections;

public class GrappleInputManager : MonoBehaviour
{

    GrappleScript grapple;
    public Camera cam;

    public float angleStep = 1;
    [Range(0.0f, 360.0f)]
    public float angleTolerance = 90;
    [SerializeField]
    bool ropeCreatedLeftButton = true;
    [SerializeField]
    bool ropeCreatedRightButton = true;
    bool leftOrRight;
    bool canReelIn;

    public float controllerAngle;

    GameObject target;

    void Start()
    {
        grapple = GetComponent<GrappleScript>();
        cam = Camera.main;
    }

    void Update()
    {
        UpdateInput();
        //UpdateInputController();
    }

    private void UpdateInput()
    {
        if (grapple.pivotAttached)
        {
            leftOrRight = false;
            if (canReelIn)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                {

                    grapple.paying_out = false;
                    grapple.reelInSpeed = 5;
                    grapple.reeling_in = true;
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                {
                    grapple.reeling_in = false;
                    grapple.payOutSpeed = 5;
                    grapple.paying_out = true;
                }
                else
                {
                    grapple.reeling_in = false;
                    grapple.paying_out = false;
                }
            }
        }
        else
        {
            leftOrRight = true;
            grapple.reeling_in = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (leftOrRight)
            {
                canReelIn = true;
                // Find mouse position
                Vector3 mouseInput = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20);
                Vector2 mouseClick = cam.ScreenToWorldPoint(mouseInput);

                // Find ray direction and raycast
                Vector2 rayDirection = mouseClick - (Vector2)this.transform.position;
                RaycastHit2D hit = Physics2D.Raycast((Vector2)this.transform.position, rayDirection, grapple.grapplingHookRange, ~(1 << grapple.playerLayer));
                float angle = angleStep;
                Quaternion rot;

                // If the raycast does not hit anything, loop raycast until object is hit
                while (hit.collider == null && angle < angleTolerance)
                {
                    rot = Quaternion.AngleAxis(angle, Vector3.forward);
                    hit = Physics2D.Raycast((Vector2)this.transform.position, rot * rayDirection, grapple.grapplingHookRange, ~(1 << grapple.playerLayer));

                    if (hit.collider != null)
                        break;

                    rot = Quaternion.AngleAxis(-angle, Vector3.forward);
                    hit = Physics2D.Raycast((Vector2)this.transform.position, rot * rayDirection, grapple.grapplingHookRange, ~(1 << grapple.playerLayer));
                    angle += angleStep;

                }
                // if something is hit, and that is not the player
                if (hit.collider != null && hit.collider.gameObject.layer != grapple.playerLayer && hit.collider.gameObject.tag == "Hookable")
                {
                    grapple.AttachRope(hit.point);
                }
            }

            if (!leftOrRight)
            {
                grapple.ReleaseRope();
            }


            /*
            grapple.reeling_in = Input.GetAxis("Mouse ScrollWheel");
            grapple.paying_out = Input.GetKey(KeyCode.X);
            */
        }

        if (Input.GetMouseButtonDown(1))
        {

            if (leftOrRight)
            {
                canReelIn = false;
                grapple.reelInSpeed = 15;
                // Find mouse position
                Vector3 mouseInput = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20);
                Vector2 mouseClick = cam.ScreenToWorldPoint(mouseInput);

                // Find ray direction and raycast
                Vector2 rayDirection = mouseClick - (Vector2)this.transform.position;
                RaycastHit2D hit = Physics2D.Raycast((Vector2)this.transform.position, rayDirection, grapple.grapplingHookRange, ~(1 << grapple.playerLayer));
                float angle = angleStep;
                Quaternion rot;

                // If the raycast does not hit anything, loop raycast until object is hit
                while (hit.collider == null && angle < angleTolerance)
                {
                    rot = Quaternion.AngleAxis(angle, Vector3.forward);
                    hit = Physics2D.Raycast((Vector2)this.transform.position, rot * rayDirection, grapple.grapplingHookRange, ~(1 << grapple.playerLayer));

                    if (hit.collider != null)
                        break;

                    rot = Quaternion.AngleAxis(-angle, Vector3.forward);
                    hit = Physics2D.Raycast((Vector2)this.transform.position, rot * rayDirection, grapple.grapplingHookRange, ~(1 << grapple.playerLayer));
                    angle += angleStep;

                }
                // if something is hit, and that is not the player
                if (hit.collider != null && hit.collider.gameObject.layer != grapple.playerLayer && hit.collider.gameObject.tag == "Hookable")
                {

                    grapple.AttachRope(hit.point);
                    grapple.reeling_in = true;
                }
            }

            if (!leftOrRight)
            {
                grapple.ReleaseRope();
            }
        }
    }
    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    private void UpdateInputController()
    {
        float x = Input.GetAxis("RightStickX");
        float y = Input.GetAxis("RightStickY");
        if (x != 0.0f || y != 0.0f)
        {
            controllerAngle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

            if (grapple.pivotAttached)
            {

                leftOrRight = false;
                if (canReelIn)
                {
                    if (Input.GetAxis("Vertical") > 0f)
                    {

                        grapple.paying_out = false;
                        grapple.reelInSpeed = 5;
                        grapple.reeling_in = true;
                    }
                    else if (Input.GetAxis("Vertical") < 0f)
                    {
                        grapple.reeling_in = false;
                        grapple.payOutSpeed = 5;
                        grapple.paying_out = true;
                    }
                    else
                    {
                        grapple.reeling_in = false;
                        grapple.paying_out = false;
                    }
                }
            }
            else
            {
                leftOrRight = true;
                grapple.reeling_in = false;
            }

            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                if (leftOrRight)
                {
                    canReelIn = true;
                    // Find mouse position
                    Vector3 moveInput = new Vector3(Input.GetAxis("RightStickX"), Input.GetAxis("RightStickY"), 0);
                    Vector2 mouseClick = cam.ScreenToWorldPoint(moveInput);

                    // Find ray direction and raycast
                    Vector2 rayDirection = mouseClick - (Vector2)this.transform.position;
                    RaycastHit2D hit = Physics2D.Raycast((Vector2)this.transform.position, rayDirection, grapple.grapplingHookRange, ~(1 << grapple.playerLayer));
                    float angle = angleStep;
                    Quaternion rot;

                    // If the raycast does not hit anything, loop raycast until object is hit
                    while (hit.collider == null && angle < angleTolerance)
                    {
                        rot = Quaternion.AngleAxis(angle, Vector3.forward);
                        hit = Physics2D.Raycast((Vector2)this.transform.position, rot * rayDirection, grapple.grapplingHookRange, ~(1 << grapple.playerLayer));

                        if (hit.collider != null)
                            break;

                        rot = Quaternion.AngleAxis(-angle, Vector3.forward);
                        hit = Physics2D.Raycast((Vector2)this.transform.position, rot * rayDirection, grapple.grapplingHookRange, ~(1 << grapple.playerLayer));
                        angle += angleStep;

                    }
                    // if something is hit, and that is not the player
                    if (hit.collider != null && hit.collider.gameObject.layer != grapple.playerLayer && hit.collider.gameObject.tag == "Hookable")
                    {
                        grapple.AttachRope(hit.point);
                    }
                }

                if (!leftOrRight)
                {
                    grapple.ReleaseRope();
                }


                /*
                grapple.reeling_in = Input.GetAxis("Mouse ScrollWheel");
                grapple.paying_out = Input.GetKey(KeyCode.X);
                */
            }

            if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            {

                if (leftOrRight)
                {
                    canReelIn = false;
                    grapple.reelInSpeed = 15;
                    // Find mouse position
                    Vector3 mouseInput = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20);
                    Vector2 mouseClick = cam.ScreenToWorldPoint(mouseInput);

                    // Find ray direction and raycast
                    Vector2 rayDirection = mouseClick - (Vector2)this.transform.position;
                    RaycastHit2D hit = Physics2D.Raycast((Vector2)this.transform.position, rayDirection, grapple.grapplingHookRange, ~(1 << grapple.playerLayer));
                    float angle = angleStep;
                    Quaternion rot;

                    // If the raycast does not hit anything, loop raycast until object is hit
                    while (hit.collider == null && angle < angleTolerance)
                    {
                        rot = Quaternion.AngleAxis(angle, Vector3.forward);
                        hit = Physics2D.Raycast((Vector2)this.transform.position, rot * rayDirection, grapple.grapplingHookRange, ~(1 << grapple.playerLayer));

                        if (hit.collider != null)
                            break;

                        rot = Quaternion.AngleAxis(-angle, Vector3.forward);
                        hit = Physics2D.Raycast((Vector2)this.transform.position, rot * rayDirection, grapple.grapplingHookRange, ~(1 << grapple.playerLayer));
                        angle += angleStep;

                    }
                    // if something is hit, and that is not the player
                    if (hit.collider != null && hit.collider.gameObject.layer != grapple.playerLayer && hit.collider.gameObject.tag == "Hookable")
                    {

                        grapple.AttachRope(hit.point);
                        grapple.reeling_in = true;
                    }
                }

                if (!leftOrRight)
                {
                    grapple.ReleaseRope();
                }
            }
        }
    }
}

