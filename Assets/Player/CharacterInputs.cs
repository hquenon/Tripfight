using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputs : MonoBehaviour
{
    public CharacterController2D controller;
    public float speed;


    float horizontalMove = 0f;
    bool jump = false;
    bool dash = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Fire3"))
        {
            controller.Dash();
        }



        if (Input.GetButtonDown("Jump"))
        {
            controller.Jump();
        }


    }
    void FixedUpdate()
    {
        controller.Move(horizontalMove * speed * Time.deltaTime);



    }
}
