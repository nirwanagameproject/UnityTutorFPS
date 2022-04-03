using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController character_Controller;

    private Vector3 move_Direction;

    public float speed = 5f;
    private float gravity = 20f;
    public float jump_force = 10f;
    private float vertical_Velocity;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        character_Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveThePlayer();
    }

    void MoveThePlayer() {
        //get input value
        move_Direction = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f,
                                        Input.GetAxis(Axis.VERTICAL));

        //move position
        move_Direction = transform.TransformDirection(move_Direction);
        //define speed movement - deltaTime will make slow movement
        move_Direction *=speed * Time.deltaTime;

        ApplyGravity();

        character_Controller.Move(move_Direction);
    }

    void ApplyGravity() {
        vertical_Velocity -= gravity * Time.deltaTime;
        PlayerJump();
        move_Direction.y = vertical_Velocity * Time.deltaTime;

        // if(character_Controller.isGrounded){
        //     //add velocity minus gravity
        //      vertical_Velocity -= gravity * Time.deltaTime;

        //      //check jump key in
        //      PlayerJump();
        // } else {
        //     //velocity reduce by gravity
        //     vertical_Velocity -= gravity * Time.deltaTime;
        // }

        //y position back to default
        move_Direction.y = vertical_Velocity * Time.deltaTime;
        //move_Direction.y = vertical_Velocity;
    }

    void PlayerJump() {
        //if key space - for jump
        if(character_Controller.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            //set position into jump
            vertical_Velocity = jump_force;
        }

    }
}
