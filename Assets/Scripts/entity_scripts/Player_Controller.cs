using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour{
   
    private bool movingleft = false;
    private bool isDashing = false;
    private bool canDash = true;
    private bool isAtk = false;

    private Animator anim;
    private Rigidbody2D rb2d;
    private TrailRenderer tr;

    [Header ("Movement")]
    public float speed = 2f;
    public float run = 1f;
    
    [Header ("Dashing")]
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCoolDown = 1f;
    
    [Header("Melee")]
    public float atkTime = 1f;
    public float atkdelay = 0.5f;
    public int damage = 20;
    public float atkrange = 0.5f;

    [Header("Ranged")]
    public float atkrate = 1f;
    private float timebwshots;

    [Header("Components")]
    [SerializeField] private Transform atkpoint;
    [SerializeField] private Transform shotpoint;
    [SerializeField] private LayerMask enemylayer;
    [SerializeField] private GameObject projectile;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }

    void Update()
    {

        //disable updates while dashing
        if (isDashing || isAtk) return;


        //Movement 

        float sp = speed;
        int x, y;
        float x_axis = Input.GetAxisRaw("Horizontal");
        float y_axis = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift)) sp = run; //Speed
        else sp = speed;

        if (x_axis > 0.1) x = 1; //To make sure speed is constant
        else if (x_axis < -0.1) x = -1;
        else x = 0;
        if (y_axis > 0.1) y = 1;
        else if (y_axis < -0.1) y = -1;
        else y = 0;

        transform.localPosition += new Vector3(x * sp * Time.deltaTime, y * sp * Time.deltaTime, 0);

        //Dashing
        if (Input.GetKeyDown(KeyCode.Z) && canDash) StartCoroutine(Dash());

        //Attacking
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Atk());
            Attack();
        }

        //Ranged 
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90f);

        if (timebwshots <= 0)
        {
             if (Input.GetMouseButtonDown(0))
             {
                Instantiate(projectile,shotpoint.position,transform.rotation);
                timebwshots = atkrate;
             }
        }
        else
        {
            timebwshots -= Time.deltaTime;
        }

        //Animations
        if ((x != 0) || (y != 0)) anim.SetBool("walk", true);
        else anim.SetBool("walk", false);


        //Flipping
       /* if ((x_axis < 0) && !movingleft)
        {
            movingleft = true;
            GetComponent<Transform>().localScale = new Vector3(-1, 1, 1);
        }
        else if ((x_axis > 0) && movingleft)
        {
            movingleft = false;
            GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        }*/
    }

    private IEnumerator Atk()
    {
        isAtk = true;
        yield return new WaitForSeconds(atkTime);
        isAtk = false;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb2d.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCoolDown);
        canDash = true;
    }

    void Ranged()
    {
        anim.SetTrigger("atk");
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("hit");   
            Instantiate(projectile, shotpoint.position, transform.rotation);
        }
    }

    void Attack()
    {
        

        Collider2D[] hit = Physics2D.OverlapCircleAll(atkpoint.position, atkrange, enemylayer);

        foreach (Collider2D enemy in hit)
        {
            enemy.GetComponent<Health>().TakeDamage(damage);
        }

        anim.SetTrigger("atk");
    }
}