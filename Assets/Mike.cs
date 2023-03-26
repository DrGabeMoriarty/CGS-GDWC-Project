using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mike : MonoBehaviour
{

    [SerializeField] private float speed = 1.0f;
    private Animator anim;
    void Start()
    {
        //anim = GetComponent<Animator>();
    }


    void Update()
    {
        int y;
        //Movement 
        if (Input.GetKey(KeyCode.I))
            y = 1;
        else if (Input.GetKey(KeyCode.K))
            y = -1;
        else
            y = 0;

        transform.localPosition += new Vector3(0, y * speed * Time.deltaTime, 0);

        //Animation
        /*if (y < 0)
        {
            anim.SetBool("walkleft", false);
            anim.SetBool("walkright", false);
            anim.SetBool("walkup", false);
            anim.SetBool("walkdown", true);
            anim.SetBool("idle", false);
        }
        else if (y > 0)
        {
            anim.SetBool("walkleft", false);
            anim.SetBool("walkright", false);
            anim.SetBool("walkup", true);
            anim.SetBool("walkdown", false);
            anim.SetBool("idle", false);
        }
        else
        {
            anim.SetBool("walkleft", false);
            anim.SetBool("walkright", false);
            anim.SetBool("walkup", false);
            anim.SetBool("walkdown", false);
            anim.SetBool("idle", true);
        }*/
    }
}
