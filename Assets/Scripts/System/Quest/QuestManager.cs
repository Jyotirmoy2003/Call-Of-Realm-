using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public QuestAssigner currentAssigner;
    public bool AlreadyQuestAssigned=false; 
    public KeyCode questAssignKey=KeyCode.KeypadEnter;
    [SerializeField] List<QuestGoal> GoalListener=new List<QuestGoal>();
    
    

    void Awake()
    {
        instance=this;
    }

    void Update()
    {
        
        
        if(currentAssigner!=null &&Input.GetKeyDown(questAssignKey))
        {
            if(AlreadyQuestAssigned)
            {
                UIManager.instance.ShowLog("Only one quest can be assigned at a time",4f);
                return;
            }
            AssignQuest();
            currentAssigner=null;
        }
    }



    
    public void AddGoalToListener(QuestGoal goal)=> GoalListener.Add(goal);
    public void RemoveGoalToListener(QuestGoal goal)=> GoalListener.Remove(goal);
    

    //assign the quest
    public void AssignQuest()
    {
        UIManager.instance.ShowLog("Quest Assigned",3f);
        UIManager.instance.ShowQuest(currentAssigner.QuestName,currentAssigner.objective);
        AlreadyQuestAssigned=true;
        currentAssigner.Assign();
    }

    public void LeaveQuest() => currentAssigner=null;
    

    //Listen to event when EnemyKilled
    public void OnEnemyKilled(Component sender,object data)
    {
            foreach (QuestGoal item in GoalListener)
            {
                
                if(item.qestGoalType==GoalType.Kill)
                {
                    item.ListenToEvent(sender,data);
                }
            }
    }

    //Listen to event when gatherItem
    public void OnGatherItem(Component sender,object data)
    {
            foreach (QuestGoal item in GoalListener)
            {
                if(item.qestGoalType==GoalType.Gather)
                    item.ListenToEvent(sender,data);
            }
    }
}
