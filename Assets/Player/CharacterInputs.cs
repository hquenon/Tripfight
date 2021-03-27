using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputs : MonoBehaviour
{
    public CharacterController2D controller;
    public float speed;


    float horizontalMove = 0f;
    bool jump = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        if(Input.GetAxisRaw("Jump") == 1)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }

    }
    void FixedUpdate()
    {
        controller.Move(horizontalMove * speed * Time.deltaTime, false, jump);
    }
}
