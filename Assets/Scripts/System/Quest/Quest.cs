using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class Quest : MonoBehaviour
{
   public List<QuestGoal> goals =new List<QuestGoal>();
    public string QuestName { get; set; }
    public string description { get; set; }
    public int experienceReward { get; set; }
    public bool completed { get; set; }
    private     QuestGoal questGoal;
    [SerializeField] GameEvent QuestCuurentAmountEvent;


    void Start()
    {
        string val="Done"+goals[0].curretnAmount.ToString()+"\n'R' to cancle Quest";
        UIManager.instance.QuestCuurentAmout(val);
    }
    public void CheckGoals()
    {
        completed=goals.All(g=>g.completed);
        string val="Done"+goals[0].curretnAmount.ToString()+"/n 'R' to cancle Quest";
        UIManager.instance.QuestCuurentAmout(val);

        if(completed)
        {
            GiveReward();   
        }

    }

    public void GiveReward()
    {
        UIManager.instance.QuestCuurentAmout("");
        QuestManager.instance.AlreadyQuestAssigned=false;
        UIManager.instance.ShowQuest("","");
        UIManager.instance.ShowLog("Quest Completed,Talk To Sage",2f);
        
        //give reward to player
        QuestManager.instance.RemoveGoalToListener(questGoal);
        Memory.instance.AddMemory(experienceReward);

        Destroy(this);
    }

    public void AddQuestGoals(QuestGoal questGoal)
    {
        this.questGoal=questGoal;
        goals.Add(questGoal);
        experienceReward+=questGoal.experienceReward;
    }

    public void RemoveQuest()
    {
        UIManager.instance.QuestCuurentAmout("");
        QuestManager.instance.AlreadyQuestAssigned=false;
        UIManager.instance.ShowQuest("","");
        QuestManager.instance.RemoveGoalToListener(questGoal);
        Destroy(this);
    }



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            RemoveQuest();
        }
    }




}
