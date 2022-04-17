using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootingEnemyState
{
    idle,
    retreat,
    shoot,
    stagger,
}

public class ShootingEnemyController : Enemy
{
    public float lookRadius = 10f;

    public float speed;

    public Transform target;

    public Rigidbody2D rb;

    public Animator animator;

    public float mininmumDistance;

    public ShootingEnemyState state;

    public GameObject projectile;

    public float TimeBetweenShot;

    public float nextShotTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        state = ShootingEnemyState.idle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, target.position) <= lookRadius && Vector2.Distance(transform.position, target.position) > mininmumDistance && state != ShootingEnemyState.stagger)
        {
            shoot();
        }
        else if(Vector2.Distance(transform.position, target.position) <= mininmumDistance)
        {
            retreat();
        }
    }

    public void shoot()
    {
        state = ShootingEnemyState.shoot;
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + TimeBetweenShot;
            Instantiate(projectile, transform.position, Quaternion.identity);
        }
    }

    public void retreat()
    {
        state = ShootingEnemyState.retreat;
        transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow"))
        {
            if (state != ShootingEnemyState.stagger)
            {
                Rigidbody2D arrow = collision.GetComponent<Rigidbody2D>();
                state = ShootingEnemyState.stagger;
                Vector2 direction = arrow.transform.position - transform.position;
                health -= 1;
                if (health <= 0)
                {
                    StartCoroutine(dieCo());
                }
                rb.AddForce(-direction * 15f, ForceMode2D.Impulse);
                StartCoroutine(takeDamageCo());
            }
        }
    }

    IEnumerator dieCo()
    {
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
    }

    IEnumerator takeDamageCo()
    {
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector2.zero;
        state = ShootingEnemyState.idle;
    }
}
