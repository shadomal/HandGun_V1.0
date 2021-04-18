using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    [Header("NavMashComponents")]
    [SerializeField] private NavMeshAgent enemyAgent;
    [SerializeField] private Transform target;

    void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
    }
    public void trackPlayer()
    {
        if (target != null)
        {
            enemyAgent.SetDestination(target.position);
        }
    }

}
