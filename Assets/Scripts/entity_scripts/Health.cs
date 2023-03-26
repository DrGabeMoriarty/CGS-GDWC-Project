using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sp;
    private bool enemyinv = false;

    [Header("Health")]
    [SerializeField] private float maxhealth = 100f; 
    
    [Header ("Iframes")]
    [SerializeField] private float inv = 5f;
    [SerializeField] private int num_flash = 4;
    [SerializeField] private float deathtime = 1f;

    public float currhealth { get; private set; }

    private Health playerhealth;

    private void Awake(){
        currhealth = maxhealth;
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        playerhealth = FindObjectOfType<Player_Controller>().GetComponent<Health>();
        if (playerhealth == null)
            playerhealth = FindObjectOfType<Player_Piano>().GetComponent<Health>();
    }
  
    public void TakeDamage(float damage){

        if (GetComponent<Executioner>() != null || GetComponent<Enemy>() != null || GetComponent<King>() != null)
        {
            if (!enemyinv)
            {
                currhealth = Mathf.Clamp(currhealth - damage, 0f, maxhealth);
                StartCoroutine(Inv_ex());
            }
        }
        else currhealth = Mathf.Clamp(currhealth - damage, 0f, maxhealth);

        if (currhealth > 0){
            StartCoroutine(Invincible());
            anim.SetTrigger("hurt");
        }
        else if(currhealth <= 0){
            if (GetComponent<Player_Controller>() != null || GetComponent<Player_Piano>()!=null 
                || GetComponent<Mike>() != null){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Rename this if any error
                GetComponent<Player_Controller>().enabled =false;
            }
            else if(GetComponent<Executioner>() != null || GetComponent<King>() != null)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                Invoke("DestroyEnemy", deathtime);
                playerhealth.GiveHealth(5f);
            }

            anim.SetTrigger("die");
        }
        }

    public void GiveHealth(float health){
        currhealth = Mathf.Clamp(currhealth + health,0f,maxhealth);
    }

    public void TakePhantomDamage(float damage)
    {
        currhealth = Mathf.Clamp(currhealth - damage, 0f, maxhealth);
    }

        private IEnumerator Inv_ex()
    {
        enemyinv = true;
        for (int i = 0; i < num_flash; i++)
        {
            sp.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(inv / num_flash);
            sp.color = Color.white;
            yield return new WaitForSeconds(inv / num_flash);
        }
        enemyinv = false;
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

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    public float getMaxHealth()
    {
        return maxhealth;
    }

}
