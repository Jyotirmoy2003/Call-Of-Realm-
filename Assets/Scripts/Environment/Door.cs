using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]

public class Door : MonoBehaviour,IInteractable
{
     private Outline outline;
   private float timmerForOutline;
   private bool doorOpen=false;
   private Animator animator;
   private bool isLooking=false;

   
    void Start()
    {
        outline=GetComponent<Outline>();
        outline.enabled=false;
        animator=GetComponent<Animator>();
    }
    public void Interact()
    {
        IntDoor();
    }

    public bool Looking
    {
        // when accessing the property simply return the value
        get => isLooking;

        // when assigning the property apply visuals
        set
        {
            // same value ignore to save some work
            if(isLooking == value) return;

            // store the new value in the backing field
            isLooking = value;

            outline.enabled=isLooking;
        }
    }
    void IntDoor()  //Mian Fun To Open And Close
    {
            if(!doorOpen)
            {
               animator.SetBool("DoorOpen",true);
              
                
            }else
            {
                animator.SetBool("DoorOpen",false);
                
            }
            doorOpen=!doorOpen;

            

    }

}
