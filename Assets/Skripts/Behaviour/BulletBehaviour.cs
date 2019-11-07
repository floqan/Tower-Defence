using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed;
    public int damage;
    private Transform target;
    public GameObject effect;

    public void setTarget(Transform _target)
    {
        target = _target;
    }
   
    // Update is called once per frame
    void Update()
    {
     if(target == null)
        {
            Destroy(gameObject);
            return;
        }   
     else
        {
            
            Vector3 dir = target.position - transform.position;
            float distance = Time.deltaTime * speed;
            if(dir.magnitude <= distance)
            {
                Destroy(Instantiate(effect, transform.position, transform.rotation), 2.0f);
                target.GetComponent<EnemyBehaviour>().decreaseHealth(damage);
                Destroy(gameObject);
            }

            transform.Translate(dir.normalized * distance, Space.World);
            transform.LookAt(target); 
        }
    }
}
