using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gleir : MonoBehaviour,IDataPersistence
{
    private Rigidbody2D rb2d;
    private Animator anim;
    [SerializeField] public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       float x = Input.GetAxis("Horizontal");
       float y = Input.GetAxis("Vertical");

        if (x != 0)
            rb2d.velocity = new Vector2(x*speed*Time.deltaTime*10, 0);
        else if (y != 0)
            rb2d.velocity = new Vector2(0, y*speed*Time.deltaTime*10);
        else
            rb2d.velocity = Vector2.zero;

        //Animation
        if (x < 0)
        {
            anim.SetBool("walkleft", true);
            anim.SetBool("walkright", false);
            anim.SetBool("walkup", false);
            anim.SetBool("walkdown", false);
            anim.SetBool("idle", false);
        }
        else if (x > 0)
        {
            anim.SetBool("walkleft", false);
            anim.SetBool("walkright", true);
            anim.SetBool("walkup", false);
            anim.SetBool("walkdown", false);
            anim.SetBool("idle", false);
        }
        else if (y < 0)
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
        }
    }

    public void SaveData(ref GameData data)
    {
        data.SceneNumber = SceneManager.GetActiveScene().buildIndex;
        if (data.SceneNumber == 11)
            data.isBinge = true;
    }
    public void LoadData(GameData data){}
}
