﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem
{
    public const int MAX_FRAGMENTS = 2;
    public event EventHandler OnDamaged, OnHealed, OnDead;
    private List<Heart> heartList;
    public HealthSystem(int healthAmount)
    {
        heartList = new List<Heart>();
        for (int i = 0; i < healthAmount; i++)
        {
            Heart heart = new Heart(2);
            heartList.Add(heart);
        }

        //heartList[heartList.Count - 1].SetFragments(0);
    }
    public List<Heart> GetHeartList()
    {
        return heartList;
    }
    public void Damage(int damageAmt)
    {
        for(int i=heartList.Count-1;i>=0;i--)//cycle through all hearts
        {
            Heart heart = heartList[i];
            if (damageAmt>heart.GetFragmentAmount())//test heart health versus damage
            {
                damageAmt -= heart.GetFragmentAmount();
                heart.Damage(heart.GetFragmentAmount());
            }
            else
            {
                heart.Damage(damageAmt);
                break;
            }
        }
        if (OnDamaged!=null)
        {
            OnDamaged(this, EventArgs.Empty);
        }
        if (IsDead())
        {
            if (OnDead != null)
            {
                OnDead(this, EventArgs.Empty);
            }
        }
    }
    public void Heal(int healAmt)
    {
        for(int i=0;i < heartList.Count;i++)//cycle through all hearts
        {
            Heart heart = heartList[i];
            int missingFragments = MAX_FRAGMENTS - heart.GetFragmentAmount();
            if (healAmt>missingFragments)
            {
                healAmt -= missingFragments;
                heart.Heal(missingFragments);
            }
            else
            {
                heart.Heal(healAmt);
                    break;
            }
        }
        if (OnHealed != null)
        {
            OnHealed(this, EventArgs.Empty);
        }
    }
    public bool IsDead()
    {
        return heartList[0].GetFragmentAmount() == 0;
    }
    public class Heart
    {
        private int fragments;
        public Heart(int fragments)
        {
            this.fragments = fragments;
        }
        public int GetFragmentAmount()
        {
            return fragments;
        }
        public void SetFragments(int fragments)
        {
            this.fragments = fragments;
        }
        public void Damage(int damageAmt)
        {
            if (damageAmt>=fragments)
            {
                fragments = 0;
            }
            else
            {
                fragments -= damageAmt;
            }
        }
        public void Heal(int healAmt)
        {
            if (fragments+healAmt>MAX_FRAGMENTS)
            {
                fragments = MAX_FRAGMENTS;
            }
            else
            {
                fragments += healAmt;
            }
        }
    }
}
public class HealthManager : MonoBehaviour
{
    public enum UIType { player, enemy }
    public UIType ui = UIType.player;
    public float maxHealth = 100, minHealth = 0;
    public Image HealthFill, DamageFill, BarImage;

    public float showHealthTime = 1, fadeOutTime = .5f, damageShowTime = 1, damageShowSpeed = 1f;
    public bool IsDead = false;
    public Color HealthColor, DamageColor, BackColor;
    private Color invisible = new Color(0, 0, 0, 0);
    public GameObject HealthPickupPrefab;

    private float currentHealth = 100, damageShowTimer, healthBarFadeTimer;
    private bool isHealing = false, coroutineStarted = false, healthIsVisible = false, deathCoroutineStarted = false;
    private CharacterObject character;
    public Material DizzyMat;
    //private PlayerRespawner respawner;

    private void Start()
    {
        character = GetComponentInParent<CharacterObject>();
        //respawner = GameObject.FindGameObjectWithTag("Respawner").GetComponent<PlayerRespawner>();
        SetDefaultMeter();

        switch (ui)
        {
            case UIType.player:
                UpdateFill();
                break;
            case UIType.enemy:
                break;
            default:
                break;
        }
        

        //if (ui == UIType.enemy)
        //{
        //    HideHealth();
        //}
    }

    private void HideHealth()
    {
        BarImage.color = invisible;
        DamageFill.color = invisible;
        HealthFill.color = invisible;

        healthIsVisible = false;

    }

    private void UpdateFill()
    {
        HealthFill.fillAmount = currentHealth / 100;
    }

    private void UpdateFillForHeal()
    {
        DamageFill.fillAmount = currentHealth / 100f;
    }

    private void Update()
    {
        switch (ui)
        {
            case UIType.player:
                if (damageShowTimer < 0)//if the timer is up
                {
                    if (HealthFill.fillAmount < DamageFill.fillAmount && isHealing)//if the bars aren't equal and it's healing
                    {
                        HealthFill.fillAmount += damageShowSpeed * Time.deltaTime;//increase the health bar
                    }
                    else if (HealthFill.fillAmount < DamageFill.fillAmount)//if the health amount is smaller than the damage show
                    {
                        DamageFill.fillAmount -= damageShowSpeed * Time.deltaTime;//decrease the damage bar 
                    }
                    else if (isHealing)//otherwise if the bars are even we're done showing healing so turn the bool off
                        isHealing = false;
                }
                else//DECREASE TIMERS
                {
                    damageShowTimer -= Time.deltaTime;
                    healthBarFadeTimer -= Time.deltaTime;
                }

                if (healthBarFadeTimer < 0)//if we need to start fading health out
                {
                    if (!coroutineStarted && healthIsVisible)
                        StartCoroutine(FadeHealth());
                }
                break;
            case UIType.enemy:
                break;
            default:
                break;
        }
       

        //if (!this.gameObject.CompareTag("Player"))//if it's not the player, make health bar face camera
        //    BarImage.transform.LookAt(Camera.main.transform);
    }

    private IEnumerator FadeHealth()
    {
        coroutineStarted = true;

        for (float f = fadeOutTime; f > 0; f -= Time.deltaTime)//iterate over time
        {
            Color h = HealthFill.color;//get color
            Color d = DamageFill.color;//gt color
            Color b = BarImage.color;

            h.a = f;//set the alpha to the variable being counted down for
            d.a = f;
            b.a = f;

            HealthFill.color = h;//set that to be our new color
            DamageFill.color = d;
            BarImage.color = b;

            yield return new WaitForEndOfFrame();
        }

        //if (ui == UIType.enemy)
        //{
        //    if (healthBarFadeTimer > 0)//if you get hit while it's fading
        //    {
        //        ShowHealth();//go back to showing
        //    }

        //    if (healthBarFadeTimer <= 0)//as long as that timer isn't running, we've done a successful fade and can set the bool to false
        //        healthIsVisible = false;
        //}

        coroutineStarted = false;
    }


    void SetDefaultMeter()
    {
        currentHealth = 0;
        UpdateFill();
        UpdateFillForHeal();
    }
    public void ChangeHealth(int _val)
    {
        currentHealth += _val;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        damageShowTimer = damageShowTime;//set the timer back to max when injured happens
        if (_val>0)
        {

            isHealing = true;
            switch (ui)
            {
                case UIType.player:
                    UpdateFillForHeal();
                    break;
                case UIType.enemy:
                    break;
                default:
                    break;
            }
        }
        else
        {
            healthBarFadeTimer = showHealthTime;//reset timer for showing health bar here too
            switch (ui)
            {
                case UIType.player:
                    UpdateFill();
                    break;
                case UIType.enemy:
                    break;
                default:
                    break;
            }
        }
    }
    public void AddHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;


        damageShowTimer = damageShowTime;//set the timer back to max when injured happens

        isHealing = true;
        switch (ui)
        {
            case UIType.player:
                UpdateFillForHeal();
                break;
            case UIType.enemy:
                break;
            default:
                break;
        }
        
    }

    public void RemoveHealth(int amount)
    {
        damageShowTimer = damageShowTime;//set the timer back to max when injured happens
        healthBarFadeTimer = showHealthTime;//reset timer for showing health bar here too


        currentHealth -= amount;

        if (currentHealth <= minHealth)
        {
            currentHealth = minHealth;
            //if (!deathCoroutineStarted)
            //    StartCoroutine(DeathEvent(false));
        }
        switch (ui)
        {
            case UIType.player:
                UpdateFill();
                break;
            case UIType.enemy:
                break;
            default:
                break;
        }
        
    }

    private void ShowHealth()
    {
        BarImage.color = BackColor;
        HealthFill.color = HealthColor;
        DamageFill.color = DamageColor;

        healthIsVisible = true;
    }

    public void FinisherDeath()
    {
        if (!deathCoroutineStarted)
            StartCoroutine(DeathEvent(true));
        //GameEngine.gameEngine.Screenshake();
    }

    /// <summary>
    /// waits until the death animation is done and then destroys the character
    /// </summary>
    /// <returns></returns>
    IEnumerator DeathEvent(bool spawnHealth)
    {
        deathCoroutineStarted = true;
        IsDead = true;

        if (spawnHealth)
            Instantiate(HealthPickupPrefab, transform.position, transform.rotation);

        yield return new WaitForSeconds(6);//get length of death animation        

        switch (character.controlType)
        {
            case CharacterObject.ControlType.AI:
                gameObject.SetActive(false);
                break;
            case CharacterObject.ControlType.PLAYER:
                //RESPAWN HERE
                //respawner.RespawnPlayer();
                break;
            default:
                break;
        }

    }
}
