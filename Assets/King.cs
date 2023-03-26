using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class King : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float changeDirectionCooldown = 1f;

    private Vector2 targetDirection;
    private Rigidbody2D rb2d;
    private List<GameObject> Orbs = new List<GameObject>();

    [Header("Attack Parameters")]
    [SerializeField] private float atktime= 3f;
    [SerializeField] private float tiredtime = 3f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private int OrbCount = 12;
    [SerializeField] private float timeTofire = 1f;

    private bool resume = false;
    private float tempTime;
    private float cooldown;
    private float timebwshots;
    private Quaternion rotation;
    private bool isAtk = false;
    // Start is called before the first frame update
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        targetDirection = transform.up;
        cooldown = changeDirectionCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAtk)
        {
            if(tempTime <= 0)
            {
                isAtk = false;
                tempTime = tiredtime;
            }
            else
            {
                if (resume)
                {
                    Attack();
                    tempTime -= Time.deltaTime;
                }
            }
        }

        else
        {
            if (tempTime <= 0)
            {
                isAtk = true;
                tempTime = atktime;
            }
            else
            {
              tempTime -= Time.deltaTime;   
            }
        }

    }

    private void Attack()
    {
        //Movement
        SetDirection();
        SetVelocity();

        //Attack
        int angleOrbs = (int) Mathf.Floor(360 / OrbCount);
        float a = UnityEngine.Random.Range(0, 30);
        if (timebwshots <= 0)
        {
            for (int i = 0; i < OrbCount; i++)
            {
                Instantiate(projectile, transform.position, Quaternion.AngleAxis((float)(i * angleOrbs) + a, transform.forward));
                timebwshots = timeTofire;
            }            
        }
        else
            timebwshots -= Time.deltaTime;
    }

    public void Pause()
    {
        resume = false;
    }
    public void Resume()
    {
        resume = true;
    }

    private void SetVelocity()
    {
        targetDirection = rotation*targetDirection;
        targetDirection.Normalize();

        rb2d.velocity = targetDirection * speed;
    }

    private void SetDirection()
    {
        cooldown -= Time.deltaTime;
        if(cooldown <= 0)
        {
            float angleChange = UnityEngine.Random.Range(-90f, 90f);
            rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            cooldown = changeDirectionCooldown;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            cooldown = 0;
            SetDirection();
            SetVelocity();
        }
    }
}
