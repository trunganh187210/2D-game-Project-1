using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.takeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
    /*public void Knock(Rigidbody2D enemy, float knockBackTime)
    {
        StartCoroutine(KnockBackCo(enemy, knockBackTime));
    }

    IEnumerator KnockBackCo(Rigidbody2D enemy, float knockBackTime)
    {
        if (enemy != null)
        {
            yield return new WaitForSeconds(knockBackTime);
            enemy.velocity = Vector2.zero;
            enemy.isKinematic = true;
        }
    }*/
}
