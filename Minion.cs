using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinionState
{
    chase,
    stagger
}

public class Minion : Enemy
{
    public float speed;

    public Transform target;

    public Rigidbody2D rb;

    public Animator animator;

    public MinionState state;

    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        state = MinionState.chase;
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector2.zero;
    }

    private void FixedUpdate()
    {
        chase();
        Animate();
    }

    void chase()
    {
        state = MinionState.chase;
        movement = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        animator.SetBool("Moving", true);
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
            if (state != MinionState.stagger)
            {
                Rigidbody2D arrow = collision.GetComponent<Rigidbody2D>();
                state = MinionState.stagger;
                health -= 1;
                if(health <= 0)
                {
                    Destroy(gameObject);
                }
                Vector2 direction = arrow.transform.position - transform.position;
                rb.AddForce(-direction * 15f, ForceMode2D.Impulse);
                StartCoroutine(takeDamageCo(arrow));
            }
        }
    }

    IEnumerator takeDamageCo(Rigidbody2D arrow)
    {
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector2.zero;
        state = MinionState.chase;
    }
}
