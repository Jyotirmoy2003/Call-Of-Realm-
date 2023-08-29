
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public bool IsGround;
    [SerializeField]  CharacterController controller;
    [SerializeField] float sprintSpeed=10,walkSpeed=5,crouchSpeed=2;
    [SerializeField] float groundDistance=0.3f;
    [SerializeField] LayerMask ground;
    [SerializeField] Transform orientation;
    [SerializeField] float JumpForce=5;
    [SerializeField] float gravity=-9.8f;

    [SerializeField] KeyCode jumpKey=KeyCode.Space;
    [SerializeField] KeyCode crouchKey=KeyCode.LeftControl;
    [SerializeField] KeyCode sprintKey=KeyCode.LeftShift;

    [Range(.1f,3f)]
    [SerializeField] float FootStepPitch=.2f;
    Vector3 velocity;
    private AudioSource footStep;
    private Transform myTransform;
    private float horizontal;
    private float vertical;
    private  float curentSpeed;
    private MovementState state;
    private Rigidbody rb;
    private bool settingPlayerpos=false;

    private float startCrouchHight,crouchhight;

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
        //Play footstep sound
       // AudioManager.instance.PlaySound("FootStep",this.gameObject);
        myTransform=GetComponent<Transform>();
        startCrouchHight=myTransform.localScale.y;
        crouchhight=startCrouchHight/2;
        rb=GetComponent<Rigidbody>();

    }

    
    void Update()
    {
       PlayerInput();
       Jump();
      if(!settingPlayerpos)
            Move();
       
       Statehandeler();
       
    }

    void FixedUpdate()
    {
         
    }

    void PlayerInput()
    {
        IsGround=Physics.CheckSphere(orientation.position,groundDistance,ground);

        if(IsGround && velocity.y<0)
        {
            velocity.y=-1f;
        }

        horizontal=Input.GetAxis("Horizontal");
		vertical=Input.GetAxis("Vertical");

        //start crouch
        if(Input.GetKeyDown(crouchKey))
        {
            myTransform.localScale=new Vector3(myTransform.localScale.x,crouchhight,myTransform.localScale.z);
            controller.Move(Vector3.down*5f);
        }
        //stop crouch
        if(Input.GetKeyUp(crouchKey))
        {
            myTransform.localScale=new Vector3(myTransform.localScale.x,startCrouchHight,myTransform.localScale.z);

        }
    }

    void Move()
    {
        curentSpeed=Input.GetKey(sprintKey) ? sprintSpeed : walkSpeed; //set speed

        Vector3 move=myTransform.right*horizontal+myTransform.forward*vertical;
        move.Normalize();
        controller.Move(move*curentSpeed*Time.deltaTime);
        
        
         controller.Move(velocity*Time.deltaTime);

    }
    void Jump()
	{
		if(Input.GetKey(jumpKey) && IsGround)
		{
            velocity.y=Mathf.Sqrt(JumpForce*-2f*gravity);
		}
	}

     void Statehandeler()
    {
        //Mode:-Crouch
        if(Input.GetKeyDown(crouchKey))
        {
            state=MovementState.crouch;
            curentSpeed=crouchSpeed;
        }
        //Mode:-sprintig
        else if(IsGround && Input.GetKey(sprintKey))
        {
            state=MovementState.sprint;
            curentSpeed=sprintSpeed;
        }
        //Mode:-walking 
        else if(IsGround)
        {
            state=MovementState.walk;
            curentSpeed=walkSpeed;
        }

        //Mode:- Air
        else{
            state=MovementState.air;
            velocity.y+=gravity*Time.deltaTime;
        }
    }

	public void SetPlayerPosition(Vector3 val)
	{
        settingPlayerpos=true;
		Debug.Log("SetPlayer positon");
		myTransform.position=val;
        Invoke("ResetBool",0.1f);
        
	}
    void ResetBool()
    {
        settingPlayerpos=false;
    }




    public void SetPlayerData(float sprintSpeed,float gravity)
	{
		this.sprintSpeed=sprintSpeed;
        this.gravity=gravity*-1;
	}

   
}
