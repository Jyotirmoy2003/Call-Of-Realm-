using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Collectable : MonoBehaviour,IInteractable,IGatherable
{
    public int itemId=0;
    [Range(0f,101f)]
   public float chance;
   public int ID { get; set; }

   [SerializeField] float destroyAfter=15f;
   [SerializeField] GameEvent GatherItemEvent;

   private  bool isLooking=false;
   private Outline outline;
   private float currentnumber=0f;
   
    void Start()
    {
        ID=itemId;

        outline=GetComponent<Outline>();
        outline.enabled=false;
        Destroy(this.gameObject,destroyAfter);
    }

   

    public bool TakeChance()
    {
        currentnumber=Random.Range(0f,100f);
        return currentnumber<=chance;
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

    public void Interact()
    {
        CollectItem();
    }

    public void CollectItem()
    {
        GatherItemEvent.Raise(this,ID);
        Destroy(this.gameObject);
    }
}
