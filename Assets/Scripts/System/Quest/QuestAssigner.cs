using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(Outline),typeof(Villegers),typeof(DialugeTrigger))]
public class QuestAssigner : MonoBehaviour,IInteractable
{
    [SerializeField] List<QuestObject> questObjects=new List<QuestObject>();
    public string QuestName,objective;
    private Quest myQuest;
    private Outline outline;
     private DialugeTrigger Dt;
    private bool isLooking=false;
    private Villegers villeger;



     void Start()
    {
        outline=GetComponent<Outline>();
        Dt=GetComponent<DialugeTrigger>();
        villeger=GetComponent<Villegers>();
        Dt.dialouge.sentences.Add("press 'Tab' to accept it:");

        //set properties of outline
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
                QuestManager.instance.LeaveQuest();
                DialougeManager.instance.EndDialouge();
                villeger.Interact(false);
            }
            
        }
    }
    



    public void Interact()
    {
        villeger.Interact(true);
        QuestManager.instance.currentAssigner=this;
        
        if(!DialougeManager.instance.triggered)
        {
            Dt.Trigger();
        }else if(DialougeManager.instance.triggered)
        {
            DialougeManager.instance.DisplayNextSentence();
        }
    }
   
    public void Assign()
    {
        myQuest=QuestManager.instance.gameObject.AddComponent<Quest>();

        //go throug all goals and add them in the current quest
        foreach (QuestObject item in questObjects)
        {

            if(item.goalType==GoalType.Kill)
            {
                myQuest.AddQuestGoals(item.KillGoalData);
                item.KillGoalData.EnterState(item.KillGoalData.enemyId, item.KillGoalData.description, false,0, item.KillGoalData.requiredAmount,myQuest);
                
            }else if(item.goalType==GoalType.Gather){
                myQuest.AddQuestGoals(item.gatherGoal);
                item.gatherGoal.EnterState(item.gatherGoal.itemId, item.gatherGoal.description, false,0, item.gatherGoal.requiredAmount,myQuest);
            }
        }
    }



    
}



