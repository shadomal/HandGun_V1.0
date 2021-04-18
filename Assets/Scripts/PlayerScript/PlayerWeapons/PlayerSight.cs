using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSight : MonoBehaviour
{
    [Header("GameObjects")]
    private GameObject arma;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform canoArma;
    [Header("Valores Pistola")]
    [SerializeField] private int municao;
    [SerializeField] private int pente;
    private float fireHate;
    private float timeToShoot;
    [Header("Audio e Animações")]
    [SerializeField] private AudioSource pistolaSFX;
    [SerializeField] private AudioSource ReloadWeapon;
    void Awake()
    {
        municao = 1000000;
        pente = 3;
        fireHate = 1.5f;
        timeToShoot = 3;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (municao > 0)
            {
                if (timeToShoot > 0.2)
                {
                    municao--;
                    //pistolaSFX.Play();
                    Instantiate(bullet, canoArma.position, canoArma.rotation);
                    timeToShoot = 0;
                }
            }
        }
        timeToShoot += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (municao == 0 && pente > 0)
            {
                pente -= 1;
                municao = 10;
            }
        }
    }

}
