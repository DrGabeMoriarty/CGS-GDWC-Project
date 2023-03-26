using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 1f;

    [Header("Attack Parameters")]
    [SerializeField] private float atkcooldown;
    [SerializeField] private float range;
    [SerializeField] private float distance;
    [SerializeField] private float aggrodist;
    [SerializeField] private int damage;

    //[Header ("Colliders")]
    /*[SerializeField]*/
    private BoxCollider2D box;
    //[SerializeField] private Transform transform;

    [Header("Player")]
    [SerializeField] private LayerMask playerlayer;
    /*[SerializeField]*/ private Transform player;
    private Health playerhealth;

    private float timer = Mathf.Infinity;
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

        timer += Time.deltaTime;

        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //rb2d.rotation = angle;
        float magnitude = direction.magnitude;
        direction.Normalize();

        //Chase
        if (magnitude >= distance && magnitude <= aggrodist)
        {
            rb2d.velocity = (Vector2) direction * speed *10*Time.deltaTime;
        }

        //Attack
        if (PlayerClose())
        {
            if (timer >= atkcooldown)
            {
                timer = 0;
                DamagePlayer();
                anim.SetTrigger("Attack");
            }
        }
    }

    private bool PlayerClose()
    {
        RaycastHit2D hit = Physics2D.BoxCast(box.bounds.center + transform.right * distance * transform.localScale.x,
        new Vector3(box.bounds.size.x + range, box.bounds.size.y, box.bounds.size.z), 0, Vector2.left, 0, playerlayer);

        if (hit.collider != null)
        {
            playerhealth = hit.transform.GetComponent<Health>();
        }

        return (hit.collider != null);
    }

    private void DamagePlayer()
    {
        if (PlayerClose())
        {
            playerhealth.TakeDamage(damage);
        }
    }

}