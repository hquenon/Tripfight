using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private float dammage = 0;
    public float hurtInvincibilityTime=0.5f;

    private float hurtInvincibilityTimer;
    private bool invincible = false;



    CharacterController2D controller;
    Rigidbody2D rd;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        updateHurtInvincibilityTimer();
        Debug.Log("Damage: "+dammage);
        
    }

    public void getHit(float dmg)
    {
        if (invincible) { return; }
        setHurtInvincibilityTimer();
        dammage += dmg;
        controller.SetDisableTimer(0.25f);
        
    }

    public void getPushed(Vector3 force)
    {
        rd.velocity = Vector3.zero;
        rd.AddForce(force * dammage / 100);
    }

    private void setHurtInvincibilityTimer()
    {
        hurtInvincibilityTimer = hurtInvincibilityTime;
        invincible = true;
    }
    private void updateHurtInvincibilityTimer()
    {
        if (hurtInvincibilityTimer > 0)
        {
            hurtInvincibilityTimer -= Time.deltaTime;
        } else
        {
            invincible = false;
        }
        
    }

}
