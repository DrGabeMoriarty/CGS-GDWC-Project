using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image back_bar;
    [SerializeField] private Image fill_bar;

    void Start(){
        back_bar.fillAmount = (health.currhealth/health.getMaxHealth());
    }

    void Update()
    {
        fill_bar.fillAmount = (health.currhealth / health.getMaxHealth()); 
    }

}
