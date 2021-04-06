using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public CharacterStats stats;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getHit(float dmg)
    {
        stats.getHit(dmg);
    }

    public void getPushed(Vector3 force)
    {

        stats.getPushed(force);
    }

}
