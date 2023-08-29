using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]  float sensetivity;
    [SerializeField] Transform playerBody;
    float xRotation=0f;
    [SerializeField] KeyCode cursorLockButton=KeyCode.Mouse2;
    private bool IsCursoreLocked=false;
    [SerializeField] private  bool IsActiveCheat=false;
    void Start()
    {
        Cursor.lockState=CursorLockMode.Locked;
        sensetivity= PlayerPrefs.GetFloat("mouse",100f);
        
    }

    void Update()
    {   
        if(Input.GetKey(cursorLockButton))
        {
            if(IsCursoreLocked) {Cursor.lockState=CursorLockMode.None; IsCursoreLocked=false;}
            else {Cursor.lockState=CursorLockMode.Locked;IsCursoreLocked=true;}
        }
        //exit panel
        if(Input.GetKey(KeyCode.Escape))
        {
            UIManager.instance.ShowExitPanel();
            Cursor.lockState=CursorLockMode.None;
            IsCursoreLocked=false;
        }
        
        //Cheat
        if(Input.GetKeyDown(KeyCode.Backspace) && IsActiveCheat)
        {
            Memory.instance.AddMemory(20);
        }

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float mouseX=Input.GetAxis("Mouse X")*sensetivity*Time.deltaTime;
        float mouseY=Input.GetAxis("Mouse Y")*sensetivity*2/3*Time.deltaTime;

        xRotation-=mouseY;
        xRotation=Mathf.Clamp(xRotation, -90f,90f);
        transform.localRotation=Quaternion.Euler(xRotation,0f,0f);

        playerBody.Rotate(Vector3.up*mouseX);

    }

    //listen to event
    public void ActiveCheatEvent(Component sender,object data)
    {
        if(data is bool)
        {
            IsActiveCheat=(bool)data;
        }
    }

   
}
