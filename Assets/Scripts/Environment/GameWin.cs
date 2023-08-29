using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWin : MonoBehaviour
{
    [SerializeField] int backIndex=0;

    void Start()
    {
        Time.timeScale=1;
        Cursor.lockState=CursorLockMode.None;
    }
    public void OnClickBack()
    {
        LevelLoder.instance.LoadLevel(backIndex);
    }
}
