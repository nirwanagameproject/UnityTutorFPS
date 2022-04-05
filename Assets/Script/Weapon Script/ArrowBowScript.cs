using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBowScript : MonoBehaviour
{
    private Rigidbody myBody;

    public float speed = 30f;
    public float deactivate_timer = 3f;
    public float damage = 50f;

    void Awake() 
    {
        myBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeactivateGameObject",deactivate_timer);
    }

    public void launch(Camera mainCamera)
    {
        myBody.velocity = mainCamera.transform.forward * speed; //Camera.main.transform.forward * speed;
        transform.LookAt(transform.position + myBody.velocity);
    }

    void DeactivateGameObject()
    {
        if(gameObject.activeInHierarchy){
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider target){
        //after hit target will deactivate
        if(target.tag == Tags.ENEMY_TAG) {
            target.GetComponent<HealthScript>().ApplyDamage(damage);

            gameObject.SetActive(false); 
        }
    }
}
