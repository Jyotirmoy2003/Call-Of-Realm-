 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialugeTrigger : MonoBehaviour
{
    public Dialouge dialouge;

    public void Trigger()
    {
        DialougeManager.instance.StartDialouge(dialouge);
    }

}
