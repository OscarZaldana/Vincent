using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(LineRenderer))]
//[RequireComponent (typeof(Rigidbody2D))]

public class GrappleScript : MonoBehaviour {
	
	public bool pivotAttached = false;					
	
	private float ropeLength;							
    Player player;

	public Vector2 pivotPoint;							
	private List<Vector3> pivotList;					
	private List<bool> currentSwingDirection;			


	public float reelInSpeed = 1;						
	public float payOutSpeed = 1;						
	public bool reeling_in = false;						
	public bool paying_out = false;                     
    bool addRB;

    private LineRenderer rope;								
	private Rigidbody2D rigid_body;						

	
	private Vector2 vectorToPivot;						
	private float distanceToPivot;						
	private Vector2 directionToPivot;					

	public float ropeBendTolerance = 0.01f;				
	public float grapplingHookRange = 100f;			
	public int playerLayer = 8;							
	public bool autoSetLayer = true;
	public Vector2 ropeBasePoint = new Vector2(0,0);
	public bool allowRotation = true;				
	public bool ropeCollisions = true;					
	public float strength = 1;
	void Start()
	{
        player = GetComponent<Player>();

		rope = GetComponent<LineRenderer>();
		rope.SetPosition(0,transform.position);
		rope.SetPosition(1,transform.position);

		pivotList = new List<Vector3>();
		currentSwingDirection = new List<bool>();

		if(autoSetLayer)
			playerLayer = this.gameObject.layer;
	}

	void FixedUpdate ()
    {


		
		if(pivotAttached)
		{
            if (addRB)
            {
                gameObject.AddComponent<Rigidbody2D>();
                rigid_body = this.GetComponent<Rigidbody2D>();
                rigid_body.constraints = RigidbodyConstraints2D.FreezeRotation;
                rigid_body.gravityScale = 5;
                addRB = false;
            }
            if (pivotList.Count==0)
			{
				pivotList.Add(pivotPoint);
				SetRopeLength();
			}

			if(ropeCollisions)
			{
				vectorToPivot = (Vector2)(pivotList[pivotList.Count-1] - this.transform.position);	
				distanceToPivot = vectorToPivot.magnitude;
				directionToPivot = vectorToPivot.normalized;
				RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, directionToPivot,distanceToPivot , ~(1<<playerLayer));			

				if(hit.collider!=null)
				{
					if(	Vector2.Distance(hit.point,pivotList[pivotList.Count-1]) > ropeBendTolerance ) 

					{
						AddRopeBend(hit);
						pivotList.Add(pivotPoint);
						SetRopeLength();
						currentSwingDirection.Add(isBendClockwise());

					}
				}
				
				if(currentSwingDirection.Count>0)
				{
					if(currentSwingDirection[currentSwingDirection.Count-1]!= isBendClockwise())
					{
						pivotList.RemoveAt(pivotList.Count-1);
						currentSwingDirection.RemoveAt(currentSwingDirection.Count-1);
						SetRopeLength();
					}
				}
			}
			vectorToPivot = (Vector2)(pivotList[pivotList.Count-1] - this.transform.position);	
			distanceToPivot = vectorToPivot.magnitude;
			directionToPivot = vectorToPivot.normalized;
			float speedTowardsPivot = Vector2.Dot(rigid_body.velocity, directionToPivot);

			NeutraliseGravity();

			if(speedTowardsPivot<=0 && distanceToPivot >= ropeLength)
			{
				rigid_body.AddForce(directionToPivot* Vector2.Dot(Physics2D.gravity , directionToPivot)*-1);
				rigid_body.velocity = GetComponent<Rigidbody2D>().velocity - (Vector2)(speedTowardsPivot*directionToPivot);
			}
			if(reeling_in)
			{
				if(speedTowardsPivot <= reelInSpeed)
                {
					rigid_body.velocity = GetComponent<Rigidbody2D>().velocity + (Vector2)(reelInSpeed*directionToPivot);					
				}
				ropeLength = ropeLength - reelInSpeed*Time.deltaTime*1.3f;
			}
			else if(paying_out)
			{
				if(speedTowardsPivot <= payOutSpeed)
                {
					rigid_body.velocity = GetComponent<Rigidbody2D>().velocity - (Vector2)(payOutSpeed*directionToPivot);	
				}
				ropeLength = ropeLength + payOutSpeed*Time.deltaTime;

			}
			if(!allowRotation)
			transform.rotation = Quaternion.FromToRotation(Vector2.up,directionToPivot);
		}
        else
        {
            Destroy(GetComponent<Rigidbody2D>());
            addRB = true;
        }
	}
	
	void Update()
	{
		renderRope();	
	}
	
	public void SetRopeLength()
	{
		ropeLength = Vector2.Distance(pivotList[pivotList.Count-1],this.transform.position);
	}
	void renderRope()
	{
		if(pivotAttached)
		{
			rope.SetVertexCount(1+pivotList.Count);
			rope.SetPosition(0,transform.TransformPoint(ropeBasePoint));
			for(int i = 0 ; i<pivotList.Count; i++)
			{
				rope.SetPosition(i+1,pivotList[pivotList.Count-1-i]);
			}
		}
		else
		{
			pivotList.Clear();
			currentSwingDirection.Clear();
			rope.SetVertexCount(1);
			rope.SetPosition(0,transform.position);
		}
	}
	
	void AddRopeBend(RaycastHit2D hit)
	{
		Vector3 direc = (Vector3)hit.point - hit.transform.position;
		pivotPoint =  hit.transform.position + direc.normalized*(direc.magnitude + 0.01f);
	}
	
	bool isBendClockwise()
	{
		Vector2 playerPos = transform.position;
		Vector2 vectorPlayerToCurrentPivot = (Vector2)pivotList[pivotList.Count-1] - playerPos;
		Vector2 vectorCurrentPivotToLastPivot = pivotList[pivotList.Count-2] - pivotList[pivotList.Count-1];
		float dot = (vectorPlayerToCurrentPivot.y*vectorCurrentPivotToLastPivot.x - vectorPlayerToCurrentPivot.x*vectorCurrentPivotToLastPivot.y);
		return dot<0;
	}
	
	public void AddPivot(Vector3 newPivot)
	{
		pivotList.Add(newPivot);
	}
	public void ReleaseRope()
	{
		pivotAttached = false;

		if(allowRotation)
			return;
		//rigid_body.constraints = RigidbodyConstraints2D.FreezeRotation;
		//rigid_body.constraints = RigidbodyConstraints2D.None;
	}
	public void AttachRope(Vector2 grapplePoint)
	{
		pivotPoint = grapplePoint;
		pivotAttached = true;
	}
	public void SetReelingIn(bool currentReelingState)
	{
		reeling_in = currentReelingState;
	}
	public void SetPayingOut(bool currentPayingState)
	{
		paying_out = currentPayingState;
	}

	void NeutraliseGravity()
	{
		rigid_body.AddForce(directionToPivot * Vector2.Dot(Physics2D.gravity * rigid_body.gravityScale * strength , directionToPivot) *-1);
	}

}
