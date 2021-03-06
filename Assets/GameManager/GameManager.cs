using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.AI;

public class GameManager : SpawnPlayer
{

    [Header("Turno Vars")]
    public int turnos;
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
    public static bool isTurn = false;

    [Header("JOGADORES - ")]
    private GameObject player, enemy;
    private GameObject playerClone, enemyClone;

    [Header("DICE CONTROLLER")]
    public static int dice_1, dice_2;
    [SerializeField] public static bool buttonReady_1, buttonReady_2;
    public Button Button_Right, Button_Left;
    public Text uiButtonText;
    public Text uiButtonText_two;
    public Text TimeToStart_txt;
    public Text enemyDice, playerDice;
    public GameObject UI_INICIALIZING_GAME;
    public GameObject UI_InGame;
    public GameObject CAM_INICIALIZE;
    public float timeToStart;
    public bool startCount;
    [Header("GameVars")]
    public int playerPontuacao, enemyPontuacao;
    private bool IsPlayerDead, IsEnemyDead;

    void Awake()
    {
        this.turnos = 0;
        this.turnoTimer = 60f; //segundos
        this.isRunning = false;

        buttonReady_1 = false;
        buttonReady_2 = false;
        timeToStart = 5;
        SetStateGame(STATE_GAME.INITIALIZING);
        Button_Right.onClick = new Button.ButtonClickedEvent();
        Button_Right.onClick.AddListener(() =>
        {
            buttonReady_1 = !buttonReady_1;
            uiButtonText_two.text = buttonReady_1 ? "READY" : "UNREADY";
            CheckCanStart();
        });
        Button_Left.onClick = new Button.ButtonClickedEvent();
        Button_Left.onClick.AddListener(() =>
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
                Debug.Log("Iniciando...");
            }
        }
    }
    public void RollingDices()
    {
        dice_1 = Random.Range(0, 20);
        dice_2 = Random.Range(0, 20);
        Debug.Log("Rolling Dices this result is: " + " PLAYER 1:" + dice_1 + " | " + " ENEMY:" + dice_2);
        if (dice_1 == dice_2)
        {
            RollingDices();
            Count();
            Debug.Log("Rolling new dices this result is: " + " PLAYER 1:" + dice_1 + " | " + " ENEMY:" + dice_2);
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
                    Debug.Log("LOADING PLAYERS.... - MENSAGE LINE: 120");
                    LoadingPlayers();
                    inGame = true;
                    return;
                }
            }
            return;
        }
    }
    public void LoadingPlayers()
    {
        UI_INICIALIZING_GAME.SetActive(false);
        UI_InGame.SetActive(true);
        CAM_INICIALIZE.SetActive(false);
        if (GetStateGame() == STATE_GAME.START_TURN)
        {
            Debug.Log("STATE GAME: " + GetStateGame() + " - LINE MENSAGE - 136");
            SetupSpawn();
            playerClone = GameObject.FindGameObjectWithTag("Player");
            enemyClone = GameObject.FindGameObjectWithTag("Enemy");

            isRunning = true;

            Debug.Log("Insert nick names... check Dice Results - LINE MENSAGE - 143");
            userName_txt.text = "lost";
            enemyName_txt.text = "pedrin";

            if (dice_1 > dice_2)
            {
                Debug.Log("PLAYER WIN - RESULT IS: " + dice_1);
                SetStateGame(STATE_GAME.PLAYER_TURN);
                PlayerTurn();
            }
            else if (dice_1 < dice_2)
            {
                Debug.Log("BOT WIN - RESULT IS: " + dice_2);
                SetStateGame(STATE_GAME.ENEMY_TURN);
                EnemyTurn();
            }
        }
    }
    static int playerHealth;
    static int playerStamina;
    public void RefreshStats()
    {
        if (inGame)
        {
            PlayerController states = playerClone.GetComponent<PlayerController>();
            playerHealth = states.GetLife();
            playerStamina = states.GetStamina();
            Debug.Log("Player Health: " + playerHealth + " | " + "Player stamina: " + playerStamina);
        }
    }
    public GameObject endTurn;
    public void CountTime()
    {
        if (isRunning || turnoTimer == 60)
        {
            turnoTimer -= Time.deltaTime;
            turnos_txt.text = turnoTimer.ToString("0");
            if (turnoTimer <= 30)
            {

                if (turnoTimer <= 0)
                {
                    isRunning = false;
                    turnos += 1;
                    turnosCount.text = turnos.ToString();
                    Debug.Log("TURNOS QUE SE PASSARAM: " + turnos);
                    SearchPlayers();
                    CheckPlayers();
                }
            }
        }
    }

    public void PlayerTurn()
    {
        if (this.GetStateGame() == STATE_GAME.PLAYER_TURN)
        {
            playerClone.GetComponent<PlayerController>().enabled = true;
            //playerCam.SetActive(true);
        }
    }
    public void EnemyTurn()
    {
        enemyClone.GetComponent<NavMeshAgent>().enabled = true;
        if (this.GetStateGame() == STATE_GAME.ENEMY_TURN)
        {
            IsEnemyTurn();
        }
    }
    public bool IsEnemyTurn()
    {
        return isTurn = true;
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
    //Butão para a troca de turno do Player para o inimigo.
    public void ResetGame()
    {
        if (GetStateGame() == STATE_GAME.REBOOTING)
        {
            Debug.Log("Function on... plis reset game");
        }
    }
    public void CheckPlayers()
    {
        if (player == null)
        {
            SetStateGame(STATE_GAME.WIN);
            this.enemyPoints_txt.text += 1.ToString();
            turnos += 1;
            Destroy(this.enemy);
            this.turnoTimer = 60;
            SetStateGame(STATE_GAME.REBOOTING);
            ResetGame();
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
                    player.GetComponent<PlayerController>().enabled = false;
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
    public bool inGame = false;
    public void SearchPlayers()
    {
        if (inGame == true && Time.timeScale == 1)
        {
            inGame = false;
            SetStateGame(STATE_GAME.REBOOTING);
            Debug.Log("Saindo de jogo... Resetando a cena... - STATUS ATUAL DO MANAGER - " + GetStateGame());
            if (playerClone == null)
            {
                IsPlayerDead = true;
                Debug.Log("Player Not found... REBOOTING SCENE...... - MESSAGE LINE - 315" + " " + GetStateGame());
                RebotGame();
            }
            else
            {
                IsEnemyDead = true;
                Debug.Log("Enemy Not found... REBOOTING SCENE...... - MESSAGE LINE - 321" + " " + GetStateGame());
                RebotGame();
            }
        }
        return;
    }
    bool rebotEnd;
    public void RebotGame()
    {
        Destroy(playerClone);
        Destroy(enemyClone);
        if (GetStateGame() == STATE_GAME.REBOOTING)
        {
            SetupSpawn();
            playerClone = GameObject.FindGameObjectWithTag("Player");
            enemyClone = GameObject.FindGameObjectWithTag("Enemy");
            //RELOAD PLAYER ATRIBUTS
            PlayerController status = playerClone.GetComponent<PlayerController>();
            status.SetLife(250);
            status.SetStamina(200);

            enemyConfig enemyStatus = enemyClone.GetComponent<enemyConfig>();
            enemyStatus.SetLife(250);
            enemyStatus.SetStamina(200);

            //RESET COUNT TIME;
            turnoTimer = 60;

            dice_1 = Random.Range(0, 20);
            dice_2 = Random.Range(0, 20);
            Debug.Log("Rolling Dices this result is: " + " PLAYER 1:" + dice_1 + " | " + " ENEMY:" + dice_2);
            if (dice_1 == dice_2)
            {
                RollingDices();
                Debug.Log("Rolling new dices this result is: " + " PLAYER 1:" + dice_1 + " | " + " ENEMY:" + dice_2);
                return;
            }
            else
            {
                if (dice_1 > dice_2)
                {
                    Debug.Log("PLAYER WIN - RESULT IS: " + dice_1);
                    SetStateGame(STATE_GAME.PLAYER_TURN);
                    PlayerTurn();
                }
                else if (dice_1 < dice_2)
                {
                    Debug.Log("BOT WIN - RESULT IS: " + dice_2);
                    SetStateGame(STATE_GAME.ENEMY_TURN);
                    EnemyTurn();
                }
            }

            /*Debug.Log("Reboot... Estado atual de jogo: " + GetStateGame() + "MENSAGE LINE: 343");
            SetStateGame(STATE_GAME.START_TURN);
            UpdateStatus();
            LoadingPlayers();
            */

        }
    }

    public void UpdateGameInfo()
    {
        if (IsPlayerDead)
        {
            turnos += 1;
            this.enemyPoints_txt.text += 1.ToString();
            turnosCount.text = turnos.ToString();
        }
    }
    void Update()
    {
        Count();
        CountTime();
        SearchPlayers();
        RefreshStats();

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
