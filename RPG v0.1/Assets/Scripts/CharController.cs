using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CharController : MonoBehaviour {
    int direction = 1;
    public float speed = 3;
   // private bool ifAttack, prev_ifAttack;
    private float arrow_rot;

    Animator anim;
    Camera cam;
    Vector3 weapon_pos;
    public GameObject weapon;
    public GameObject HookRightHand;
    public GameObject HookBack;
    public GameObject arrow;
    private Vector3 mousePosition, prmousePosition;
    private float prx, pry, prz;


    void Awake()
    {
        
        anim = GetComponent<Animator>();
        cam = Camera.main;
        arrow.SetActive(false);
        prx = Input.mousePosition.x;
        pry = Input.mousePosition.y;
        prz = Input.mousePosition.z;


    }

    void Update()
    {
        Move();

        ifArmed();

       
        
        Quaternion targetRotation = Quaternion.LookRotation(
            new Vector3(transform.position.x - cam.transform.position.x,
            0, 
            transform.position.z - cam.transform.position.z));
       
       

        if (anim.GetFloat("Forward") > 0 && anim.GetFloat("Side") == 0)
            speed = 6;
        else speed = 2.5f;


        float step = speed * Time.deltaTime;



        if (anim.GetFloat("Forward") > 0 && cam.transform.rotation.y != transform.rotation.y)
        {
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
        }

        if(arrow.active)

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);

        if (!arrow.active)
        {
            if (anim.GetFloat("Forward") < 0 && cam.transform.rotation.y != transform.rotation.y)
            {
                targetRotation = Quaternion.LookRotation(
                    new Vector3(-transform.position.x + cam.transform.position.x,
                0,
                -transform.position.z + cam.transform.position.z));

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
            }
            if (anim.GetFloat("Side") > 0 && cam.transform.rotation.y != transform.rotation.y)
            {
                targetRotation = Quaternion.LookRotation(
                    new Vector3(transform.position.z - cam.transform.position.z,
                0, 
                -transform.position.x + cam.transform.position.x));

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
            }

            if (anim.GetFloat("Side") < 0 && cam.transform.rotation.y != transform.rotation.y)
            {
                targetRotation = Quaternion.LookRotation(
                    new Vector3(-transform.position.z + cam.transform.position.z,
                0,
                transform.position.x - cam.transform.position.x));

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
            }
        }


    }

    void Move()
    {
        float side, forward;
        
        side = anim.GetFloat("Side");
        forward = anim.GetFloat("Forward");
        


            anim.SetFloat("Forward", Input.GetAxis("Vertical"));

            anim.SetFloat("Side", Input.GetAxis("Horizontal"));
     
            anim.SetBool("Run", Input.GetKey(KeyCode.LeftShift));
        
        

        if ( Mathf.Abs( forward)>0 ||Mathf.Abs(side) > 0)
            anim.SetBool("Movement", true);
        else
            anim.SetBool("Movement", false);

       

      

        switch (direction)
        {
            case 1: anim.SetBool("FaceForward", true);
                anim.SetBool("FaceRight", false);
                anim.SetBool("FaceBackward", false);
                anim.SetBool("FaceLeft", false);
                if (side > 0 && forward == 0) direction = 2;
                else if (side < 0 && forward == 0) direction = 4;
                else if(side == 0 && forward < 0) direction = 3;
                break;

            case 2:
                anim.SetBool("FaceForward", false);
                anim.SetBool("FaceRight", true);
                anim.SetBool("FaceBackward", false);  
                anim.SetBool("FaceLeft", false);
                if (side == 0 && forward > 0) direction = 1;
                else if (side < 0 && forward == 0) direction = 4;
                else if (side == 0 && forward < 0) direction = 3;
                break;
            case 3:
                anim.SetBool("FaceForward", false);
                anim.SetBool("FaceRight", false);
                anim.SetBool("FaceBackward", true);
                anim.SetBool("FaceLeft", false);
                if (side == 0 && forward > 0) direction = 1;
                else if (side < 0 && forward == 0) direction = 4;
                else if (side > 0 && forward == 0) direction = 2;
                break;
            case 4:
                anim.SetBool("FaceForward", false);
                anim.SetBool("FaceRight", false);
                anim.SetBool("FaceBackward", false);
                anim.SetBool("FaceLeft", true);
                if (side == 0 && forward > 0) direction = 1;
                else if (side == 0 && forward < 0) direction = 3;
                else if (side > 0 && forward == 0) direction = 2;
                break;
         
        }


        Jump();





    }
    void Jump()
    {
        anim.SetBool("Jump", Input.GetKey(KeyCode.LeftAlt));
    }
    void ifArmed()
    {
        
         arrow_rot = arrow.transform.eulerAngles.z;


       // ifAttack = anim.GetBool("Attack");

        if (Input.GetKey(KeyCode.Space))

        anim.SetTrigger("Arm/Disarm");
        anim.SetBool("Attack", Input.GetKey(KeyCode.Mouse0));
        if (Input.GetKey(KeyCode.Mouse0))
            anim.SetTrigger("Attack 0");

        if (anim.GetBool("Attack 0"))
        {
            if (arrow_rot > 30 && arrow_rot < 90)
                anim.SetTrigger("Attack_Top_Right 0");
            if (arrow_rot > 330 || arrow_rot < 30)
                anim.SetTrigger("Attack_Right 0");
            if (arrow_rot > 150 && arrow_rot < 210)
                anim.SetTrigger("Attack_Left 0");
            if (arrow_rot > 270 && arrow_rot < 330)
                anim.SetTrigger("Attack_Down_Right");
            if (arrow_rot > 90 && arrow_rot < 150)
                anim.SetTrigger("Attack_Top");


        }

        //Rotating 180 arrow after attack
       
/*
        if (ifAttack && !prev_ifAttack)
            arrow.transform.Rotate(0,0,180);
        prev_ifAttack = ifAttack;
        
         */
            
    }
    void Equip() {

        weapon.transform.parent = HookRightHand.transform;
        
     
        arrow.SetActive(true);
       

    }
    void Unequip()
    {

        weapon.transform.parent = HookBack.transform;
        arrow.SetActive(false);



    }
    void ResetTrig() {
        anim.ResetTrigger("Arm/Disarm");
        anim.ResetTrigger("Attack_Right 0");
        anim.ResetTrigger("Attack_Left 0");
        anim.ResetTrigger("Attack_Top_Right 0");
        anim.ResetTrigger("Attack_Top");
        anim.ResetTrigger("Attack_Down_Right");
        anim.ResetTrigger("Attack 0");

    }
}
