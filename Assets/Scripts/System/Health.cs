using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour,IDamageable
{
    public static Health instance;

    [SerializeField] float totalHealth=100;
    [SerializeField] GameEvent tookDamageEvent;
    [SerializeField] HealthBar bar;
 

    private float currentHealt=0;

    void Awake()
    {
        instance=this;
    }
    

    void Start()
    {
      
            currentHealt=totalHealth;
            bar.SetMaxHealth(totalHealth);
            bar.SetHealth(currentHealt);
        
    }

    public void TakeDamage(float amount)
    {
        currentHealt-=amount;
        tookDamageEvent.Raise(this,amount);

        if(currentHealt<=0)
        {
            UIManager.instance.ShowRestartPanel();
            //Destroy(this.gameObject);
        }
       
      
            bar.SetHealth(currentHealt);
        
    }

    public void Addhelth(float amount)
    {
        currentHealt+=amount;
        if(currentHealt>=totalHealth)
        {
            currentHealt=totalHealth;
        }
        bar.SetHealth(currentHealt);
    }

    
}
