using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    #region Vars
    [Header("Vida e Estamina")]
    private static int stamina;
    [SerializeField] private int maxStamina;
    private static int life_;
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
    private float forceMult;

    public float turnSpeed;
    Camera mainCamera;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        //life_ = GetComponent<Unit>().health;
        //maxLife_ = GetComponent<Unit>().maxHealth;
        life_ = 150;
        this.maxLife_ = 250;
        this.speed = 20;
        this.rigidPlayer = GetComponent<Rigidbody>();
        stamina = 200;
        this.maxStamina = 350;
        this.StaminaCost = 0.8f;
        this.SightAnimation = GetComponent<Animator>();
        this.animator = GetComponent<Animator>();
        this.playerDown = false;
        this.turnSpeed = 10f;
        this.forceMult = 30;

        this.mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        if (Input.GetKeyDown(KeyCode.X))
        {
            RemoveHealth(10);
        }

        Anim();
    }
    private void FixedUpdate()
    {
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
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

        //rigidPlayer.velocity = transform.forward * moveV * speed;
        //transform.Rotate(0, moveH * speed, 0);

        rigidPlayer.velocity = transform.forward * moveV * forceMult + transform.right * moveH * forceMult;

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

    public bool isExhausted() => stamina <= 0;

    public int GetStamina() => stamina;
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

        if (stamina > this.maxStamina)
        {
            stamina = this.maxStamina;
        }

        if (stamina <= 0)
        {
            stamina = 0;
        }

        //UpdateStaminaBar();
    }

    /* public void AddStamina(int stamina)
    {
        this.SetStamina(this.GetStamina() + stamina);
    }*/
    public void AddStamina(int staminaIncrement) => this.SetStamina(stamina + staminaIncrement);
    public void RemoveStamina(int stamina)
    {
        this.SetStamina(this.GetStamina() - stamina);
    }

    public void UpdateStaminaBar()
    {
        this.StaminaBar.fillAmount = ((1.6f / ((float)this.maxStamina)) * ((float)stamina));
    }
    #endregion
    #region VIDA
    public int GetLife() => life_;
    public void SetLife(int increment)
    {
        if (increment >= life_)
        {
            life_ = maxLife_;
        }
        else
        {
            life_ = increment;
        }
        UpdateLifeBar();
        if (life_ <= 0)
        {
            DeathController();
            DisableSight();
        }
    }
    public void AddHealth(int lifeIncrement) => this.SetLife(life_ + lifeIncrement);
    public void RemoveHealth(int lifeReduced) => this.SetLife(life_ - lifeReduced);
    public void UpdateLifeBar() => this.LifeBar.fillAmount = ((1.6f / this.maxLife_) * life_);
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
}