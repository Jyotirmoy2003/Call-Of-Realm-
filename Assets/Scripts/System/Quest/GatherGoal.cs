using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GatherGoal : QuestGoal
{
    public int itemId;
    [HideInInspector]
     public Quest myquest;
    

    public GatherGoal(int item,string description,bool completed,int curretnAmount,int requiredAmount,Quest quest)
   {
        this.itemId=item;
        this.description=description;
        this.curretnAmount=curretnAmount;
        this.requiredAmount=requiredAmount;
        this.completed=completed;
        myquest=quest;
        qestGoalType=GoalType.Gather;
        Init();
   }
   public void EnterState(int itemId,string description,bool completed,int curretnAmount,int requiredAmount,Quest quest)
   {
     this.itemId=itemId;
        this.description=description;
        this.curretnAmount=curretnAmount;
        this.requiredAmount=requiredAmount;
        this.completed=completed;
        myquest=quest;
        qestGoalType=GoalType.Gather;
        Init();
   }

    //base class constructer
   public override void Init()
   {
        base.Init();
        //add this goal to the manager class for listening from game event
        QuestManager.instance.AddGoalToListener(this);
        
   }


   void GotItem(int item)
    {
        curretnAmount++;
        Evaluate();
        myquest.CheckGoals();
    }

     // will be called from the manager calss when listended to enemy died
   public override void ListenToEvent (Component sender,object data)
   {    
    Debug.Log("Listened"+(int)data);
        if(data is int)
        {
           
            if(itemId==(int)data)
            {
                GotItem((int)data);
            }
        }
   }
}
