using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Executioner : MonoBehaviour
{
    [Header("Attack")]

    [SerializeField] private float atkTime = 5f;
    [SerializeField] private float tiredtime = 5f;

    [Header ("Player")]
    [SerializeField] private LayerMask playerlayer;
    [SerializeField] private float range = 1f;
    [SerializeField] private float damage = 30f;
    [SerializeField] private float distance = 1f;
    private Transform player;
    private Health playerhealth;

    private Animator anim;
    private BoxCollider2D box;
    private Rigidbody2D rb2d;

    private bool isInv = true;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        //rb2d = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if (isInv)
        {
            GetComponent<Health>().Inv();
            StartCoroutine(Atk());
            Attack();
        }
        else
        {
            StartCoroutine(Tired());
            Tire();
        }

    }

    private IEnumerator Atk()
    {
        isInv = true;
        yield return new WaitForSeconds(atkTime);
        isInv = false;
    }
    private IEnumerator Tired()
    {
        isInv = false;
        yield return new WaitForSeconds(tiredtime);
        isInv = true;
    }

    private void Tire()
    {
        anim.SetBool("Attack",false);
        anim.SetBool("Tired", true);
    }
    private void Attack()
    {

        anim.SetBool("Attack", true);
        anim.SetBool("Tired", false);

        if (PlayerClose())
        {
            DamagePlayer();
            Debug.Log("gere");
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
