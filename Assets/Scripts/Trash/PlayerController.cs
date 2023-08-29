using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	#region Variables
	public static PlayerController instance;
  
	[SerializeField] LayerMask ground;
	[SerializeField] float JumpForce=2,groundDistance=0.2f;
	[SerializeField] CharacterController characterController;
    [SerializeField] Transform cameraHolder;
    [SerializeField] float sprintSpeed,walkSpeed,smoothTime;
    [SerializeField] Animator animator;
	[SerializeField] ParticleSystem dustParticel;
	[SerializeField] Transform playerObj;
	[SerializeField] Transform orientation;
	[SerializeField] CinemachineBrain brain;
	[SerializeField] CinemachineFreeLook freeLook;

    private float verticalLookRotation;
	private bool IsGround;
	private Vector3 moveDir;
    private Transform myTransform;
	[HideInInspector]
    public float curentSpeed=0;
	private Rigidbody rb;
	#endregion
	

    void Awake()
	{
		instance=this;
		rb=GetComponent<Rigidbody>();
        myTransform=GetComponent<Transform>();
	}
    void Start()
    {
		
        
    }


    void Update()
    {
		
        
        Look();
        Move();
        Jump();

      
    }
    






    void Look()
	{
		Vector3 viewDir=myTransform.position-new Vector3(cameraHolder.position.x,myTransform.position.y,cameraHolder.position.z);
		orientation.forward=viewDir.normalized;
	}
    void Move()
	{

		IsGround=Physics.CheckSphere(orientation.position,groundDistance,ground);
        

		float horizontal=Input.GetAxis("Horizontal");
		float vertical=Input.GetAxis("Vertical");
		
		

		moveDir =( orientation.forward*vertical + orientation.right*horizontal).normalized;
		//if got input
		if(moveDir.magnitude>=0.1f)
		{
			curentSpeed=Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed; //set speed
			playerObj.forward=Vector3.Slerp(playerObj.forward,moveDir,Time.deltaTime*smoothTime);//smooth out player Object
		
			//calculate angel
			float targetAngel=Mathf.Atan2(moveDir.x,moveDir.z)*Mathf.Rad2Deg;
			//smooth out angel
			float angel=Mathf.SmoothDampAngle(myTransform.eulerAngles.y,targetAngel,ref verticalLookRotation,smoothTime);
			//set angel to player
			myTransform.rotation=Quaternion.Euler(0f,angel,0f);
			characterController.Move(moveDir*curentSpeed*Time.deltaTime);
			
		}else{
			curentSpeed=0;
		}
		animator.SetFloat("Speed",curentSpeed);
		
	}
	void Jump()
	{
		if(Input.GetKeyDown(KeyCode.Space) && IsGround)
		{
			animator.SetTrigger("Jump");
			rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
		}
	}

	


	//fun to set player position to a position
	public void SetPlayerPosition(Transform val)
	{
		Debug.Log("SetPlayer positon");
		myTransform.position=val.position;
	}
	public void SetPlayerData(float val)
	{
		sprintSpeed=val;
	}
	
	
}
