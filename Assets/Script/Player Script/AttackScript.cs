using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public float damage = 2f;
    public float radius = 1f;
    public LayerMask layerMask;

    // Update is called once per frame
    void Update()
    {
       Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);
       if(hits.Length > 0) {

           //hit the damage
           hits[0].gameObject.GetComponent<HealthScript>().ApplyDamage(damage);
           print("Hit damage to  : " + hits[0].gameObject.tag);
           gameObject.SetActive(false);
       } 
    }
}
