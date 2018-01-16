using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]

public class CharController : MonoBehaviour {
   
    public float speed = 3;

    public float moveSpeed;
    bool acceleration = true;

    RaycastHit hit;



    Animator anim;
    Camera cam;
    AimingCircle circle;


    private void Start()
    {
        anim.SetBool("Armed", false);
    }




    void Awake()
    {
        
        anim = GetComponent<Animator>();
        cam = Camera.main;
        circle= gameObject.GetComponentInChildren<AimingCircle>();

       
       





    }

    void Update()
    {
        Move();

        ifArmed();

      

        

        anim.SetFloat("Idle", GetComponentInChildren<AimingCircle>().choosePositionX);
        anim.SetFloat("Attack_Vertical", GetComponentInChildren<AimingCircle>().choosePositionY);

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

        if (GetComponentInChildren<AimingCircle>().ifVisible)

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);

        if (!GetComponentInChildren<AimingCircle>().ifVisible)
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

    private void LateUpdate()
    {

        // Adjusting feet position and rotation to ground 

        /*if (Physics.Raycast(anim.GetBoneTransform(HumanBodyBones.LeftToes).position,
           -anim.GetBoneTransform(HumanBodyBones.LeftToes).up, out hit, 0.2f))
        {
            anim.GetBoneTransform(HumanBodyBones.LeftToes).rotation =
                Quaternion.FromToRotation(anim.GetBoneTransform(HumanBodyBones.LeftToes).up, hit.normal)
                * anim.GetBoneTransform(HumanBodyBones.LeftToes).rotation;
            anim.GetBoneTransform(HumanBodyBones.LeftToes).position = hit.point;

        }

        if (Physics.Raycast(anim.GetBoneTransform(HumanBodyBones.RightToes).position,
          -anim.GetBoneTransform(HumanBodyBones.RightToes).up, out hit, 0.2f))
        {
            anim.GetBoneTransform(HumanBodyBones.RightToes).rotation =
               Quaternion.FromToRotation(anim.GetBoneTransform(HumanBodyBones.RightToes).up, hit.normal)
               * anim.GetBoneTransform(HumanBodyBones.RightToes).rotation;
            anim.GetBoneTransform(HumanBodyBones.RightToes).position = hit.point;

        }
        */

    }

    void Move()
    {
        float side, forward;
       
        
        side = anim.GetFloat("Side");
        forward = anim.GetFloat("Forward");
        
        


            anim.SetFloat("Forward", Input.GetAxis("Vertical"));

            anim.SetFloat("Side", Input.GetAxis("Horizontal"));

        if (Input.GetKey(KeyCode.LeftShift))

            StartCoroutine(speedUp());

        if (Input.GetKeyUp(KeyCode.LeftShift))

            StartCoroutine(slowDown());

        if (Input.GetMouseButton(1))   //right mouse button

            anim.SetBool("Block", true);
        else
            anim.SetBool("Block", false);






        if (Input.GetKey(KeyCode.Z))
            switch (acceleration)
            {
                case true:
                    moveSpeed += 0.05f;
                    if (moveSpeed >= 1.0f)
                        acceleration = false;
                    break;

                case false:
                    moveSpeed -= 0.05f;
                    if (moveSpeed <= 0.0f)
                        acceleration = true;
                    break;
               

            }
               

            

        if (moveSpeed > 1)
            moveSpeed = 1;
        if (moveSpeed < 0)
            moveSpeed = 0;

        anim.SetFloat("Speed", moveSpeed);
                
        
        

        if ( Mathf.Abs( forward)>0 ||Mathf.Abs(side) > 0)
            anim.SetBool("Movement", true);
        else
            anim.SetBool("Movement", false);


        Jump();





    }
    void Jump()
    {
        anim.SetBool("Jump", Input.GetKey(KeyCode.LeftAlt));
    }
    void ifArmed()
    {



        if (Input.GetKey(KeyCode.Space))
        {
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))

                anim.SetBool("Armed",true);

            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle_Armed"))

                anim.SetBool("Armed", false);

            anim.SetTrigger("Arm/Disarm");
        }
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            anim.SetTrigger("Attack 0");
           // StartCoroutine(circle.returnToCenter());
           
        }

        
            
    }
    

   

    IEnumerator speedUp( ) {

        for (; moveSpeed < 1; moveSpeed += 0.1f)
            yield return null;

    }
    IEnumerator slowDown() {

        for (; moveSpeed > 0; moveSpeed -= 0.1f)
            yield return null;

    }
}
