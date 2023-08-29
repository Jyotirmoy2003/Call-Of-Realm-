using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float destroyAfter=7f;
    [SerializeField] float explosionRange=5f,force=5f;
    [SerializeField] GameObject explodeParticel;
    [HideInInspector]
    public float damage=5f;
    private Transform mytransform;
    private Rigidbody rbOfNearbyObjects;


    void Start()
    {
        mytransform=this.transform;
        //destroy this game object after a particuler time
      Destroy(this.gameObject,destroyAfter);
    }

    
    void OnCollisionEnter(Collision info)
    {
        //if collided with some damageable object then damage that
        // if(info.gameObject.TryGetComponent<IDamageable>(out var val))
        //     {
        //         val.TakeDamage(damage);
        //     }
        Explode();
       Destroy(this.gameObject);
        
        
    }

    void OnTriggerEnter(Collider info)
    {
        //if collided with some damageable object then damage that
    if(info.CompareTag("Area"))
      {
            return;
      }
        Explode();

       Destroy(this.gameObject);
        
        
    }


   


    void Explode()
    {
        //show particels
        //Get All the objects which are in range of explosion
        Collider[] colliders=Physics.OverlapSphere(mytransform.position,explosionRange);

        foreach(Collider nearbyObject in colliders)
        {
            //if has rigidbody then add push forche
            if(nearbyObject.TryGetComponent<Rigidbody>(out rbOfNearbyObjects))
            {
                rbOfNearbyObjects.AddExplosionForce(force,mytransform.position,explosionRange);
            }
            //if it is damagebale then damage it
            if(nearbyObject.TryGetComponent<IDamageable>(out var val))
            {
                val.TakeDamage(damage);
            }
            
        }

        Instantiate(explodeParticel,mytransform.position,Quaternion.identity);
    }
}
