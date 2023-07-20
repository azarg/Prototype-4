using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    private Rigidbody enemyRb;
    private GameObject player;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(direction * speed);
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
