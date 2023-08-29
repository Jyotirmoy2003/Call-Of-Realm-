using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[System.Serializable]
public class KillGoal : QuestGoal
{
     public int enemyId;
     [HideInInspector]
     public Quest myquest;
   



    //Constructer
   public KillGoal(int enemyId,string description,bool completed,int curretnAmount,int requiredAmount,Quest quest)
   {
        this.enemyId=enemyId;
        this.description=description;
        this.curretnAmount=curretnAmount;
        this.requiredAmount=requiredAmount;
        this.completed=completed;
        myquest=quest;
        qestGoalType=GoalType.Kill;
        Init();
   }
   //external fun to call constructer
   public void EnterState(int enemyId,string description,bool completed,int curretnAmount,int requiredAmount,Quest quest)
   {
     this.enemyId=enemyId;
        this.description=description;
        this.curretnAmount=curretnAmount;
        this.requiredAmount=requiredAmount;
        this.completed=completed;
        myquest=quest;
        qestGoalType=GoalType.Kill;
        Init();
   }


    //base class constructer
   public override void Init()
   {
        base.Init();
        //add this goal to the manager class for listening from game event
        QuestManager.instance.AddGoalToListener(this);
        
   }

   void EnemyDied()
   { 
          curretnAmount++;
          Evaluate();
          myquest.CheckGoals();
   }


    // will be called from the manager calss when listended to enemy died
   public override void ListenToEvent (Component sender,object data)
   {
        if(data is int)
        {
            if(enemyId==(int)data)
            {
               
                EnemyDied();
            }
        }
   }

}
