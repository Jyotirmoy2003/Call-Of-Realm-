
using UnityEngine;

public class SkipGame : MonoBehaviour
{
    private int currentCompletedEvent=0;
    [SerializeField] int showCheatAfter=2;
    [SerializeField] float showTimmer=2f;
    [SerializeField] GameEvent ActiveCheatEvent;
   

    public void ListenToQuest(Component sender,object data)
    {
        currentCompletedEvent++;
        if(currentCompletedEvent==showCheatAfter)
        {
            Invoke("ShoCheat",showTimmer);
        }
    }

    void ShoCheat()
    {
        UIManager.instance.ShowLog("As It is s Jam game, \n Cheat:- 'Backspace' to get memory \n Must talk to sage after getting some memory back",12f);
        ActiveCheatEvent.Raise(this,true);
    }
}
