using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sp;
    private bool dead = false;

    [Header("Health")]
    [SerializeField] private float maxhealth = 100f; 
    
    [Header ("Iframes")]
    [SerializeField] private float inv = 5f;
    [SerializeField] private int num_flash = 4;

    public float currhealth { get; private set; }

    private void Awake(){
        currhealth = maxhealth;
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
    }
  
    public void TakeDamage(float damage){

        currhealth = Mathf.Clamp(currhealth - damage,0f,maxhealth);
    
        if(currhealth > 0){
            StartCoroutine(Invincible());
            anim.SetTrigger("hurt");
        }
        else if(!dead){
            
            
            if(GetComponent<Player_Controller>() != null){
                
                SceneManager.LoadScene(1); //Rename this if any error
                GetComponent<Player_Controller>().enabled =false;
            }

            if (GetComponent<Enemy>() != null)
            {
                GetComponent<Enemy>().enabled = false;
                Destroy(gameObject);
            }
            
            if(GetComponent<Executioner>() != null)
            {
                SceneManager.LoadScene(0);
            }

            dead = true;

            anim.SetTrigger("die");
        }
        }

    public void GiveHealth(float health){
        currhealth = Mathf.Clamp(currhealth + health,0f,maxhealth);
    }

    public void Inv()
    {
        StartCoroutine(Invincible2());
    }

    private IEnumerator Invincible2()
    {
        Physics2D.IgnoreLayerCollision(6, 3, true);

        for (int i = 0; i < num_flash; i++)
        {
            yield return new WaitForSeconds(inv / num_flash);
            yield return new WaitForSeconds(inv / num_flash);
        }
        Physics2D.IgnoreLayerCollision(6, 3, false);

    }

    private IEnumerator Invincible(){
        
        Physics2D.IgnoreLayerCollision(6,3,true);

        for (int i = 0; i < num_flash; i++){
            sp.color = new Color(1,0,0,0.5f);
            yield return new WaitForSeconds(inv/num_flash);       
            sp.color = Color.white;
            yield return new WaitForSeconds(inv/num_flash);     
        }
        Physics2D.IgnoreLayerCollision(6,3,false);
    
    }

    private void Deactivate(){
        gameObject.SetActive(false);
    }

    public float getMaxHealth()
    {
        return maxhealth;
    }

}
