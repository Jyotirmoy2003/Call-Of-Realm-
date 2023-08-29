
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSPlayerController : MonoBehaviour
{
    public static FPSPlayerController instance;
    [SerializeField] float walkSpeed=5f,sprintSpeed=10f,crouchSpeed=3f,JumpForce=0.5f,airMultiplier=0.4f;
    [SerializeField] Transform orientation,myTransform;
    [SerializeField] LayerMask ground;
    [SerializeField] float groundDistance=0.2f;
    [SerializeField] float groundDrag=20;
    [SerializeField] float gravity=5f;

    [SerializeField] KeyCode jumpKey=KeyCode.Space;
    [SerializeField] KeyCode crouchKey=KeyCode.LeftControl;
    [SerializeField] KeyCode sprintKey=KeyCode.LeftShift;

    private float startCrouchHight,crouchhight;
    private bool IsGround=false;
    private float moveSpeed;

    float horizontal,vertical;

    private Vector3 moveDir;
    private Rigidbody rb;
    private MovementState state;

    private enum MovementState{
        sprint,
        walk,
        crouch,
        air,

    }



    void Awake()
    {
        instance=this;
    }

    void Start()
    {
        rb=GetComponent<Rigidbody>();
        rb.freezeRotation=true;
        startCrouchHight=myTransform.localScale.y;
        crouchhight=startCrouchHight/2;

    }
    void Update()
    {
        PlayerInput();
        Statehandeler();
        Jump();
    }

    void PlayerInput()
    {
        //movement
        horizontal=Input.GetAxis("Horizontal");
        vertical=Input.GetAxis("Vertical");
        //cursor
        
        //start crouch
        if(Input.GetKeyDown(crouchKey))
        {
            myTransform.localScale=new Vector3(myTransform.localScale.x,crouchhight,myTransform.localScale.z);
            rb.AddForce(Vector3.down*5f,ForceMode.Impulse);
        }
        //stop crouch
        if(Input.GetKeyUp(crouchKey))
        {
            myTransform.localScale=new Vector3(myTransform.localScale.x,startCrouchHight,myTransform.localScale.z);

        }

    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        IsGround=Physics.CheckSphere(orientation.position,groundDistance,ground);

        if(IsGround) rb.drag=groundDrag;
        else {rb.drag=0;}

        //calculate the movement direction
        moveDir=orientation.forward*vertical+orientation.right*horizontal;
        if(IsGround)
            rb.AddForce(moveDir.normalized*moveSpeed*10f,ForceMode.Force);
        else
            rb.AddForce(moveDir.normalized*moveSpeed*10f*airMultiplier,ForceMode.Force);
    
    }


    void Jump()
    {
        if(Input.GetKey(jumpKey) && IsGround)
        {
            rb.AddForce(myTransform.up*JumpForce,ForceMode.Impulse); //force mode impulse for only appling once thte force
        }
    }

    void Statehandeler()
    {
        //Mode:-Crouch
        if(Input.GetKeyDown(crouchKey))
        {
            state=MovementState.crouch;
            moveSpeed=crouchSpeed;
        }
        //Mode:-sprintig
        else if(IsGround && Input.GetKey(sprintKey))
        {
            state=MovementState.sprint;
            moveSpeed=sprintSpeed;
        }
        //Mode:-walking 
        else if(IsGround)
        {
            state=MovementState.walk;
            moveSpeed=walkSpeed;
        }

        //Mode:- Air
        else{
            state=MovementState.air;
            rb.AddForce(Vector3.down*gravity,ForceMode.Force);
        }
    }





    //fun to set player position to a position
	public void SetPlayerPosition(Vector3 val)
	{
		myTransform.position=val;
	}
	public void SetPlayerData(float sprintSpeed,float gravity)
	{
		this.sprintSpeed=sprintSpeed;
        this.gravity=gravity;
	}
}
