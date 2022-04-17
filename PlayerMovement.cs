using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    walk,
    shootArrow,
    stagger
}

public class PlayerMovement : Player
{
    public PlayerState state;

    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    public Animator animator;

    Vector2 movement;

    public bool isShooting;

    public float HorizontalDir, VerticalDir;

    public Transform FirePoint;

    public GameObject bulletPrefab;

    public float arrowForce = 20f;

    Vector3 shootUp = new Vector3(0, 0, 0);

    Vector3 shootDown = new Vector3(0, 0, 180);

    Vector3 shootLeft = new Vector3(0, 0, 90);

    Vector3 shootRight = new Vector3(0, 0, -90);

    public VectorValue startingPos;

    void Start()
    {
        loadPlayer();
        state = PlayerState.walk;
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", -1);
        transform.position = startingPos.initialValue;
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector2.zero;
        ProcessInputs();
        RotateArrow(); //huong ten ban theo huong di chuyen nguoi choi
    }

    void FixedUpdate()
    {
        if (isShooting && state != PlayerState.shootArrow && state != PlayerState.stagger)
        {
            StartCoroutine(ShootingCo());
        }else if (state == PlayerState.walk)
        {
            Animate();
        }
    }

    private IEnumerator ShootingCo()
    {
        animator.SetBool("Shoot", true);
        state = PlayerState.shootArrow;
        Shoot();
        yield return null;
        animator.SetBool("Shoot", false);
        yield return new WaitForSeconds(0.4f);
        state = PlayerState.walk;
    }

    void ProcessInputs()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        isShooting = Input.GetButton("Fire1"); //nút space
    }

    void Animate()
    {
        if(movement != Vector2.zero)
        {
            Move();
            HorizontalDir = movement.x;
            VerticalDir = movement.y;
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void RotateArrow()
    {
        if (VerticalDir > 0.5)
        {
            FirePoint.eulerAngles = shootUp;
        }
        else if (VerticalDir < -0.5)
        {
            FirePoint.eulerAngles = shootDown;
        }
        else if (VerticalDir == 0)
        {
            if(HorizontalDir > 0.5)
            {
                FirePoint.eulerAngles = shootRight;
            }else if(HorizontalDir < -0.5)
            {
                FirePoint.eulerAngles= shootLeft;
            }
        }
    }
    void Move()
    {
        movement.Normalize(); // đi chéo sẽ ko bị nhanh hơn dọc và ngang
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        GameObject arrow = Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(FirePoint.up * arrowForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (state != PlayerState.stagger)
            {
                Rigidbody2D enemy = collision.GetComponent<Rigidbody2D>();
                state = PlayerState.stagger;
                Vector2 direction = enemy.transform.position - transform.position;
                rb.AddForce(-direction * 15f, ForceMode2D.Impulse);
                takeDamge();
                StartCoroutine(PlayerKnockCo());
            }
        }
        else if (collision.CompareTag("Boss"))
        {
            if (state != PlayerState.stagger)
            {
                Rigidbody2D boss = collision.GetComponent<Rigidbody2D>();
                state = PlayerState.stagger;
                Vector2 direction = boss.transform.position - transform.position;
                rb.AddForce(-direction * 15f, ForceMode2D.Impulse);
                health -= 2;
                if (health <= 0)
                {
                    SceneManager.LoadScene(0);
                }
                StartCoroutine(PlayerKnockCo());
            }
        }
        else if (collision.CompareTag("Entrance"))
        {
            savePlayer();
        }
        else if (collision.CompareTag("FireProj"))
        {
            state = PlayerState.stagger;
            takeDamge();
            StartCoroutine(PlayerKnockCo());
        }

    }

    private IEnumerator PlayerKnockCo()
    {
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector2.zero;
        state = PlayerState.walk;
    }

    void takeDamge()
    {
        health -= 1;
        if (health <= 0)
        {
            SceneManager.LoadScene(0);
        }
        StartCoroutine(PlayerKnockCo());
    }
}
