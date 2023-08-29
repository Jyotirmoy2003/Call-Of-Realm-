using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="QestsObjects")]
public class QuestObject:ScriptableObject
{
   public GoalType goalType;
   
    public GatherGoal gatherGoal;
    public KillGoal KillGoalData;
    
}
