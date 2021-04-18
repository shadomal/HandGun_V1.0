using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCore : enemyConfig
{
    void Update()
    {
        FindPlayer();
        EnemyShoting();
    }
}
