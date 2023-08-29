using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] float sensX,sensY;
    [SerializeField] Transform myTransform,orientation;

    private float xRotation,yRotation;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX=Input.GetAxisRaw("Mouse X")*Time.deltaTime*sensX;
        float mouseY=Input.GetAxisRaw("Mouse Y")*Time.deltaTime*sensY;

        yRotation+=mouseX;
        xRotation-=mouseY;

        xRotation=Mathf.Clamp(xRotation,-90f,90f);

        myTransform.rotation=Quaternion.Euler(xRotation,yRotation,0);
        orientation.rotation=Quaternion.Euler(0,yRotation,0);

    }
}
