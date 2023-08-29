using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMachanics : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private IInteractable currentInteractable ;

    [SerializeField] float distance;
    [SerializeField] KeyCode interactKey =KeyCode.Q;
    

    
   

    // Update is called once per frame
    void Update()
    {
        
        
        #region IIntractable
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       if (Physics.Raycast(ray, out hit, distance))
        {
            if(hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                // hitting an IInteractable -> store
                SetInteractable(interactable);
            }
            else
            {
                // hitting something that is not IInteractable -> reset
                SetInteractable(null);
            }
            
        }else{
            SetInteractable(null);
        }


        // if currently focusing an IInteractable and click -> interact
        if(currentInteractable != null && Input.GetKeyDown(KeyCode.Q))
        {
            currentInteractable.Interact();
        }
        #endregion
    }





    private void SetInteractable(IInteractable interactable)
    {
        // if is same instance (or both null) -> ignore
        if(currentInteractable == interactable) return;

        // otherwise if current focused exists -> reset
        if(currentInteractable != null) currentInteractable.Looking=false;

        // store new focused
        currentInteractable = interactable;

        // if not null -> set looking
        if(currentInteractable != null) 
        {
            currentInteractable.Looking=true;
            UIManager.instance.SetKeySuggest(interactKey.ToString());
        }else{
          
            UIManager.instance.SetKeySuggest("empty");
        
        }
    }

    
}
