using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumManager : GameState
{
    public STATE_GAME state; 
    public STATE_GAME GetStateGame() => this.state;
    public void SetStateGame(STATE_GAME actualyState) => this.state = actualyState;
}
