using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    public GameObject powerupIndicatorPrefab;
    public Vector3 offset;
    public bool hasPowerup = false;
    public float powerupStrength = 15;

    private GameObject powerupIndicator;

    private void OnDestroy()
    {
        Destroy(powerupIndicator);
    }

    void Start()
    {
        powerupIndicator = Instantiate(powerupIndicatorPrefab);
        powerupIndicator.SetActive(hasPowerup);
    }

    void Update()
    {
        if (hasPowerup)
        {
            powerupIndicator.transform.position = transform.position + offset;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasPowerup) return;

        if (other.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            StartCoroutine(Powerup());
        }
    }

    IEnumerator Powerup()
    {
        hasPowerup = true;
        powerupIndicator.SetActive(true);
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasPowerup && collision.gameObject.CompareTag("Enemy"))
        {
            var enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            var direction = (collision.gameObject.transform.position - transform.position);
            enemyRb.AddForce(direction * powerupStrength, ForceMode.Impulse);
        }
    }
}
