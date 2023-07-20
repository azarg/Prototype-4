using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    public GameObject powerupIndicatorPrefab;
    public Vector3 offset;
    public bool hasPowerup = false;
    public float powerupStrength = 15;
    public string enemyTag;

    private GameObject powerupIndicator;

    private void OnDestroy()
    {
        Destroy(powerupIndicator);
    }

    // Start is called before the first frame update
    void Start()
    {
        powerupIndicator = Instantiate(powerupIndicatorPrefab);
        powerupIndicator.SetActive(hasPowerup);
    }

    // Update is called once per frame
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
        if (collision.gameObject.CompareTag(enemyTag))
        {
            var enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            var direction = (collision.gameObject.transform.position - transform.position);
            float force = hasPowerup ? powerupStrength : powerupStrength;
            enemyRb.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}
