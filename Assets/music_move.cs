using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music_move : MonoBehaviour
{
    [SerializeField] private float speed;
    public float life = 1f;
    public float damage = 10f;

    private Rigidbody2D rb2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Invoke("DestroyProjectile", life);
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = new Vector2(speed, 0);
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {

            if (collision.CompareTag("Player"))
            {
                Debug.Log("hit");
                collision.GetComponentInParent<Health>().TakeDamage(damage);
            }
            DestroyProjectile();
        }
    }
}
