using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    Vector2 targetPos;

    public float speed;

    public float MaxTime;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = FindObjectOfType<PlayerMovement>().transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if (Time.deltaTime > MaxTime)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Arrow"))
        {
            Destroy(gameObject);
        }
    }
}
