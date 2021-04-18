using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
public class GameManager : ScriptBattleSystemTeste
{

    public enum STATE_GAME
    {
        INITIALIZING, //Carregar todas as infos;
        START_TURN, //Inicia primeiro turno
        PLAYER_TURN, // TURNO PLAYER
        ENEMY_TURN, // TURNO INIMIGO
        STOP_TURN, // Para o turno
        REBOOTING, // ATUALIZAR A CENA(CONTADORES - PONTUAÇÃO, TURNOS RESTANTES - TEMPO, STAMINA E VIDA) 
        WIN,
        LOSE
    }

    [Header("Jogadores Vars")]

    [Header("Turno Vars")]
    public int turnos;
    //public float turnoTimer;
    public static bool stop;
    public static bool turnoEnd;
    public static bool isPlayerAction, isEnemyAction;
    public float turnoTimer = 0;
    public bool isRunning = true;
    [Header("STATUS_ACTUALLY")]
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
    void Awake()
    {
        this.turnos = 0;
        this.turnoTimer = 60f; //segundos
        stateGame = STATE_GAME.INITIALIZING;
        SetupBattle();
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
        playerLife = player.gameObject.GetComponent<PlayerController>().GetLife();
        playerStamina = player.gameObject.GetComponent<PlayerController>().GetStamina();
        //Inimigo
        enemyName_txt.text = enemy.gameObject.GetComponent<Unit>().userName;
        enemyLife = enemy.gameObject.GetComponent<enemyConfig>().GetLife();
        enemyStamina = enemy.gameObject.GetComponent<enemyConfig>().GetStamina();

        //Mudança de estado
        SetStateGame(STATE_GAME.START_TURN);
        InitTurn();
    }
    public STATE_GAME GetStateGame() => this.stateGame;
    public void SetStateGame(STATE_GAME state) => this.stateGame = state;
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
    void Update()
    {
        CountTime();
        if (Input.GetKey(KeyCode.K))
        {
            SkipTurn();
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
        playerPontuacao = Convert.ToInt32(playerPoints_txt);
        enemyPontuacao = Convert.ToInt32(enemyPoints_txt);
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
    public void SkipTurn()
    {
        this.turnoTimer -= 20;
    }
    public void TryAgain()
    {
        SceneManager.LoadScene("MapaBlocagem");
    }
    public void CloseGame()
    {
        Application.Quit();
    }
}
