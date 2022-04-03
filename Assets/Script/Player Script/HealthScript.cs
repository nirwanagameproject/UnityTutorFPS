using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthScript : MonoBehaviour
{
    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;
    private EnemyController enemy_Controller;

    public float health = 100f;
    public bool is_Player, is_Boar, is_Cannibal;

    private bool is_Dead;
    
    void Awake()
    {
        if( is_Boar || is_Cannibal) {
            enemy_Anim = GetComponent<EnemyAnimator>();
            enemy_Controller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();
        }

        if(is_Player){

        }
    }

    public void ApplyDamage(float damage) {
        if(is_Dead){
            return;
        }

        health -= damage;

        if(is_Player){
            //show health bar ui
        }

        if(is_Boar || is_Cannibal){
            if(enemy_Controller.Enemy_State == EnemyState.PATROL){
                enemy_Controller.chase_Distance = 50f;
            }
        }
    }
}
