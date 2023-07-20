using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PowerupController : MonoBehaviour
{
    public GameObject powerupIndicatorPrefab;
    public GameObject powerupPushPrefab;
    public GameObject powerupRocketsPrefab;
    public GameObject rocketPrefab;

    public Vector3 offset;
    public Vector3 offsetOverhead;

    public float powerupStrength = 15;

    private GameObject powerupIndicator;
    private GameObject powerupOverheadIndicator;
    private PowerupType powerupType;
    private float fireCooldown = 0.5f;
    private float lastFireTime;

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
        lastFireTime = Time.time;
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
                Fire();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (powerupType == PowerupType.Pushback)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                var enemyRb = collision.gameObject.GetComponent<Rigidbody>();
                var direction = (collision.gameObject.transform.position - transform.position);
                enemyRb.AddForce(direction * powerupStrength, ForceMode.Impulse);
            }
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

    private void Fire()
    {
        if (Time.time < lastFireTime + fireCooldown) return;

        lastFireTime = Time.time;
        var e = FindObjectsOfType<EnemyController>();
        if (e != null)
        {
            GameObject closestEnemy = e[0].gameObject;
            float dist = Vector3.Distance(transform.position, closestEnemy.transform.position);
            for (int i = 1; i < e.Length; i++)
            {
                if (Vector3.Distance(transform.position, e[i].transform.position) < dist)
                {
                    closestEnemy = e[i].gameObject;
                    dist = Vector3.Distance(transform.position, closestEnemy.transform.position);
                }
            }
            var rocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            rocket.transform.LookAt(closestEnemy.transform.position);
            rocket.transform.Rotate(90, 0, 0);
            rocket.GetComponent<RocketBehaviour>().Fire(closestEnemy.transform);
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


}

