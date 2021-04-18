using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    public Transform Braco;
    public float BracoSpeed;
    float BracoAngle;

    public Transform Bracin;
    public float BracinSpeed;
    float BracinAngle;

    public GameObject test;


    // Update is called once per frame
    void Update()
    {
        //RotateArm();
        RotateArminho();
    }
    // CIMA -> BAIXO
    /*void RotateArm()
    {
        BracoAngle += Input.GetAxis("Mouse X") * BracoSpeed * -Time.deltaTime;
        BracoAngle = Mathf.Clamp(BracoAngle, -90, 90);
        Braco.localRotation = Quaternion.AngleAxis(BracoAngle, Vector3.up);
        //Debug.Log(BracoAngle + "Lado");
        
    }*/

    //DIREITA -> ESQUEDA
    void RotateArminho()
    {
        BracinAngle += Input.GetAxis("Mouse Y") * BracinSpeed * -Time.deltaTime;
        BracinAngle = Mathf.Clamp(BracinAngle, 180, 360);
        Bracin.localRotation = Quaternion.AngleAxis(BracinAngle, Vector3.right);
        Debug.Log(BracinAngle + "Lado");
    }
}
