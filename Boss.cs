using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health;

    public string bossName;

    public GameObject minion;

    public void takeDamage(int dmgTake)
    {
        health -= dmgTake;
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
