using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralAttack : MonoBehaviour, IHitboxResponder, IAttack
{
    public HitBox hitbox;
    public float attackTime = 0.25f;
    public float dammage = 25;
    public float projectionForce = 1000;

    private float timer;
    public void collisionedWith(Collider2D collider)
    {
        hitbox.startCheckingCollision();
        HurtBox hurtbox = collider.GetComponent<HurtBox>();
        hurtbox.getHit(dammage);

        Vector3 attackDirection = collider.transform.position - this.transform.position;
        hurtbox.getPushed(attackDirection * projectionForce);
    }

    public void attack()
    {
        hitbox.startCheckingCollision();
        timer = attackTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        hitbox.useResponder(this);
        hitbox.stopCheckingCollision();
    }

    // Update is called once per frame
    void Update()
    {
     if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            hitbox.stopCheckingCollision();
        }
    }
}
