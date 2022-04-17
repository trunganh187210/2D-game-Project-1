using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TreeBossArmState
{
    chase,
    enraged,
    stop,
    retreat,
    stagger
}
public class TreeBossArm : Boss
{
    public TreeBossArmState state;

    public float lookRadius = 10f;

    public float speed;

    public Transform target;

    public Rigidbody2D rb;

    Vector2 startingPos;

    public GameObject projectile;

    public float TimeBetweenShot;

    public float nextShotTime;

    Vector2 shootingPos;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        state = TreeBossArmState.stop;
        rb = this.GetComponent<Rigidbody2D>();
        startingPos = rb.position;
    }

    private void Update()
    {
        if(health < 7)
        {
            state = TreeBossArmState.enraged;
            shootingPos.x = transform.position.x;
            shootingPos.y = transform.position.y - 3;
            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + TimeBetweenShot;
                Instantiate(projectile, shootingPos, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector2.Distance(transform.position, target.position) <= lookRadius && Vector2.Distance(transform.position, startingPos) <= 1.5f)
        {
            chase();
        }
        else if(state != TreeBossArmState.enraged)
        {
            retreat();
        }
    }

    public void chase()
    {
        state = TreeBossArmState.chase;
        Vector2 lookDir = target.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90f;
        rb.rotation = angle;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void retreat()
    {
        state = TreeBossArmState.stop;
        StartCoroutine(retreatCo());
        /*state = TreeBossArmState.retreat;
        transform.position = Vector2.MoveTowards(transform.position, startingPos, speed * Time.deltaTime);*/
    }

    IEnumerator retreatCo()
    {
        yield return new WaitForSeconds(0.2f);
        state = TreeBossArmState.retreat;
        transform.position = Vector2.MoveTowards(transform.position, startingPos, speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow"))
        {
            if (state != TreeBossArmState.stagger)
            {
                Rigidbody2D arrow = collision.GetComponent<Rigidbody2D>();
                state = TreeBossArmState.stagger;
                /*Vector2 direction = arrow.transform.position - transform.position;
                rb.AddForce(-direction * 15f, ForceMode2D.Impulse);*/
                StartCoroutine(takeDamageCo(arrow));
            }
        }
    }

    IEnumerator takeDamageCo(Rigidbody2D arrow)
    {
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector2.zero;
        state = TreeBossArmState.stop;
    }
}
