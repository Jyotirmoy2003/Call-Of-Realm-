using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(Outline))]
public class HaveConverttion : MonoBehaviour,IInteractable
{
    [SerializeField] Color outlineColor=Color.black;
    [Range(1f,10f)]
    [SerializeField] float outlineWidth=5;
    private DialugeTrigger Dt;
    private bool isLooking=false;
    private Outline outline;
    private QuestAssigner questAssigner;


    void Start()
    {
        outline=GetComponent<Outline>();
        Dt=GetComponent<DialugeTrigger>();

        //set properties of outline
        outline.OutlineWidth=outlineWidth;
        outline.OutlineColor=outlineColor;
        outline.enabled=false;
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
                AudioManager.instance.StopSound("Talking",this.gameObject);
            ;}
            
        }
    }

    public void Interact()
    {
       
        if(!DialougeManager.instance.triggered)
        {
            Dt.Trigger();
            AudioManager.instance.PlaySound("Talking",this.gameObject);

        }else
        {
            DialougeManager.instance.DisplayNextSentence();
        }
    }
   
   
}
    
