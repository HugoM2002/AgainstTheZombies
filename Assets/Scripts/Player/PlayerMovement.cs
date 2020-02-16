using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controllerPlayer;
    public bool iswalking;
    public float speed = 10f;
    public float gravity = -9.90f;
    public float groundDistance = 0.4f;
    public float jumpHeigt = 3f;

    Vector3 velocity;

    public Transform groundCheck;
    public LayerMask groundMask;
    bool ground;

    // Update is called once per frame
    void Update()
    {
        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * X + transform.forward * Z;

        controllerPlayer.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controllerPlayer.Move(velocity * Time.deltaTime);

        ground = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(ground && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if(Input.GetButtonDown("Jump") && ground)
        {
            velocity.y = Mathf.Sqrt(jumpHeigt * -2f * gravity);
        }
    }

    public int GetPlayerStealth()
    {
        if (iswalking)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
}
