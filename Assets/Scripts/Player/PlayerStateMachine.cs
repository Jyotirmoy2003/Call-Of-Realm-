using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class PLayerBase{
    public abstract void EnterState(PlayerStateMachine ps);
    public abstract void UpdateState(PlayerStateMachine ps);
    public abstract void AbilityState(PlayerStateMachine ps);

}

public class PlayerLightState : PLayerBase
{

    public override void EnterState(PlayerStateMachine ps)
    {
        ps.currentAblitiycost=ps.lightABilityCost;
        UIManager.instance.SetPlayerState(ps.lightColor);
        PlayerMovement.instance.SetPlayerData(ps.lightModeSpeed,ps.lightModeGravity);
        ps.abilityCoolDown=ps.lightAbilityCoolDown;
    }

    public override void UpdateState(PlayerStateMachine ps)
    {
        if(Input.GetKeyDown(ps.stateChangekey))
        {
            ps.SwitchState(ps.playerDarkState);
        }
    }
    public override void AbilityState(PlayerStateMachine ps)
    {
        ps.Shoot();
        AudioManager.instance.PlaySound("Fireball");
    }

    
}

public class PlayerNatureState : PLayerBase
{

    public override void EnterState(PlayerStateMachine ps)
    {
        ps.currentAblitiycost=ps.natureABilityCost;

        UIManager.instance.SetPlayerState(ps.natureColor);
        PlayerMovement.instance.SetPlayerData(ps.natureModeSpeed,ps.natureModeGravity);
        ps.abilityCoolDown=ps.natureAbilityCoolDown;
    }

    public override void UpdateState(PlayerStateMachine ps)
    {
        if(Input.GetKeyDown(ps.stateChangekey))
        {
            ps.SwitchState(ps.playerLightState);
        }
       
    }
    public override void AbilityState(PlayerStateMachine ps)
    {
        ps.SpawnShield();
        AudioManager.instance.PlaySound("Shield");
    }
}

public class PlayerDarkState : PLayerBase
{
    private float teleportAngel;
    private bool IsObstacle=false;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 pos;

     public override void EnterState(PlayerStateMachine ps)
    {
        ps.currentAblitiycost=ps.darkABilityCost;

        UIManager.instance.SetPlayerState(ps.darkColor);
        PlayerMovement.instance.SetPlayerData(ps.darkModeSpeed,ps.darkModeGravity);
        //Chnage Ability cool down
        ps.abilityCoolDown=ps.darkAbilityCoolDown;
    }

    public override void UpdateState(PlayerStateMachine ps)
    {
        
        if(Input.GetKeyDown(ps.stateChangekey))
        {
            ps.SwitchState(ps.playerNatureState);
        }
        
    }
    public override void AbilityState(PlayerStateMachine ps)
    {
        
        
        //cancel out the y position of player looking and fixed it with players current y position
       pos=new Vector3(ps.teleportPosition.position.x,ps.myTransform.position.y,ps.teleportPosition.position.z);
       //if we hit obstacale then teleport just before the obstacle
       if(Physics.Raycast(ps.myTransform.position,ps.cameraHolder.forward,out hit, ps.teleportDistance,ps.obstacleOfTeleporter))
       {
            pos=new Vector3(hit.point.x,ps.myTransform.position.y,hit.point.z);
       }
       //save the postion before teleporting to new position
       if(PlayerMovement.instance.IsGround)
        ps.lastPlayerPosition=ps.myTransform.position;
        
        //change player position
        PlayerMovement.instance.SetPlayerPosition(pos);
        AudioManager.instance.PlaySound("Teleport");
        
    }

    
}

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerLightState playerLightState=new PlayerLightState();
    public PlayerNatureState playerNatureState=new PlayerNatureState();
    public PlayerDarkState playerDarkState=new PlayerDarkState();

    private PLayerBase currentState;

    [Tooltip("set the timmer to check after this time if player is falling from the world")]
    [SerializeField] float checkAfter=5f;
    [SerializeField] float playerAllowedYpos=0;
    
    [HideInInspector]
    public Vector3 lastPlayerPosition;
    public bool isAbilitiyButtonDown=false;
    public Transform myTransform,shootPointTransform,cameraHolder;
    public float abilityCoolDown=4;
    public KeyCode stateChangekey=KeyCode.E;
    public KeyCode abilityKey=KeyCode.Mouse0;

    [Header("Light")]
    
    public float lightModeSpeed;
    public float lightModeGravity;
    public float lightAbilityCoolDown=4;
    public Projectile projectilePrefab;
    public float shootForce=40f;
    public float damage=5;
    public float lightABilityCost=5f;
    public Color lightColor;

    [Header("Nature")]
    public float natureModeSpeed;
    public float natureModeGravity;
    public float natureAbilityCoolDown=4;
    public Shield shield;
    public float natureABilityCost=3f;
    public Color natureColor;

    [Header("Dark")]
    public float darkModeSpeed;
    public float darkModeGravity;
    public float teleportDistance=5;
    public float darkAbilityCoolDown=4;
    public float darkABilityCost=15f;
    public Color darkColor;
    public Transform teleportPosition;
    public LayerMask obstacleOfTeleporter;

    //My Fields
    public float currentAblitiycost=1f;

    private Magic magic;
    


    
    void Start()
    {
        magic=GetComponent<Magic>();
        teleportDistance=Vector3.Distance(myTransform.position,teleportPosition.position);
        currentState=playerNatureState;
        currentState.EnterState(this);
        lastPlayerPosition=new Vector3(myTransform.position.x,myTransform.position.y+2f,myTransform.position.z);
        
        StartCoroutine(PlayerFallhandeler());
    }

   
    void Update()
    {
        currentState.UpdateState(this);

        if(Input.GetKeyDown(abilityKey) && !isAbilitiyButtonDown &&magic.AbleToUseMagic(currentAblitiycost))
        {
            magic.UseMagic(currentAblitiycost);
            isAbilitiyButtonDown=true;
            currentState.AbilityState(this);
            //start cool down
            StartCoroutine(AbilityCoolDown(abilityCoolDown));
        }
        
    }


    //fun to switch between states 
    // can call from different states
    public void SwitchState(PLayerBase nextState)
    {
        currentState=nextState;
        currentState.EnterState(this);
        //sound
        AudioManager.instance.PlaySound("Ability");
    }


    public IEnumerator AbilityCoolDown(float amount)
    {
        yield return new WaitForSeconds(amount);

        isAbilitiyButtonDown=false;
    }

    public void SpawnShield()
    {
        Instantiate(shield,shootPointTransform.position,shootPointTransform.rotation);
    }


    public void Shoot()
    {
       Projectile val= Instantiate(projectilePrefab,shootPointTransform.position,myTransform.rotation);
       val.damage=damage;
       val.GetComponent<Rigidbody>().AddForce(shootPointTransform.forward*shootForce,ForceMode.Impulse);
       CameraShake.instance.Shake();

    }



    //check if player is falling from ground
    IEnumerator PlayerFallhandeler()
    {
        if(myTransform.position.y<playerAllowedYpos)
        {
            PlayerMovement.instance.SetPlayerPosition(lastPlayerPosition);
        }
        
        yield return new WaitForSeconds(checkAfter);

        StartCoroutine(PlayerFallhandeler());
    }
}
