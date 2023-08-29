using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Potion : MonoBehaviour,IInteractable
{

    [SerializeField] PotionType potionType;
    [SerializeField] float destroyAfterr=15f;
    [SerializeField] float addAmount=20f;
    [Range(0f,101f)]
    public float chance=20f;
    private float cuurentNumber=0;
    private Outline outline;
    private bool isLooking=false;

    void Start()
    {
        outline=GetComponent<Outline>();
        outline.enabled=false;
        Destroy(this.gameObject,destroyAfterr);
    }

    public void Interact()
    {
        CollectItem();
    }

   public bool Looking
    {
        // when accessing the property simply return the value
        get => isLooking;

        // when assigning the property apply visuals
        set
        {
            // same value ignore to save some work
            if(isLooking == value) return;

            // store the new value in the backing field
            isLooking = value;

            outline.enabled=isLooking;
        }
    }
    


    public void CollectItem()
    {
       
        //add health
        if(potionType==PotionType.Heal)
        {
            Health.instance.Addhelth(addAmount);
        }else if(potionType==PotionType.Magic)
        {
            Magic.instance.AddMagic(addAmount);
        }
        Destroy(this.gameObject);
    }

    public bool TakeChance()
    {
        cuurentNumber=Random.Range(1f,100f);
        return cuurentNumber<chance;
    }
    
  
}

public enum PotionType{
    Heal,
    Magic,
    Toxic
}
