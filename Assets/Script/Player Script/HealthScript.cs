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

    private EnemyAudio enemyAudio;
    
    void Awake()
    {
        if( is_Boar || is_Cannibal) {
            enemy_Anim = GetComponent<EnemyAnimator>();
            enemy_Controller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            //sound
            enemyAudio = GetComponentInChildren<EnemyAudio>();
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

        if(health <= 0f){
            PlayerDied();
            is_Dead = true;
        }
    }

    void PlayerDied() {
        if(is_Cannibal){
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 50f);

            enemy_Controller.enabled = false;
            navAgent.enabled = false;
            enemy_Anim.enabled = false;

            //coroutine
            StartCoroutine(DeadSound());
            //spawn more enemies
        }

        if(is_Boar){
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemy_Controller.enabled = false;

            enemy_Anim.Dead();

            //coroutine
            StartCoroutine(DeadSound());
            //spawn more enemies
        }

        if(is_Player){
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);
            for(int i = 0; i < enemies.Length; i++){
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }

            //stop spawn enemies

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
        }

        if(tag == Tags.PLAYER_TAG){
            //restart 3 second
            Invoke("RestartGame", 3f);
        } else {
            Invoke("TurnOffGameObject", 3f);
        }
    }

    void RestartGame(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    void TurnOffGameObject(){
        gameObject.SetActive(false);
    }

    IEnumerator DeadSound(){
        yield return new WaitForSeconds(0.3f);
        enemyAudio.Play_DeadSound();
    }
}
