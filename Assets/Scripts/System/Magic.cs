using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public static Magic instance;
    [SerializeField] float maxMagic=100f;
    [SerializeField] HealthBar bar;
    [SerializeField] bool IsRegenerative=false;
    [SerializeField] float startRegenerationAfter=6f;
    private float currentMagic=0f;
    private bool HaveUsedMagic=false;

        void Awake()
        {
            instance=this;
        }
        void Start()
        {
            currentMagic=maxMagic;
            bar.SetMaxHealth(maxMagic);
            bar.SetHealth(currentMagic);
            
        }

        public void UseMagic(float amount)
        {
            if(currentMagic<=0) return;
            currentMagic-=amount;
    
            HaveUsedMagic=true;
            bar.SetHealth(currentMagic);
            //if its regenerative then call the fun to regenarate
            if(IsRegenerative)
            {
                //if already a coroutine is going on then stop that. As it can effect our new changes
                StopAllCoroutines();
                //restart the coroutine
                StartCoroutine(Regenarate());
            }
        

        }
    

        //fun to regenerate magic
        IEnumerator Regenarate()
        {
            //wait until the start regenartion timmer
            yield return new WaitForSeconds(startRegenerationAfter);
            HaveUsedMagic=false;
            //if still the Coroutine is not stopped (have not used magic)
            while(currentMagic<maxMagic && !HaveUsedMagic)
            {
                currentMagic+=0.1f;
                bar.SetHealth(currentMagic);
                yield return null;
            }
            //if any how magic amount is gratter then the max possible amount
            if(currentMagic>=maxMagic)
            {
                currentMagic=maxMagic;
                HaveUsedMagic=false;
            }
            
        }

        public void AddMagic(float amount)
        {
            currentMagic+=amount;
            if(currentMagic>=maxMagic)
            {
                currentMagic=maxMagic;
            }
    
            bar.SetHealth(currentMagic);
        }

        public bool AbleToUseMagic(float amount)
        {
            return currentMagic>=amount;
        }
}
