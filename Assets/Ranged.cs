using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 1f;

    [Header("Attack Parameters")]
    [SerializeField] private float firerate;
    [SerializeField] private float stopDistance;
    [SerializeField] private float runDistance;
    [SerializeField] private float startDistance;
    [SerializeField] private int damage;
    [SerializeField] private Transform shotpoint;
    [SerializeField] private GameObject projectile;

    private float timetoFire;
    private BoxCollider2D box;
    

    [Header("Player")]
    [SerializeField] private LayerMask playerlayer;
    private Transform player;

    private Animator anim;
    private Rigidbody2D rb2d;


    private void Awake()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //rb2d.rotation = angle;
        float magnitude = direction.magnitude;
        direction.Normalize();

        //Chase
        if (magnitude >= stopDistance && magnitude <= startDistance)
        {
            rb2d.velocity = (Vector2)direction * speed * 10 * Time.deltaTime;
            anim.SetBool("isRun",true);
        }
        else if(magnitude < runDistance){
            rb2d.velocity = (Vector2)direction * speed * -10 * Time.deltaTime;
            anim.SetBool("isRun", true);
        }
        else
        {
            rb2d.velocity = Vector2.zero;
            anim.SetBool("isRun", false);
        }

        //Attack
        if (magnitude < stopDistance)
        {
            anim.SetTrigger("Attack");
            Vector3 difference = player.transform.position - transform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            if (timetoFire <= 0)
            {
                    Instantiate(projectile, shotpoint.position, Quaternion.Euler(0f, 0f, rotZ - 90f));
                    timetoFire = firerate;
            }
            else
            {
                timetoFire -= Time.deltaTime;
            }
        }
    }
}
