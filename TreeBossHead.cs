using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TreeBossHeadState
{
    sleep,
    normal,
    enraged,
}

public class TreeBossHead : Boss
{
    public TreeBossHeadState state;

    public float lookRadius = 10f;

    public Transform target;

    public Rigidbody2D rb;

    public GameObject projectile;

    public float TimeBetweenShot;

    public float nextShotTime;

    public float TimeBetweenShotEnraged;

    Vector2 secondndShotDir;

    public Transform[] minionSpawn;

    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = this.GetComponent<Rigidbody2D>();
        state = TreeBossHeadState.sleep;
        secondndShotDir.x = transform.position.x - 1;
        secondndShotDir.y = transform.position.y;
    }

    /*private void Update()
    {
        
    }*/

    // Update is called once per frame
    void FixedUpdate()
    { 
        if(Vector2.Distance(transform.position, target.position) < lookRadius && state != TreeBossHeadState.enraged)
        {
            state = TreeBossHeadState.normal;
        }

        if (health <= 10)
        {
            state = TreeBossHeadState.enraged;
            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + TimeBetweenShotEnraged;
                Instantiate(projectile, transform.position, Quaternion.identity);
                Instantiate(projectile, secondndShotDir, Quaternion.identity);
            }
        }

        if (state == TreeBossHeadState.enraged)
        {
            if (i < minionSpawn.Length)
            {
                Instantiate(minion, minionSpawn[i].position, Quaternion.identity);
                i++;
            }
        }
        else if(state != TreeBossHeadState.sleep) 
        {
            state = TreeBossHeadState.normal;
            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + TimeBetweenShot;
                Instantiate(projectile, transform.position, Quaternion.identity);
            }
        }
    }
}
