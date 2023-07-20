using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    public float speed = 10;
    public float strength = 10;
    public float aliveTimer = 1.5f;

    private Transform target;
    private Vector3 direction;

    void Update()
    {
        if (target != null)
        {
            direction = (target.transform.position - transform.position).normalized;
            transform.LookAt(target);
            transform.Rotate(90, 0, 0);
        }
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Fire(Transform newTarget)
    {
        target = newTarget;
        Destroy(gameObject, aliveTimer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (target != null)
        {
            if(collision.gameObject.CompareTag(target.tag))
            {
                var rb = collision.gameObject.GetComponent<Rigidbody>();
                var away = -collision.contacts[0].normal;
                rb.AddForce(away * strength, ForceMode.Impulse);
                Destroy(gameObject);
            }
        }
    }
}
