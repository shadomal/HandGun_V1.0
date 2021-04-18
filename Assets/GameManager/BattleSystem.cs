using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
/*
public enum GAMESTATE
{
    START,
    CHANGE_STATE,
    PLAYER_TURN,
    ENEMY_TURN,
    WIN,
    LOSE,
    RESET,
    UPDATE_SCORE,
}

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GAMESTATE state;
    public static bool isPlayerAction, isEnemyAction;

    void Start()
    {
        state = GAMESTATE.START;
        SetupSpawn();
        ChangeState();
        isPlayerAction = false;
        isEnemyAction = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    #region STATE - START
    public void SetupSpawn()
    {
        Instantiate(playerPrefab, GetRandomLocation(), new Quaternion(0, 0, 0, 0));
        Instantiate(enemyPrefab, GetRandomLocation(), new Quaternion(0, 0, 0, 0));

    }

    public Vector3 GetRandomLocation()
    {
        int _x = 473;
        int _y = 25;
        int _z = 510;

        int raio = 320;

        int x = Random.Range(-raio, +raio);
        int z = Random.Range(-raio, +raio);

        Vector3 vector = new Vector3(_x + x, _y, _z + z);

        if (!IsSafeLocation(vector))
        {
            return GetRandomLocation();
        }

        //vector.y = getSafeY(vector);

        return vector;
    }
    /*public int getSafeY(Vector3 location)
    {
        return false;
    }
    public bool IsSafeLocation(Vector3 location)
    {
        List<GameObject> nearby = GetNearbyObjects(location, 10);

        if (nearby.ToArray().Length == 0)
        {
            return true;
        }

        return false;
    }

    public List<GameObject> GetNearbyObjects(Vector3 location, int radius)
    {
        List<GameObject> objects = new List<GameObject>();

        GameObject[] cenario = GameObject.FindGameObjectsWithTag("Cenario");
        for (int i = 0; i < cenario.Length; i++)
        {
            objects.Add(cenario[i]);
        }

        objects.Add(GameObject.FindGameObjectWithTag("Player"));
        objects.Add(GameObject.FindGameObjectWithTag("Enemy"));

        List<GameObject> nearby = new List<GameObject>();

        for (int i = 0; i < objects.ToArray().Length; i++)
        {
            GameObject gameObject = objects.ToArray()[i];

            if (gameObject == null || gameObject.transform == null || gameObject.transform.position == null)
            {
                continue;
            }

            int x = (int)gameObject.transform.position.x;
            int y = (int)gameObject.transform.position.y;
            int z = (int)gameObject.transform.position.z;

            int _x = (int)location.x;
            int _y = (int)location.y;
            int _z = (int)location.z;

            if (((x - radius) > _x && (x + radius) < _x) && ((y - radius) > _y && (y + radius) < _y) && ((z - radius) > _z && (z + radius) < _z))
            {
                nearby.Add(gameObject);
            }
        }

        return nearby;
    }
    #region STATE_GAME CHANGE_STATE
    public void ChangeState()
    {

        state = GAMESTATE.CHANGE_STATE;
        Debug.Log("ESTADO DE JOGO ATUAL: INICIALIZANDO - " + this.state);

        if (playerPrefab != null && enemyPrefab != null)
        {
            playerPrefab.GetComponent<PlayerController>().GetStamina();
            playerPrefab.GetComponent<PlayerController>().GetLife();
            enemyPrefab.GetComponent<enemyConfig>().GetLife();
            enemyPrefab.GetComponent<enemyConfig>().GetStamina();

            if (!CooldownManager.IsExpired("CHANGE_TURN", "TURN_CD"))
            {
                return;
            }
            CooldownManager.AddCooldown("CHANGE_TURN", "TURN_CD", 5000);
            state = GAMESTATE.PLAYER_TURN;
            PlayerAction();
            Debug.Log("TROCANDO PARA AÇÕES DO JOGADOR: " + this.state);
        }
    }
    #region STATE_GAME - PLAYER_TURN
    public void PlayerAction()
    {

        enemyPrefab.GetComponent<EnemyCore>().enabled = false;
        enemyPrefab.GetComponent<NavMeshAgent>().enabled = false;

        if (playerPrefab.GetComponent<PlayerController>().isExhausted() == false)
        {
            playerPrefab.GetComponent<PlayerController>().enabled = false;
            state = GAMESTATE.ENEMY_TURN;
            Debug.Log("TROCANDO DE TURNO + " + this.state + " AÇÃO DO INIMIGO");
            isPlayerAction = true;
        }

    }
    public void EnemyAction()
    {
        enemyPrefab.GetComponent<EnemyCore>().enabled = true;
        enemyPrefab.GetComponent<NavMeshAgent>().enabled = true;

        if (enemyPrefab.GetComponent<EnemyCore>().isEnemyExhausted())
        {
            enemyPrefab.GetComponent<EnemyCore>().enabled = false;
            enemyPrefab.GetComponent<NavMeshAgent>().enabled = false;
            isEnemyAction = true;
        }
        Debug.Log("TURNOS FINALIZADOS - VERIFICANDO SE TODOS OS JOGADORES ESTÃO VIVOS" + "RESETANDO TURNOS");
        VerifyAction();
    }
    public void VerifyAction()
    {
        if (isEnemyAction == true && isPlayerAction == true)
        {
            ChangeState();
            isEnemyAction = false;
            isPlayerAction = false;
        }
    }

    #endregion
    #endregion
    #endregion
}
   */
