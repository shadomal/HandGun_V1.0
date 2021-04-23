using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class AchaPlayer : MonoBehaviour
{
    public CinemachineFreeLook cam;
    public CinemachineVirtualCamera camVirtual;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        find();
        Debug.Log("CHAMANDO FIND");
    }
    public void find()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            Debug.Log("PLAYER ENCONTRADOR PELA CAMERA");
            camVirtual.m_Priority = 100;
 
            cam.Follow = player.transform;
            cam.LookAt = player.transform;
        }
        Debug.Log("Procurando player.....");

    }
}
