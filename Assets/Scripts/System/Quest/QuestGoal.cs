
using UnityEngine;

[System.Serializable]
public class QuestGoal 
{
    public string description  ;
    public bool completed ;
    public int experienceReward;
    public int curretnAmount ;
    public int requiredAmount ;
    public GoalType qestGoalType ;


    public virtual void Init()
    {
        //default initial stuff
    }


    public void Evaluate()
    {
        if(curretnAmount>=requiredAmount) Complete();

    }

    public void Complete()
    {
        completed=true;
    }

    public virtual void ListenToEvent(Component sender,object data){

    }
}

public enum GoalType{
    Kill,
    Gather,
}
