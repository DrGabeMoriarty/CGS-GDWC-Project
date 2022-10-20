using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{
    float m_Xaxis,m_Yaxis;
    public float speed = 1.0f;
    public int health = 100;
    public bool died = false;
    public Vector2 looking;
    bool m_Firing=false;
    public float firepoint = 1f;
    public float reloadTime = 0.5f;
    public float rollDuration = 0.2f;
    bool m_PointerBusy=false;
    public float rollReload = 0.8f;
    public float inaccuracyFloat = 1f;
    public float recoilplr = 1f;
    public int ammocapacity = 300, ammo=300,ammoRate=3;              //ammo
    
    public int staminacap = 600;                //stamina
    public int stamina = 600;
    public int dashUsage = 60;

    public GameObject currenProj;
    public Camera maincam;
    
    private Vector2 m_Direction = new Vector2(0, -1);
    private Vector2 m_DodgeDir = new Vector2(0, 0);
    private Vector2 m_Deviation;
    private Vector2 m_Inaccuracy;
    private Rigidbody2D m_Rbd;
    private float m_LastFireTime = 0f;
    private float m_LastRollTime = 0f;
    private Quaternion m_Rot = Quaternion.Euler(0, 0, 0);
    public bool invincible = false;
    int m_DashDamage=30;
    Collider2D m_DashCollider;
    Animator m_Anim;
    public GameObject[] projList;
    ParticleSystem m_Trail;
    TrailRenderer m_TrailRender;
    private HealthBar m_AmmoBar, m_StaminaBar;
    bool m_IsDashing=false;
    public GameObject statIndicators;

    void Start()
    {
        died=false;
        gameObject.GetComponent<Collider2D>().enabled=true;
        m_Rbd = GetComponent<Rigidbody2D>();
        m_Anim = this.gameObject.GetComponent<Animator>();
        m_DashCollider = GameObject.Find("dashDamager").GetComponent<CircleCollider2D>();
        //The above can just be statically assigned to the prefab since there is only 1
        //using Find() is expensive
        m_DashCollider.enabled = false;
        //disable this by default ^
        //just make the default value 0.5 instead of assigning it in script ^

        m_TrailRender = this.gameObject.GetComponent<TrailRenderer>();
        m_Trail = GetComponent<ParticleSystem>();
        m_TrailRender.enabled = false;
        m_Trail.Stop();
        //same thing here, just disable them by default.

        health = CeoScript.Health;
        speed = CeoScript.Speed;
        ammocapacity = CeoScript.Ammocapacity;
        staminacap = CeoScript.Staminacap;
        //invincible=false;
        
        
        m_AmmoBar=GameObject.Find("AmmoBar").GetComponent<HealthBar>();               //initializing stamina and ammo bars.
        m_StaminaBar=GameObject.Find("StaminaBar").GetComponent<HealthBar>();
        m_AmmoBar.InitializeHealth(ammocapacity);
        m_StaminaBar.InitializeHealth(staminacap);
        m_AmmoBar.SetHealth(ammocapacity);
        m_StaminaBar.SetHealth(staminacap);

        currenProj = CeoScript.ActivePowerUp;    //for level testing//
        if(currenProj !=null)
        {
            
        }
        Switchweapons();
    }

    
    void FixedUpdate()
    {
        //Input
        m_Xaxis = Input.GetAxisRaw("Horizontal");
        m_Yaxis = Input.GetAxisRaw("Vertical");

        //to change animation from idle to walking, without indulging in the complexities of 16 animations inside a blend tree
        if(m_Xaxis !=0 || m_Yaxis !=0)
            m_Anim.SetFloat("animationSpeed",1);
        else
            m_Anim.SetFloat("animationSpeed",0.5f);
        
        //Bullet positioning
        looking = (m_Rbd.position - (Vector2)maincam.ScreenToWorldPoint(Input.mousePosition)).normalized;
        looking *= firepoint;
        
        //sending input to the sprite animator
        float facingX,facingY;
        if(looking.normalized.x>1/Mathf.Sqrt(2) && !m_IsDashing)
            facingX=-1;
        else if(looking.normalized.x<-1/Mathf.Sqrt(2) && !m_IsDashing)
            facingX=1;
        else
            facingX=0;

        if(looking.normalized.y>1/Mathf.Sqrt(2) && !m_IsDashing)
            facingY=-1;
        else if(looking.normalized.y<-1/Mathf.Sqrt(2) && !m_IsDashing)
            facingY=1;
        else
            facingY=0;

        //firing weapon
        if (Input.GetButton("Fire1") && (Time.time > m_LastFireTime + reloadTime) && !(invincible) && currenProj !=null && !m_PointerBusy && ammo > 0)
        {   
            ammo-=ammoRate;
            FireWeapon((m_Rbd.position-looking)+m_Inaccuracy, m_Rot);
	        m_Rbd.velocity = recoilplr*looking; //recoil for player
            m_AmmoBar.SetHealth(ammo);        //ammo, stamina bar set
        } else if (!Input.GetButton("Fire1") && ammo < ammocapacity && Time.time > m_LastFireTime + reloadTime){   //ammo recharge
	        ammo++;
            m_AmmoBar.SetHealth(ammo);        //ammo, stamina bar set
        }
	
        m_Direction = new Vector2(m_Xaxis, m_Yaxis);

        if (m_Direction.x != 0 || m_Direction.y != 0)
        {
            //Dash trigger
            if (Input.GetButton("Fire2") && (Time.time > m_LastRollTime + rollReload) && stamina >= dashUsage)
            {
                invincible = true;
                m_IsDashing=true;
                m_LastRollTime = Time.time;
                m_DodgeDir = m_Direction;
                m_DashCollider.enabled=true;
                if(stamina>=dashUsage)
		            stamina -= dashUsage;
                else if(stamina<dashUsage)
                    stamina=0;
                m_StaminaBar.SetHealth(stamina);

                AudioManager.Instance.Play("dashWhoosh");   //play dash sound
                AudioManager.Instance.Play("dashFire");
                AudioManager.Instance.SetVolume("dashFire",(float)(stamina)/staminacap);
            }
        }

        //dash process
        if (invincible && m_IsDashing)
        {
            m_Rbd.velocity = (Vector3)m_DodgeDir * (5 * speed);

            facingX = m_Xaxis;
            facingY = m_Yaxis;
                
            //trailRender.enabled = true;   //trailRender, yes or no? hmmm...

            var newEmission = m_Trail.emission;
                newEmission.rateOverDistance = 100 * (stamina)/staminacap;
                m_Trail.Play();

            if (Time.time > m_LastRollTime + rollDuration)
            {
                m_Rbd.velocity = Vector2.zero;
                m_Trail.Stop();
                invincible = false;
                m_IsDashing=false;
                m_DashCollider.enabled = false;
            }
            

            
            StartCoroutine(TrailfadeDelay());
            
            

        } //moved as an else since its more efficient than not
        else  // Normal movement
        {
            //modified the statements as otherwise it would stop player motion if stamina is regenerating
            if (!invincible && stamina < staminacap)    //stamina recharge
            {
                if(stamina<=0)
                    stamina=0;
                if(Time.time > m_LastRollTime + rollReload)
                    stamina+=1;
                m_StaminaBar.SetHealth(stamina);
            } 
            //Motion
                
        
        if(CeoScript.CurrentGameState!=CeoScript.GameState.BossBattleCleared || CeoScript.DangerLevel>0)     
            m_Rbd.transform.position += (Vector3) m_Direction * (speed * Time.fixedDeltaTime);

        }
        
        if(CeoScript.ActivePowerUp==null && CeoScript.CurrentGameState==CeoScript.GameState.PreForestLevel)
        {
            m_Anim.SetFloat("xInput",m_Xaxis);
            m_Anim.SetFloat("yInput",m_Yaxis);
        }
        else
        {
            m_Anim.SetFloat("xInput",facingX);
            m_Anim.SetFloat("yInput",facingY);
        }

        // I have moved the stuff in the second update loop into here
        // please tell me you had good reason to have 2 update loops :harold:       //yep, reason was good, but there was a misunderstanding

        //this doesnt need to be set unless it is updated
        //so putting these there \/
        
        /*
        ammoBar.setHealth(ammo);        //ammo, stamina bar set
        staminaBar.setHealth(stamina);
        */

        //bruh
        //why do you have a loop that constantly checks for the sprites?        //yep, I realized that today :harold:
        //put these things in a function that is only triggered when
        //the player goes over a powerup
        /*
        
        */



        //why is this seperate from the firing :confused:               //was with the firing actually, but then thought not to update it in fixedupdate frames
        /*
        if (Input.GetButton("Fire1") && (Time.time > lastFireTime + reloadTime) && !(invincible) && currenProj !=null && !pointerBusy && ammo > 0)  //ammo update
            ammo-=ammoRate;
        */

        /*
        if(invincible)            //dash effects
        {
            var newEmission = trail.emission;
                newEmission.rateOverDistance = 100 * stamina/staminacap;
                trail.Play();

            if(dashLight.intensity<1)
                    dashLight.intensity+=0.05f;
            StartCoroutine(trailfadeDelay());
            AudioManager.instance.Play("dashEffect");   //play dash sound
            cameraShake.instance.shakeCamera(1f*stamina/staminacap,rollDuration);
        } //moved as an else since its more efficient than not
        else if (!invincible && stamina < staminacap)    //stamina recharge
        {
            if(stamina<=0)
                stamina=0;
            if(Time.time > lastRollTime + rollReload)
	            stamina+=2;
            staminaBar.setHealth(stamina);
        }
        */
        //i moved this into an already existing if statement
        
        
    }
    
    IEnumerator TrailfadeDelay()
    {
        yield return new WaitForSeconds(0.3f);
        m_TrailRender.enabled = false; 
    }

    void FireWeapon(Vector3 position, Quaternion rotation)
    {
	    //inaccuracy for player gun, idk how to exactly implenet it for the enmy bullets, since
	    //the logic for it has changed
	    m_Inaccuracy = m_Deviation;
	    m_Inaccuracy = UnityEngine.Random.Range(-1.0f,1.0f)*inaccuracyFloat*m_Inaccuracy;
        //moved this from the update loop

        GameObject bullet = Instantiate(currenProj, position, rotation);
        PlayShotSound();
        
        if (currenProj.name == "shotgun")
        {
            m_Deviation = Vector2.Perpendicular(looking);

            if(CeoScript.CurrentGameState==CeoScript.GameState.BossBattle)
                m_Deviation /= 10;
            else
                m_Deviation /= 5;
            //moved this from the update loop

            
        }
        
        m_LastFireTime = Time.time;
        
    }

    public void TakeDamage(int dam)
    {
        if (!invincible)
        {
            health -= dam;
            CeoScript.Health -= dam;
            Debug.Log("health:" + health);

            AudioManager.Instance.Play("playerDamage");
            

            if (health <= 0)
            {
                health=0;
                statIndicators.SetActive(false);
                CeoScript.CurrentGameState=CeoScript.GameState.GameOver;
                died=true;
                //dying animation
                m_Anim.SetBool("isDead",true);
                gameObject.GetComponent<Collider2D>().enabled = false;
                
                //trigger game over
                StartCoroutine(GameOverSequence());

                this.enabled = false;
            }
        }
    }

    public void DeathSound()
    {
        AudioManager.Instance.Play("playerDeath");
    }

    public void PlayShotSound()
    {
        if(currenProj==projList[0])
            AudioManager.Instance.Play("bullet");
        else if(currenProj==projList[1] && !m_Firing)
        {
            //StartCoroutine(machineGunLoop());
            AudioManager.Instance.Play("machine_gun_shot");
        }
        else if(currenProj==projList[2])
            AudioManager.Instance.Play("Sniper");
        else if(currenProj==projList[3])
            AudioManager.Instance.Play("shotGun");
        else if(currenProj==projList[4])
        {
            AudioManager.Instance.Play("rpg_fire");
            if(ammo<=0)
                StartCoroutine(PlaySound("rpg_load",1.5f));
        }
    }   
    IEnumerator PlaySound(string name, float delay)    //play sound with delay/
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.Play(name);
    }
    
    IEnumerator GameOverSequence()
    {
        
        AudioManager.Instance.Stop("boss_phase_1");
        AudioManager.Instance.Stop("boss_phase_2");
        AudioManager.Instance.Stop("pre_forest_theme");
		AudioManager.Instance.Stop("pre_forest_danger");
        AudioManager.Instance.Stop("forest_normal_theme");
        AudioManager.Instance.Stop("forest_danger_theme");
        AudioManager.Instance.Stop("hotel_normal_theme");
        AudioManager.Instance.Stop("hotel_danger_theme");
        AudioManager.Instance.Stop("hello_pidjon");
        AudioManager.Instance.Stop("pidjon_da_god");
        
        yield return new WaitForSeconds(2);
        CeoScript.GameOver();
    }

    public void cursorOnButton()
    {
        m_PointerBusy=true;
    }
    public void CursorOffButton()
    {
        m_PointerBusy=false;
    }

    public void Switchweapons(){
        
        CeoScript.ActivePowerUp = currenProj;
        for (int i = 0; i < 5; i++)             //sprite switching
        {
            if(currenProj !=null && currenProj.name==projList[i].name)
            {
                m_Anim.SetInteger("attackMode",i+1);
            }
        }
    }
     
}
