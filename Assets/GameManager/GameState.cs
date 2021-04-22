using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public enum STATE_GAME
    {
        INITIALIZING, //Carregar todas as infos;
        START_TURN, //Inicia primeiro turno
        PLAYER_TURN, // TURNO PLAYER
        ENEMY_TURN, // TURNO INIMIGO
        LOSE, //DERROTA DO INIMIGO
        STOP_TURN, // Para o turno
        REBOOTING, // ATUALIZAR A CENA(CONTADORES - PONTUAÇÃO, TURNOS RESTANTES - TEMPO, STAMINA E VIDA) 
        WIN, // Inicializa tela de vitória ingame
    }
}