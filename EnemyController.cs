using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    patrol,
    chase,
    stagger
}

public class EnemyController : Enemy
{
    public float lookRadius = 10f;

    public float speed;

    public Transform target;

    public Rigidbody2D rb;

    public Animator animator;

    public float mininmumDistance;

    public Transform[] moveSpot;

    private int spot = 0;

    private float waitTime;

    public float startWaitTime;

    Vector2 movement;

    public EnemyState enemyState;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        waitTime = startWaitTime;
        enemyState = EnemyState.patrol;
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector2.zero;
    }
    void FixedUpdate()
    {
        if(Vector2.Distance(transform.position, target.position) <= lookRadius && Vector2.Distance(transform.position, target.position) > mininmumDistance)
        {
            chase();
            Animate();
        }
        else if (Vector2.Distance(transform.position, target.position) > lookRadius)
        {
            patrol();
            Animate();
        }
    }

    void chase()
    {
        enemyState = EnemyState.chase;
        movement = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        animator.SetBool("Moving", true);
    }

    void patrol()
    {
        enemyState = EnemyState.patrol;
        movement = (moveSpot[spot].position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, moveSpot[spot].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpot[spot].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                spot++;
                waitTime = startWaitTime;
                if (spot == moveSpot.Length)
                {
                    spot = 0;
                }
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    void Animate()
    {
        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow"))
        {
            if (enemyState != EnemyState.stagger)
            {
                Rigidbody2D arrow = collision.GetComponent<Rigidbody2D>();
                enemyState = EnemyState.stagger;
                Vector2 direction = arrow.transform.position - transform.position;
                health -= 1;
                if(health <= 0)
                {
                    gameObject.SetActive(false);
                }
                rb.AddForce(-direction * 15f, ForceMode2D.Impulse);
                StartCoroutine(takeDamageCo());
            }
        }
    }

    IEnumerator takeDamageCo()
    {
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector2.zero;
        enemyState = EnemyState.patrol;
    }
}


