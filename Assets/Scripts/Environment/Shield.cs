using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shield : MonoBehaviour
{
    [SerializeField] float timmer;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Disolve",timmer);
    }

    

    void Disolve()
    {
        Destroy(this.gameObject);
    }
}
