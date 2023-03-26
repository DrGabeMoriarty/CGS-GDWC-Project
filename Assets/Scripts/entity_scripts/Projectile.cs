using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1f;
    public float life = 1f;
    public float distance = 1f;
    public float damage = 1f;
    public LayerMask enemylayer;

    private void Start()
    {
        Invoke("DestroyProjectile", life);
    }
    void Update()
    {

        //Movement
        transform.Translate(Vector2.up * speed * Time.deltaTime);


        //Damage
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.localPosition, transform.up, distance,enemylayer);
        

        if (hitinfo.collider != null){

            if (hitinfo.collider.CompareTag("Enemy"))
            {
                hitinfo.collider.GetComponentInParent<Health>().TakeDamage(damage);
                
            }
            DestroyProjectile();
        }

    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
