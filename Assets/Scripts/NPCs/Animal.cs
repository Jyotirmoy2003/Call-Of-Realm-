using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public abstract class AnimalBase{
    public abstract void EnterState(Animal am);
    public abstract void UpdateState(Animal am);
    public abstract void COllidedState(Animal am,Collider info);
}

public class AnimalPetrolState:AnimalBase{

    private int index=0;
    

    public override void EnterState(Animal am)
    {
        //assign petrol speed
        am.navMeshAgent.speed=am.petrolSpeed;
        am.animator.Play("Walk");
        ChoseRandomPetro(am);
    }
    public override void UpdateState(Animal am)
    {
        
        //if cuurently waiting the return
        if(am.IsWaiting) return;
        //if reached to the destination then chose new petrolpoint
        if(am.navMeshAgent.remainingDistance<=0.3)
        {
            ChoseRandomPetro(am);
        }
        //if plaeyr is too close then swtich acoording state
        if(am.check && Vector3.Distance(am.playerPosition.position,am.myTransform.position)<am.awareRange)
        {
            //stop coroutin and rest the bool
            am.StopCoroutine(am.Wait());
            am.IsWaiting=false;
            if(am.avoidPlayer)
            {
                am.SwitchState(am.animalPlayerAvoid);
            }else if(am.attackPlayer){
                am.SwitchState(am.animalAttackPlayerState);
            }
        }
        
    }

    public override void COllidedState(Animal am,Collider info)
    {

    }

    //set new petrolpoint and wait for some time
    void ChoseRandomPetro(Animal am)
    {
        am.animator.Play("Idle_A");
        index=Random.Range(0,am.petrolPoins.Capacity);
        am.StartCoroutine(am.Wait());
        am.navMeshAgent.destination=am.petrolPoins[index].position;
    }
}

public class AnimalAttackPlayerState:AnimalBase
{
    private  IDamageable playerDamageable;

    public override void EnterState(Animal am)
    {
        am.animator.Play("Run");
        //assign chase speed
        am.navMeshAgent.speed=am.chasePlayerSpeed;
        playerDamageable=am.playerPosition.GetComponent<IDamageable>();
        
    }

    public override void UpdateState(Animal am)
    {
        //set destination to player
        am.navMeshAgent.destination=am.playerPosition.position;

        //if player is out of range
        if(Vector3.Distance(am.playerPosition.position,am.myTransform.position)>am.leavePlayerRange)
        {
            am.SwitchState(am.animalPetrolState);
        }


        
    }

    public override void COllidedState(Animal am,Collider info)
    {
        //test if reached to player and cuurently not attcking
        if(info.CompareTag("Player") && !am.IsAttacking)
        {
            playerDamageable.TakeDamage(am.attackDamage);
            am.StartCoroutine(am.AttackCoolDown());

        }
    }
}


public class AnimalPlayerAvoid:AnimalBase
{
   
    

    public override void EnterState(Animal am)
    {
        //assign the chase player speed as it is the max speed
        am.navMeshAgent.speed=am.chasePlayerSpeed;

        am.animator.Play("Run");
    }

    public override void UpdateState(Animal am)
    {
       
            // Calculate a point to move towards that avoids the player
            Vector3 avoidanceDirection =am.myTransform.position - am.playerPosition.position;
            Vector3 avoidancePoint = am.myTransform.position + avoidanceDirection.normalized * am.avoidanceDistance;

            // Set the destination for the NavMeshAgent to the avoidance point
            am.navMeshAgent.SetDestination(avoidancePoint);

            if(Vector3.Distance(am.myTransform.position,am.playerPosition.position)>am.avoidanceDistance)
            {
                am.SwitchState(am.animalPetrolState);
            }
           
       
    }

    public override void COllidedState(Animal am,Collider info)
    {

    }
}


public class AnimalDeath:AnimalBase{
    public override void EnterState(Animal am)
    {
        am.navMeshAgent.destination=am.myTransform.position;
        am.navMeshAgent.speed=0;

        am.Die();
    }

    public override void UpdateState(Animal am)
    {
        
    }

    public override void COllidedState(Animal am,Collider info)
    {

    }
}


[RequireComponent(typeof(NavMeshAgent),typeof(Animator))]
public class Animal : MonoBehaviour,IDamageable,IEnemy
{
    public float destroyTimmer=30f;
    public List<Transform> petrolPoins=new List<Transform>();
    public bool IsWaiting=false;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public float health;
    public Collectable collectable;
    public List<Potion> potions=new List<Potion>();
    public float dieAfter=2f;
    public int maxNumberOfDrop=4;
    [Header("PetrolState")]
    [Tooltip("how much time it should wait in each petrol point")]
    [SerializeField] float waitTimer=3f;
    public bool avoidPlayer=false;
    public bool attackPlayer=false;
    public float awareRange=5f;
    public float petrolSpeed=5f;

    [Header("PlayerAttack")]
    [Tooltip("Max speed this Entity can run")]
    public float chasePlayerSpeed=10f;
    [Tooltip("min range when it will attack and give damage")]
    public float minRangeToAttack=1f;
    public float attackDamage=5f;
    [Tooltip("min range when it will let player go and get back to petrol")]
    public float leavePlayerRange=15f;
    [Tooltip("timmer between each damage")]
    public float attackCoolDown=0.5f;
    [HideInInspector]
    public bool IsAttacking=false;
    public float avoidanceDistance=7f;

   
    public Transform myTransform;
    public bool check=false;
    public Transform playerPosition;

    public int ID { get; set; }
    [SerializeField] GameEvent EnemyDiedEvent;
    [SerializeField] int animalID=0;
    

    public AnimalPetrolState animalPetrolState=new AnimalPetrolState();
    public AnimalPlayerAvoid animalPlayerAvoid=new AnimalPlayerAvoid();
    public AnimalAttackPlayerState animalAttackPlayerState= new AnimalAttackPlayerState();
    public AnimalDeath animalDeath=new AnimalDeath();
    public AnimalBase currentState;

    
#region  MonoBehaviour
    void Start()
    {
        Destroy(this.gameObject,destroyTimmer);
        ID=animalID;

        animator=GetComponent<Animator>();
        myTransform=GetComponent<Transform>();
        navMeshAgent=GetComponent<NavMeshAgent>();
        playerPosition=GameObject.FindGameObjectWithTag("Player").transform;

        currentState=animalPetrolState;
        currentState.EnterState(this);
    }

   
    void Update()
    {
        currentState.UpdateState(this);
    }

     void OnTriggerEnter(Collider info)
    {
        currentState.COllidedState(this,info);
    }
    void OnTriggerStay(Collider info)
    {
        currentState.COllidedState(this,info);
    }

    #endregion

    public void SwitchState(AnimalBase am)
    {
        currentState=am;
        currentState.EnterState(this);
    }

    

    public IEnumerator Wait()
    {
        IsWaiting=true;
        yield return new WaitForSeconds(waitTimer);
        IsWaiting=false;
    }

    public IEnumerator AttackCoolDown()
    {
        animator.Play("Attack");
        IsAttacking=true;
        yield return new WaitForSeconds(attackCoolDown);
        IsAttacking=false;
        animator.Play("Run");

    }

   

   

    #region  Idamagable
    public void TakeDamage(float amount)
    {
        health-=amount;
        //if health is less then zero then switch to the Death state
        if(health<=0)
        {
            SwitchState(animalDeath);
        }
    }

    public void Die()
    {
        animator.Play("Death");
        //Drop Item
        if(collectable!=null)
        while(maxNumberOfDrop>0)
        {
            Vector3 pos=new Vector3(Random.Range(myTransform.position.x+maxNumberOfDrop,myTransform.position.x-maxNumberOfDrop),myTransform.position.y+2f,Random.Range(myTransform.position.z+maxNumberOfDrop,myTransform.position.z-maxNumberOfDrop));
            if(collectable.TakeChance())Instantiate(collectable,pos,Quaternion.identity);
            maxNumberOfDrop--;
        }

        if(potions!=null)
        {
            for(int i=0;i<potions.Count;i++){
            Vector3 pos=new Vector3(Random.Range(myTransform.position.x+maxNumberOfDrop,myTransform.position.x-maxNumberOfDrop),myTransform.position.y+2f,Random.Range(myTransform.position.z+maxNumberOfDrop,myTransform.position.z-maxNumberOfDrop));
            if(potions[i].TakeChance())Instantiate(potions[i],pos,Quaternion.identity);
            maxNumberOfDrop--;
            }
        }
        //Destroy object 
        Destroy(this.gameObject,dieAfter);
        EnemyDiedEvent.Raise(this,ID);

    }




    #endregion
}
