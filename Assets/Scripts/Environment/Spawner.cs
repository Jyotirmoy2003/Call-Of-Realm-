using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] AreaPetrol areaPetrol;
    [SerializeField] GameObject prefab;
    [SerializeField] float rateOfspawn;
    [SerializeField] float rateOffset;
    [SerializeField] Transform nextSpawnPos;
    [SerializeField] LayerMask spawnAbleLayer;
    [SerializeField] float spawnerRange=5;
    [SerializeField] bool isAnimalSpawner=false;

    [Tooltip("Add this only when the prefabs has fun to petrol through")]
    [SerializeField] List<Transform> areaPetrolPoints=new List<Transform>();

    [SerializeField] bool IsActive=false;

    private float currentRate;
    private Transform myTransform;
    private bool IsSpawnAble=false;
    private delegate void MyDelegate();
    private MyDelegate myDelegate;


    void Start()
    {
        myTransform=GetComponent<Transform>();
        myDelegate=(isAnimalSpawner)?SpawnAnimal:SpawnPrefab;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while(IsActive)
        {
            GetRandomrate();
            SetNextPosition();
            myDelegate();
            yield return new WaitForSeconds(currentRate);
        }
    }


    void SpawnPrefab()
    {
        Instantiate(prefab,nextSpawnPos.position,Quaternion.identity);
    }
    void SpawnAnimal()
    {
       Animal am=Instantiate(prefab,nextSpawnPos.position,Quaternion.identity).GetComponent<Animal>();
       am.petrolPoins=areaPetrolPoints;
        areaPetrol.AddAnimal(am);

    }



    void GetRandomrate()
    {
        currentRate=Random.Range(rateOfspawn+rateOffset,rateOfspawn-rateOffset);
    }

    void SetNextPosition()
    {
        //until we found a place to spawn
        while(!IsSpawnAble)
        {
           
            Vector3 pos;
            pos.x=Random.Range(myTransform.position.x+spawnerRange,myTransform.position.x-spawnerRange);
            pos.z=Random.Range(myTransform.position.z+spawnerRange,myTransform.position.z-spawnerRange);

            nextSpawnPos.position=new Vector3(pos.x,nextSpawnPos.position.y,pos.z);

            
            //check using sphere cast if its spawnable place
            IsSpawnAble=Physics.CheckSphere(nextSpawnPos.position,spawnerRange,spawnAbleLayer);
        }
        //reste the bool
        IsSpawnAble=false;
    }

    public void ActiveSpawner(bool val)
    {
        IsActive=val;
        if(val)
        StartCoroutine(Spawn());
    }

    
}
