using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
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
    public int energyCapacity = 300, energy=300,energyRate=3;              //energy
    public int dashUsage = 60;

    public GameObject currenProj;
    public Camera maincam;
    
    private Vector2 m_Direction = new Vector2(0, -1);
    private Vector2 m_DodgeDir = new Vector2(0, 0);
    private Rigidbody2D m_Rbd;
    private float m_LastFireTime;
    private float m_LastRollTime;
    private Quaternion m_Rot = Quaternion.Euler(0, 0, 0);
    public bool invincible;
    Collider2D m_DashCollider;
    Animator m_Anim;
    public GameObject[] projList;
    ParticleSystem m_Trail;
    TrailRenderer m_TrailRender;
    private HealthBar m_energyBar;
    bool m_IsDashing=false;
    public GameObject statIndicators;

    void Start()
    {
        died=false;
        gameObject.GetComponent<Collider2D>().enabled=true;
        m_Rbd = GetComponent<Rigidbody2D>();
        m_Anim = this.gameObject.GetComponent<Animator>();
        m_DashCollider = GameObject.Find("dashDamager").GetComponent<CircleCollider2D>();
        m_DashCollider.enabled = false;
        m_TrailRender = this.gameObject.GetComponent<TrailRenderer>();
        m_Trail = GetComponent<ParticleSystem>();
        m_TrailRender.enabled = false;
        m_Trail.Stop();
        //same thing here, just disable them by default.

        health = CEO_Script.Health;
        speed = CEO_Script.Speed;
        energyCapacity = CEO_Script.energyCapacity;
        //invincible=false;
        
        
        m_energyBar=GameObject.Find("energyBar").GetComponent<HealthBar>();               //initializing energy and energy bars.
        m_energyBar.InitializeHealth(energyCapacity);
        m_energyBar.SetHealth(energyCapacity);
        

        currenProj = CEO_Script.ActivePowerUp;    //for level testing//
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
        if (Input.GetButton("Fire1") && (Time.time > m_LastFireTime + reloadTime) && !(invincible) && currenProj !=null && !m_PointerBusy && energy > 0)
        {   
            energy-=energyRate;
            FireWeapon((m_Rbd.position - looking), m_Rot);
            m_energyBar.SetHealth(energy);        //energy bar set
        } else if (!Input.GetButton("Fire1") && energy < energyCapacity && Time.time > m_LastFireTime + reloadTime){   //energy recharge
	        energy++;
            m_energyBar.SetHealth(energy);        //energy bar set
        }
	
        m_Direction = new Vector2(m_Xaxis, m_Yaxis);

        if (m_Direction.x != 0 || m_Direction.y != 0)
        {
            //Dash trigger
            if (Input.GetButton("Fire2") && (Time.time > m_LastRollTime + rollReload) && energy >= dashUsage)
            {
                invincible = true;
                m_IsDashing=true;
                m_LastRollTime = Time.time;
                m_DodgeDir = m_Direction;
                m_DashCollider.enabled=true;
                if(energy>=dashUsage)
		            energy -= dashUsage;
                else if(energy<dashUsage)
                    energy=0;

                AudioManager.Instance.Play("dashWhoosh");   //play dash sound
                AudioManager.Instance.Play("dashFire");
                AudioManager.Instance.SetVolume("dashFire",(float)(energy)/energyCapacity);
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
                newEmission.rateOverDistance = 100 * (energy)/energyCapacity;
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
            //modified the statements as otherwise it would stop player motion if energy is regenerating
            if (!invincible && energy < energyCapacity)    //energy recharge
            {
                if(energy<=0)
                    energy=0;
                if(Time.time > m_LastRollTime + rollReload)
                    energy+=1;
            } 
            //Motion
                
        
        if(CEO_Script.CurrentGameState!=CEO_Script.GameState.BossBattleCleared || CEO_Script.DangerLevel>0)     
            m_Rbd.transform.position += (Vector3) m_Direction * (speed * Time.fixedDeltaTime);

        }
        
        if(CEO_Script.ActivePowerUp==null && CEO_Script.CurrentGameState==CEO_Script.GameState.PreForestLevel)
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
        energyBar.setHealth(energy);        //energy, energy bar set
        energyBar.setHealth(energy);
        */

        //bruh
        //why do you have a loop that constantly checks for the sprites?        //yep, I realized that today :harold:
        //put these things in a function that is only triggered when
        //the player goes over a powerup
        /*
        
        */



        //why is this seperate from the firing :confused:               //was with the firing actually, but then thought not to update it in fixedupdate frames
        /*
        if (Input.GetButton("Fire1") && (Time.time > lastFireTime + reloadTime) && !(invincible) && currenProj !=null && !pointerBusy && energy > 0)  //energy update
            energy-=energyRate;
        */

        /*
        if(invincible)            //dash effects
        {
            var newEmission = trail.emission;
                newEmission.rateOverDistance = 100 * energy/energycap;
                trail.Play();

            if(dashLight.intensity<1)
                    dashLight.intensity+=0.05f;
            StartCoroutine(trailfadeDelay());
            AudioManager.instance.Play("dashEffect");   //play dash sound
            cameraShake.instance.shakeCamera(1f*energy/energycap,rollDuration);
        } //moved as an else since its more efficient than not
        else if (!invincible && energy < energycap)    //energy recharge
        {
            if(energy<=0)
                energy=0;
            if(Time.time > lastRollTime + rollReload)
	            energy+=2;
            energyBar.setHealth(energy);
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
        
        GameObject bullet = Instantiate(currenProj, position, rotation);
        PlayShotSound();
        m_LastFireTime = Time.time;
    }

    public void TakeDamage(int dam)
    {
        if (!invincible)
        {
            health -= dam;
            CEO_Script.Health -= dam;
            Debug.Log("health:" + health);

            AudioManager.Instance.Play("playerDamage");
            

            if (health <= 0)
            {
                health=0;
                statIndicators.SetActive(false);
                CEO_Script.CurrentGameState=CEO_Script.GameState.GameOver;
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
            if(energy<=0)
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
        CEO_Script.GameOver();
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
        
        CEO_Script.ActivePowerUp = currenProj;
        for (int i = 0; i < 5; i++)             //sprite switching
        {
            if(currenProj !=null && currenProj.name==projList[i].name)
            {
                m_Anim.SetInteger("attackMode",i+1);
            }
        }
    }
     
}
