using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    public GameObject powerupIndicatorPrefab;
    public GameObject powerupPushPrefab;
    public GameObject powerupRocketsPrefab;

    public Vector3 offset;
    public Vector3 offsetOverhead;

    public float powerupStrength = 15;

    private GameObject powerupIndicator;
    private GameObject powerupOverheadIndicator;
    private PowerupType powerupType;

    private void OnDestroy()
    {
        Destroy(powerupIndicator);
        Destroy(powerupOverheadIndicator);
    }

    void Start()
    {
        powerupIndicator = Instantiate(powerupIndicatorPrefab);
        powerupIndicator.SetActive(false);
        powerupType = PowerupType.None;
    }

    void Update()
    {
        if (powerupType != PowerupType.None)
        {
            powerupIndicator.transform.position = transform.position + offset;
            powerupOverheadIndicator.transform.position = transform.position + offsetOverhead;
        }
        if (powerupType == PowerupType.Rockets)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                RemovePowerup();
            }
        }
    }
    private void RemovePowerup()
    {
        powerupType = PowerupType.None;
        powerupIndicator.SetActive(false);
        Destroy(powerupOverheadIndicator);
    }

    private void AddPowerup(Powerup powerup)
    {
        powerupType = powerup.type;
        powerupIndicator.SetActive(true);
        switch (powerup.type)
        {
            case PowerupType.Rockets:
                powerupOverheadIndicator = Instantiate(powerupRocketsPrefab);
                break;
            case PowerupType.Pushback:
                powerupOverheadIndicator = Instantiate(powerupPushPrefab);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (powerupType != PowerupType.None) return;

        if (other.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            var powerup = other.gameObject.GetComponent<Powerup>();
            AddPowerup(powerup);
            StartCoroutine(PowerupCooldown(powerup));
        }
    }
 
    IEnumerator PowerupCooldown(Powerup powerup)
    {
        if (powerup.hasCooldown)
        {
            yield return new WaitForSeconds(powerup.cooldownDuration);
            RemovePowerup();
        }
        else yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((powerupType == PowerupType.Pushback) && collision.gameObject.CompareTag("Enemy"))
        {
            var enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            var direction = (collision.gameObject.transform.position - transform.position);
            enemyRb.AddForce(direction * powerupStrength, ForceMode.Impulse);
        }
    }
}

