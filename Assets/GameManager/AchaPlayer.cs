using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AchaPlayer : MonoBehaviour
{
    public CinemachineFreeLook cam;
    public CinemachineVirtualCamera camVirtual;

    private GameObject player;
    public bool stopSearch;
    // Start is called before the first frame update
    void Aweke()
    {
        stopSearch = false;
    }

    // Update is called once per frame
    void Update()
    {
        find();
    }
    public void find()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (stopSearch == false)
        {
            
            Transform obj = player.transform.GetChild(1);
            stopSearch = true;
            Debug.Log("PLAYER ENCONTRADOR PELA CAMERA");
            camVirtual.m_Priority = 100;
            cam.Follow = player.transform;
            cam.LookAt = obj.transform;

        }
    }
    public void camSet()
    {
        Transform obj = player.transform.GetChild(1);
        stopSearch = false;
        Debug.Log("PLAYER ENCONTRADOR PELA CAMERA");
        camVirtual.m_Priority = 100;
        cam.Follow = player.transform;
        cam.LookAt = obj.transform;
    }
}
