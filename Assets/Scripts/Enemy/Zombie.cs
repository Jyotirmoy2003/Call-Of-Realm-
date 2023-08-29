using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour,IDamageable,IEnemy
{
    [Header("Zombie data")]
    [SerializeField] int enemyId=0;
    [SerializeField] float speed=5;
    [SerializeField] float damageAmount=1;
    [SerializeField] float damageCoolDown=2;
    [SerializeField] float health=20;
    [SerializeField] GameEvent EventEnemyDied;
    [SerializeField] List<Potion> rewards=new List<Potion>();
    
    private Animator animator;
    private Transform playerTransfrom;
    private Transform myTransform;
    private NavMeshAgent navMeshAgent;
    private IDamageable currentDamageable;
    private bool ableToAttack=true;

    #region  Interface
    public int experiencce { get; set; }
    public int ID{get;set;}

    //when died call event send ID
    public void Die()
    {
        EventEnemyDied.Raise(this,ID);
        int i=1;
        foreach (Potion item in rewards)
        {
            i++;
            Vector3 pos=new Vector3(myTransform.position.x,myTransform.position.y+i,myTransform.position.z);
            if(item.TakeChance()) Instantiate(item.gameObject,pos,myTransform.rotation);
        }
        Destroy(this.gameObject);
    }

    public void PerformAttack()
    {
        ableToAttack=false;
        //show animation
        animator.SetTrigger("Attack");
        //give damage tio the collided object
        currentDamageable.TakeDamage(damageAmount);
        //reste the bool after cool down
        Invoke("ReSetCoolDown",damageCoolDown);
    }
    public void ReSetCoolDown()
    {
        ableToAttack=true;
    }

    public void TakeDamage(float amount)
    {
        health-=amount;
        if(health<=0) Die();
    }
    #endregion

    void Start()
    {
        ID=enemyId;
        animator=GetComponent<Animator>();
        playerTransfrom=GameObject.FindGameObjectWithTag("Player").transform;
        myTransform=GetComponent<Transform>();
        navMeshAgent=GetComponent<NavMeshAgent>();

        navMeshAgent.speed=speed;
    }


   

    void FixedUpdate()
    {
        navMeshAgent.destination=playerTransfrom.position;
        animator.SetFloat("Speed",speed);
    }
    void OnTriggerEnter(Collider info)
    {
        if(info.TryGetComponent<IDamageable>(out currentDamageable)) navMeshAgent.Stop();
    }
     void OnTriggerExit(Collider info)
    {
        if(info.TryGetComponent<IDamageable>(out currentDamageable))navMeshAgent.Resume();
    }

    void OnTriggerStay(Collider info)
    {
        if(info.TryGetComponent<IDamageable>(out currentDamageable))
        {
            if(ableToAttack)PerformAttack();
        }
    }
}
