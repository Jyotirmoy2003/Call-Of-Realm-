
using System.Collections.Generic;
using UnityEngine;



public class Sage : MonoBehaviour,IInteractable
{
    public static Sage instance;
    [SerializeField] List<DialugeTrigger> dts=new List<DialugeTrigger>();
    private Outline outline;

    private Animator animator;
    private bool isLooking=false;
    public int index=0;
    private bool IsGameWin=false;


    void Start()
    {
        outline=GetComponent<Outline>();
        animator=GetComponent<Animator>();
        outline.enabled=false;

    }

    public void Interact()
    {   
        if(!DialougeManager.instance.triggered)
        {
            dts[index].Trigger();
            animator.Play("Talk");
            AudioManager.instance.PlaySound("Talking",this.gameObject);
        }else
        {
            DialougeManager.instance.DisplayNextSentence();
        }
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
            if(!isLooking) {
                DialougeManager.instance.EndDialouge();
                animator.Play("Idle");
                AudioManager.instance.StopSound("Talking",this.gameObject);
            }
            
        }
    }

    public void GotMemoryEvent(Component sender,object data)
    {
        if(index<dts.Count-1)
        {
            
            index++;
        }
        
    }
    public void GameWinEvent(Component sender,object data)
    {
        if(data is bool)
        {
           if((bool)data)
           {
                IsGameWin=true;
           }
        }
        
    }

    
}
