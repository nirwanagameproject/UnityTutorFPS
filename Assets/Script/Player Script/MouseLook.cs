using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //make invisible private varible in unity
    [SerializeField]
    private Transform playerRoot, lookRoot;

    [SerializeField]
    private bool invert;

    [SerializeField]
    private bool can_Unlock = true;

    [SerializeField]
    private float sensitivy = 5f;

    [SerializeField]
    private float smooth_Weight = 0.4f;

    [SerializeField]
    private int smooth_Steps = 10;

    [SerializeField]
    private float roll_Angle = 10f;

    [SerializeField]
    private float roll_Speed = 3f;

    [SerializeField]
    private Vector2 default_Look_Limits = new Vector2(-70f, 80f);

    private Vector2 look_Angles;
    private Vector2 current_Mouse_Look;
    private Vector2 smooth_Move;
    private float current_Roll_Angle;
    private int last_Look_Frame;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        lockAndUnlockCursor();   
        if(Cursor.lockState == CursorLockMode.Locked) {
            LookAround();
        }
    }

    //open croshhair cursor
    void lockAndUnlockCursor(){

        //press escape
        if(Input.GetKeyDown(KeyCode.Escape)){

            //if still locked
            if(Cursor.lockState == CursorLockMode.Locked) {
                //open cursor state 
                Cursor.lockState = CursorLockMode.None;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void LookAround() {
        //unity use invert position x (up-down), y(left-right)
        current_Mouse_Look = new Vector2(
            Input.GetAxis(MouseAxis.MOUSE_Y), Input.GetAxis(MouseAxis.MOUSE_X)
        );

        //x for up-down, y for right left
        look_Angles.x += current_Mouse_Look.x * sensitivy * (invert ? 1f : -1f);
        look_Angles.y += current_Mouse_Look.y * sensitivy ;

        //limit x position for not greater than defualt
        look_Angles.x = Mathf.Clamp(look_Angles.x, default_Look_Limits.x, default_Look_Limits.y);

        //get input axis raw - for roll angle using z position (effect)
        // current_Roll_Angle = 
        //     Mathf.Lerp(current_Roll_Angle, Input.GetAxisRaw(MouseAxis.MOUSE_X) * roll_Angle, Time.deltaTime * roll_Speed);

        //look for gameobject up and down, player root for left-right
        lookRoot.localRotation = Quaternion.Euler(look_Angles.x , 0f, 0f); //current_Roll_Angle);
        playerRoot.localRotation = Quaternion.Euler(0f, look_Angles.y, 0f);
    }
}
