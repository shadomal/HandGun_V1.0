﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
public class GameManager : SpawnPlayer
{

    [Header("Turno Vars")]
    public int turnos;
    //public float turnoTimer;
    public static bool stop;
    public static bool turnoEnd;
    public static bool isPlayerAction, isEnemyAction;
    public float turnoTimer = 0;
    public bool isRunning = true;
    private STATE_GAME stateGame;
    [Header("INTERFACE ATRIBUTS")]
    public Text playerPoints_txt, turnos_txt, enemyPoints_txt;
    public Text userName_txt, enemyName_txt;
    public Text turnosCount;
    public Image win_img;
    public Text winner_txt;
    public GameObject winUI;
    [Header("PLAYER ATRIBUTS")]
    private int playerLife, playerMaxLife, playerStamina;

    [Header("ENEMY ATRIBUTS")]
    private int enemyLife, enemyMaxLife, enemyStamina;
    public static bool isTurn = false;

    [Header("JOGADORES - ")]
    private GameObject player, enemy;

    [Header("DICE CONTROLLER")]
    public static int dice_1, dice_2;
    public static bool buttonReady_1, buttonReady_2;
    public Button Button_one, Button_two;
    public Text uiButtonText;
    public Text uiButtonText_two;
    public Text TimeToStart_txt;
    public Text enemyDice, playerDice;
    public GameObject UI_INICIALIZING_GAME;
    public GameObject UI_InGame;
    public GameObject CAM_INICIALIZE;
    public float timeToStart;
    public bool startCount;
    void Awake()
    {
        this.turnos = 0;
        this.turnoTimer = 60f; //segundos


        buttonReady_1 = false;
        buttonReady_2 = false;
        timeToStart = 5;
        SetStateGame(STATE_GAME.INITIALIZING);
        Button_one.onClick = new Button.ButtonClickedEvent();
        Button_one.onClick.AddListener(() =>
        {
            buttonReady_1 = !buttonReady_1;
            uiButtonText_two.text = buttonReady_1 ? "READY" : "UNREADY";
            CheckCanStart();
        });
        Button_two.onClick = new Button.ButtonClickedEvent();
        Button_two.onClick.AddListener(() =>
        {
            buttonReady_2 = !buttonReady_2;
            uiButtonText.text = buttonReady_2 ? "READY" : "UNREADY";
            CheckCanStart();
        });
    }
    public void CheckCanStart()
    {
        if (GetStateGame() == STATE_GAME.INITIALIZING)
        {
            if (buttonReady_1 == true && buttonReady_2 == true)
            {
                RollingDices();
            }
        }
    }
    public void RollingDices()
    {
        dice_1 = Random.Range(0, 20);
        dice_2 = Random.Range(0, 20);
        Debug.Log("Rolling Dices this result is: " + dice_1 + " | " + dice_2);
        if (dice_1 == dice_2)
        {
            RollingDices();
            Debug.Log("Rolling new dices this result is: " + dice_1 + " | " + dice_2);
            return;
        }
        playerDice.text = System.Convert.ToString(dice_1);
        enemyDice.text = System.Convert.ToString(dice_2);
        startCount = true;
    }

    public void Count()
    {
        if (startCount)
        {
            timeToStart -= Time.deltaTime;
            TimeToStart_txt.text = timeToStart.ToString("0");
            if (timeToStart <= 0)
            {
                startCount = false;
                SetStateGame(STATE_GAME.START_TURN);
                if (GetStateGame() == STATE_GAME.START_TURN)
                {
                    UI_INICIALIZING_GAME.SetActive(false);
                    UI_InGame.SetActive(true);
                    CAM_INICIALIZE.SetActive(false);
                    SetupBattle();
                    return;
                }
            }
            return;
        }
    }
    public void SetupBattle()
    {
        SetupSpawn();
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        SetPlayer(player, enemy);
    }
    public void SetPlayer(GameObject player, GameObject enemy)
    {
        //Jogador
        userName_txt.text = "Lost";
        //Inimigo
        enemyName_txt.text = enemy.gameObject.GetComponent<Unit>().userName;

        //Mudança de estado
        SetStateGame(STATE_GAME.START_TURN);
        InitTurn();
    }
    public void InitTurn()
    {
        Debug.Log("STATE -  " + stateGame);
        if (this.GetStateGame() == STATE_GAME.START_TURN)
        {
            SetStateGame(STATE_GAME.PLAYER_TURN);
            PlayerTurn();
        }
    }
    public void CountTime()
    {
        if (isRunning || turnoTimer == 60)
        {
            turnoTimer -= Time.deltaTime;
            turnos_txt.text = turnoTimer.ToString("0");
            if (turnoTimer <= 0)
            {
                isRunning = false;
                turnos += 1;
                turnosCount.text = turnos.ToString();
                Debug.Log("TURNOS QUE SE PASSARAM: " + turnos);
                CheckPlayers();
            }
        }
    }

    public void PlayerTurn()
    {
        if (this.GetStateGame() == STATE_GAME.PLAYER_TURN)
        {
            PlayerController playerController = player.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = true;
            }
        }
    }
    public void ChangePlayer(STATE_GAME state)
    {
        SetStateGame(state);
        turnoTimer = 60;
        isRunning = true;
        if (GetStateGame() == STATE_GAME.PLAYER_TURN && isPlayerAction == true)
        {
            PlayerTurn();
        }
        else
        {
            EnemyTurn();
        }
    }
    public void EnemyTurn()
    {
        enemy.GetComponent<NavMeshAgent>().enabled = true;
        if (this.GetStateGame() == STATE_GAME.ENEMY_TURN)
        {
            IsEnemyTurn();
        }
    }
    public bool IsEnemyTurn()
    {
        return isTurn = true;
    }
    //Butão para a troca de turno do Player para o inimigo.
    public void CheckPlayers()
    {
        if (player == null)
        {
            SetStateGame(STATE_GAME.WIN);
            this.enemyPoints_txt.text += 1.ToString();
            turnos += 1;
            Destroy(this.enemy);
            this.turnoTimer = 60;
            SetupBattle();
        }
        else if (enemy == null)
        {
            SetStateGame(STATE_GAME.LOSE);
            this.playerPoints_txt.text += 1.ToString();
            turnos += 1;
            this.turnoTimer = 60;
            Destroy(this.player);
            WinCondition();
            return;
            //SetupBattle();
        }
        else
        {
            if (turnos <= 5)
            {
                if (GetStateGame() == STATE_GAME.PLAYER_TURN)
                {
                    isPlayerAction = false;
                    player.GetComponent<PlayerControl>().enabled = false;
                    turnos += 1;
                    turnosCount.text = turnos.ToString();
                    ChangePlayer(STATE_GAME.ENEMY_TURN);
                    //Game object vazio = cam.SetActive = true;
                    //false;
                    Debug.Log("TURNO INIMIGO");
                }
                else
                {
                    ChangePlayer(STATE_GAME.PLAYER_TURN);
                    turnos += 1;
                    turnosCount.text = turnos.ToString();
                    isPlayerAction = true;
                    Debug.Log("AÇÃO DO JOGADOR");
                }
            }
        }
    }
    public void OnEndTurnBotton()
    {

        if (GetStateGame() == STATE_GAME.ENEMY_TURN)
        {
            return;
        }
        turnoTimer = 0;
        ChangePlayer(STATE_GAME.PLAYER_TURN);
    }
    public int playerPontuacao, enemyPontuacao;

    public void WinCondition()
    {
        playerPontuacao = System.Convert.ToInt32(playerPoints_txt);
        enemyPontuacao = System.Convert.ToInt32(enemyPoints_txt);
        playerPontuacao = 3;
        if (playerPontuacao == 3)
        {
            SetStateGame(STATE_GAME.WIN);
            winUI.SetActive(true);
            winner_txt.text = "VITORIA - " + playerPontuacao + " " + userName_txt;
            win_img.fillAmount += Time.deltaTime;
        }
        else
        {
            SetStateGame(STATE_GAME.WIN);
            winUI.SetActive(true);
            winner_txt.text = "VITORIA - " + enemyPontuacao + " " + enemyName_txt;
            win_img.fillAmount += Time.deltaTime;
        }
    }
    void Update()
    {
        Count();
        CountTime();
        if (Input.GetKey(KeyCode.K))
        {
            SkipTurn();
        }
    }
    public void SkipTurn()
    {
        this.turnoTimer -= 20;
    }
    public void TryAgain()
    {
        SceneManager.LoadScene("MapaBlocagem");
    }
}