using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator),typeof(NavMeshAgent))]
public class Villegers : MonoBehaviour
{
    [HideInInspector]
    public bool IsWaiting=false;
    [SerializeField] float walkSpeed=4f,waitTime=3f;
    [SerializeField] List<Transform> petrolpoints=new List<Transform>();
    [SerializeField] bool IsRandomMovement=false;
    [SerializeField] Animator animator;
    [SerializeField] float playBackSpeed=1f;
    [SerializeField] bool isKinght=false;
    

    private NavMeshAgent navMeshAgent;
    private int index=0;
    private float currentWaiTime=2f;
    private delegate void MyDelegate();
    MyDelegate PetrolPointSelector;

    void Start()
    {
        navMeshAgent=GetComponent<NavMeshAgent>();
        navMeshAgent.speed=walkSpeed;
        if(animator==null) animator=GetComponent<Animator>();

        animator.speed=playBackSpeed;
        //select the according fun (delegate to reduce update call)
        PetrolPointSelector=(IsRandomMovement)?ChoseRandomPetro:Increase;

        PetrolPointSelector();
        animator.Play("Walk");

        if(isKinght)
        {
            AudioManager.instance.PlaySound("Knight_Walk",this.gameObject);
        }
    }

    
    void Update()
    {
        //when reached to destiantion
        if(navMeshAgent.remainingDistance<=0.5 && !IsWaiting)
        {
            //choose new PetrolPoint
           PetrolPointSelector();
        }
    }

    void ChoseRandomPetro()
    {
        index=UnityEngine.Random.Range(0,petrolpoints.Capacity);
        currentWaiTime=UnityEngine.Random.Range(0,waitTime);

        navMeshAgent.destination=petrolpoints[index].position;
    }

    
    //when going 
    void Increase()
    {
        //if reached at the end
        if(index==petrolpoints.Capacity-1)
        {
            PetrolPointSelector=Decrease;
           StartCoroutine(Wait());

            
            return;   
        }
        index++;
        navMeshAgent.destination=petrolpoints[index].position; 
        
    }
    //when comming back
    void Decrease()
    {
        //if reached at the end
        if(index==0)
        {
            PetrolPointSelector=Increase;
            StartCoroutine(Wait());

            
            return;
        }
        index--;
       
       navMeshAgent.destination=petrolpoints[index].position; 
    }

    IEnumerator Wait()
    {
        animator.Play("Idle");
        IsWaiting=true;
        yield return new WaitForSeconds(waitTime);
        IsWaiting=false;
        PetrolPointSelector(); 
        animator.Play("Walk"); 
         
    }


    //when player interact with nocs to stop them
    public void Interact(bool val)
    {
        if(val)
        {
            StopCoroutine(Wait());
            navMeshAgent.speed=0;
            IsWaiting=true;
            animator.Play("Talk");
            if(isKinght)AudioManager.instance.StopSound("Knight_Walk",this.gameObject);
            AudioManager.instance.PlaySound("Talking",this.gameObject);
        }else{
            IsWaiting=false;
            navMeshAgent.speed=walkSpeed;
            animator.Play("Walk");
            navMeshAgent.destination=petrolpoints[index].position;

            AudioManager.instance.StopSound("Talking",this.gameObject);
            if(isKinght)AudioManager.instance.PlaySound("Knight_Walk",this.gameObject);
            
        }

    }
}
