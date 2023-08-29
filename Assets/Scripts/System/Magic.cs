using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public static Magic instance;
    [SerializeField] float maxMagic=100f;
    [SerializeField] HealthBar bar;
    private float currentMagic=0f;

    void Awake()
    {
        instance=this;
    }
    void Start()
    {
        currentMagic=maxMagic;
        bar.SetMaxHealth(maxMagic);
        bar.SetHealth(currentMagic);
        
    }

    public void UseMagic(float amount)
    {
        if(currentMagic<=0) return;
        currentMagic-=amount;

        bar.SetHealth(currentMagic);
    }

    public void AddMagic(float amount)
    {
        currentMagic+=amount;
        if(currentMagic>=maxMagic)
        {
            currentMagic=maxMagic;
        }

        bar.SetHealth(currentMagic);
    }

    public bool AbleToUseMagic(float amount)
    {
        return currentMagic>=amount;
    }
}
