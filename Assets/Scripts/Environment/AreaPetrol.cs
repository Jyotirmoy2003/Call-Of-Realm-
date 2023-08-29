using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AreaPetrol : MonoBehaviour
{
    [SerializeField] List<Spawner> spawner=new List<Spawner>();
    private List<Animal> animals=new List<Animal>();
    private bool IsActive=false;
    void Start()
    {
        foreach (Spawner item in spawner)
        {
            item.gameObject.SetActive(false);
        }
        
    }

   

    void OnTriggerEnter(Collider info)
    {
        if(info.CompareTag("Player"))
        {
            IsActive=true;
            Active(IsActive);
        }
    }

    void OnTriggerExit(Collider info)
    {
        if(info.CompareTag("Player"))
        {
            IsActive=false;
            Active(IsActive);
        }
    } 


    void Active(bool val)
    {
        foreach (Spawner item in spawner)
        {
            item.gameObject.SetActive(val);
            item.ActiveSpawner(val);
        }
        foreach (Animal item in animals)
        {
            item.check=val;
        }
    }

    public void AddAnimal(Animal am)
    {
        am.check=IsActive;
        animals.Add(am);
        
    } 
   


}
