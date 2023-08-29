using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Portal : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] ClampedFloatParameter distortaionIntensity=new ClampedFloatParameter(0f,-1f,1);
    [SerializeField] Transform portalExitPoint;

    private LensDistortion lens;


   

    //when player come closer to portal
    void OnTriggerEnter(Collider info)
    {
        if(info.CompareTag("Player"))
        {
            volume.profile.TryGet(out lens);
            //lens.intensity.value=distortaionIntensity.value;
           StartCoroutine( MakeDistortion(lens));

        }
    }
    //when player goes out of the portal zone
    void OnTriggerExit(Collider info)
    {
        if(info.CompareTag("Player"))
        {
            StopCoroutine(MakeDistortion(lens));
            
            volume.profile.TryGet(out lens);
            lens.intensity.value=0f;
            
        }
    }

    //when collided with player send the player 
    void OnCollisionStay(Collision info)
    {
        Debug.Log("collition");
        if(info.gameObject.CompareTag("Player"))
        {
            info.gameObject.GetComponent<FPSPlayerController>().SetPlayerPosition(portalExitPoint.position);
        }
    }

    IEnumerator MakeDistortion(LensDistortion val)
    {
        while(val.intensity.value<=distortaionIntensity.value)
        {
            val.intensity.value+=0.01f;
            yield return null;
        }
    }


}
