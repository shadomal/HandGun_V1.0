using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    #region Vars
    [Header("Vida e Estamina")]
    [SerializeField] private int stamina;
    [SerializeField] private int maxStamina;
    [SerializeField] private int life_;
    [SerializeField] private int maxLife_;
    [Header("Animações")]
    [SerializeField] private Animator SightAnimation;
    private Animator animator;

    [Header("Cameras")]
    [SerializeField] private GameObject PlayerCam;
    [SerializeField] private GameObject SightMachineCam;


    [Header("Imagens")]
    [SerializeField] private Image LifeBar;
    [SerializeField] private Image StaminaBar;
    [Header("Player Stats")]
    [SerializeField] private float jumpForce = 300;
    [SerializeField] private int speed;
    [SerializeField] private Rigidbody rigidPlayer;
    [SerializeField] private float StaminaCost;
    public bool playerDown;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        //life_ = GetComponent<Unit>().health;
        //maxLife_ = GetComponent<Unit>().maxHealth;
        life_ = 150;
        maxLife_ = 250;
        speed = 20;
        rigidPlayer = GetComponent<Rigidbody>();
        stamina = 1000;
        maxStamina = 10000;
        StaminaCost = 0.8f;
        SightAnimation = GetComponent<Animator>();
        animator = GetComponent<Animator>();
        playerDown = false;




        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame

    #region Métodos         
    #region OnCollisionEnter

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            RemoveHealth(10);
        }
        if (other.gameObject.tag == "enemyShoot")
        {
            RemoveHealth(30);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemyShoot")
        {
            RemoveHealth(30);
        }
    }
    #endregion
    #region Move
    public void move(float moveH, float moveV)
    {
        if (moveV == 0 && moveH == 0)
        {
            return;
        }

        if (this.isExhausted())
        {
            if ((moveV != 0 && moveH == 0) || moveV != 0)
            {
                //this.speed = 0;
                animator.SetFloat("Blend X", 0);
                animator.SetFloat("Blend Y", 0);
                return;
            }
        }

        rigidPlayer.velocity = transform.forward * moveV * speed;
        transform.Rotate(0, moveH * speed, 0);


        animator.SetFloat("Blend X", moveH);
        animator.SetFloat("Blend Y", moveV);

        if (moveV != 0 && moveH == 0)
        {
            this.RemoveStamina(10);
        }
    }
    /*public void move(float moveH, float moveV)
    {
        Vector3 novaVel = Vector3.up * moveH * speed;
        if (moveV == 0 && moveH == 0)
        {
            return;
        }
        if (this.isExhausted())
        {
            if ((moveV != 0 && moveH == 0) || moveV != 0)
            {
                this.speed = 5;
                rigidPlayer.velocity = transform.forward * moveV * speed;
                novaVel.y = rigidPlayer.velocity.y;
                transform.Rotate(0, moveH * moveH, 0); 
                return;
            }
        }
        rigidPlayer.velocity = transform.forward * moveV * speed;
        novaVel.y = rigidPlayer.velocity.y;
        transform.Rotate(0, moveH * speed, 0);
        animator.SetFloat("Blend X", moveH);
        animator.SetFloat("Blend Y", moveV);
        if (moveV != 0 && moveH == 0)
        {
            this.RemoveStamina(10);
        }
    }*/
    #endregion
    public void Anim()
    {
        if (animator == null) return;
    }
    #region Stamina

    public bool isExhausted() => this.stamina <= 0;

    public int GetStamina() => this.stamina;
    public void SetStamina(int stamina)
    {
        if (IsGamePaused())
        {
            return;
        }
        if (!CooldownManager.IsExpired("shadomal", "stamina"))
        {
            return;
        }
        CooldownManager.AddCooldown("shadomal", "stamina", 250);

        this.stamina = stamina;

        if (this.stamina > this.maxStamina)
        {
            this.stamina = this.maxStamina;
        }

        if (this.stamina <= 0)
        {
            this.stamina = 0;
        }

        //UpdateStaminaBar();
    }

    public void AddStamina(int stamina)
    {
        this.SetStamina(this.GetStamina() + stamina);
    }
    public void RemoveStamina(int stamina)
    {
        this.SetStamina(this.GetStamina() - stamina);
    }


    public void UpdateStaminaBar()
    {
        this.StaminaBar.fillAmount = ((1.6f / ((float)this.maxStamina)) * ((float)this.stamina));
    }
    #endregion
    #region VIDA
    public int GetLife() => this.life_;
    public void SetLife(int LifePlayer)
    {
        if (LifePlayer >= life_)
        {
            this.life_ = maxLife_;
        }
        else
        {
            this.life_ = LifePlayer;
        }
        UpdateLifeBar();
        if (life_ <= 0)
        {
            DeathController();
            DisableSight();
        }
    }
    public void AddHealth(int life) => this.SetLife(this.life_ + life);
    public void RemoveHealth(int life) => this.SetLife(this.life_ - life);
    public void UpdateLifeBar() => this.LifeBar.fillAmount = ((1.6f / this.maxLife_) * this.life_);
    public void DeathController()
    {
        Destroy(gameObject);
        playerDown = true;
    }
    #endregion
    #endregion
    #region Mira
    bool playerAim = false;
    public void SightActive()
    {

        if (playerAim == false)
        {
            playerAim = true;
            PlayerCam.gameObject.SetActive(false);
            SightMachineCam.gameObject.SetActive(true);
        }
        else
        {
            playerAim = false;
            PlayerCam.gameObject.SetActive(true);
            SightMachineCam.gameObject.SetActive(false);
        }

    }
    #endregion
    [SerializeField] private GameObject cam;
    public void DisableSight()
    {
        cam.SetActive(false);
    }

    private bool IsPause = false;

    public bool IsGamePaused()
    {
        return Time.timeScale == 0;
    }

    public float turnSpeed = 15f;
    Camera mainCamera;
    private void FixedUpdate()
    {
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }
    void Update()
    {
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");
        move(moveH, moveV);

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SightActive();
        }
        Anim();
    }
}