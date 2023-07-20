using UnityEngine;

public enum PowerupType { None, Pushback, Rockets }

public class Powerup : MonoBehaviour
{
    public PowerupType type;
    public bool hasCooldown;
    public int cooldownDuration;
}
