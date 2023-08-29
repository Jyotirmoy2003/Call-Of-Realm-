using UnityEngine;

public class Memory : MonoBehaviour
{
    public static Memory instance;
    [SerializeField] float maxMemory=100f;
    [SerializeField] HealthBar bar;
    [SerializeField] GameEvent gotMemoryEvent,gameWinEvent;
    private float currentMemory=0f;

    void Awake()
    {
        instance=this;
    }
    void Start()
    {
        currentMemory=0;
        bar.SetMaxHealth(maxMemory);
        bar.SetHealth(currentMemory);
        
    }

    public void AddMemory(float amount)
    {
        currentMemory+=amount;
        if(currentMemory>=maxMemory)
        {
            currentMemory=maxMemory;
            gameWinEvent.Raise(this,true);
        }
        gotMemoryEvent.Raise(this,amount);

        bar.SetHealth(currentMemory);
    }

    public bool IsMaxed() {
        return currentMemory>=maxMemory;
    }
    
    
}
