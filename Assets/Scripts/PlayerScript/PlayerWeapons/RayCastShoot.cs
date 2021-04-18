using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastShoot : MonoBehaviour
{
    [Header("Weapon Atributs")]
    [SerializeField] private float fireHate;
    [SerializeField] private float hitforce;
    [SerializeField] private Transform canoArma;
    private int ammo; //Munição arma
    private int PenteReserva; //Pente reserva 
    [SerializeField] private GameObject bullet;
    private float timeToShoot;

    [Header("Controladores mira e som")]
    private LineRenderer laserLine;
    [SerializeField]private AudioSource gunSFX;


    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        gunSFX = GetComponent<AudioSource>();

        ammo = 10;
        PenteReserva = 5;
        timeToShoot = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (ammo > 0)
            {
                if (timeToShoot > 0.2)
                {
                    ShootEffect();
                    laserLine.SetPosition(0, canoArma.transform.position);
                    
                    ammo--;
                    Instantiate(bullet, canoArma.position, canoArma.rotation);
                    timeToShoot = 0;
                }
            }
        }
        timeToShoot += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (ammo == 0 && PenteReserva > 0)
            {
                PenteReserva -= 1;
                ammo = 10;
            }
        }
    }
    public void ShootEffect()
    {
        gunSFX.Play();
        if (!CooldownManager.IsExpired("Pistola", "SFX"))
        {
            return;
        }
        CooldownManager.AddCooldown("Shadomal", "SFX", 500);

        laserLine.enabled = true;

    }
}
