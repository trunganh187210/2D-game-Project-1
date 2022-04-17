using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackSword : MonoBehaviour
{
    public float thrust;

    private Vector2 difference;

    public int damage;


    public void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            Enemy enemy2 = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                //enemy.isKinematic = false;
                difference = enemy.transform.position - transform.position;
                difference = difference.normalized * thrust;
                enemy.AddForce(difference, ForceMode2D.Impulse);
                //StartCoroutine(KnockBackCo(enemy));

                //enemy2.takeDamage(damage);

                Destroy(gameObject);
            }
        }
        else if(other.gameObject.CompareTag("Boss"))
        {
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            Boss boss = other.gameObject.GetComponent<Boss>();
            if (enemy != null)
            {
                //enemy.isKinematic = false;
                difference = enemy.transform.position - transform.position;
                difference = difference.normalized * thrust;
                enemy.AddForce(difference, ForceMode2D.Impulse);
                //StartCoroutine(KnockBackCo(enemy));
                boss.takeDamage(damage);
                Destroy(gameObject);
            }
        }

    }

    /*IEnumerator KnockBackCo(Rigidbody2D enemy)
    {
        if (enemy != null)
        {
            yield return new WaitForSeconds(knockBackTime);
            enemy.velocity = Vector2.zero;
            enemy.isKinematic = true;
        }
    }*/
}
