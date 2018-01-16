using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAIver2 : MonoBehaviour
{


    public string state = "patrol";

    float rotSpeed = 1f;

    float angle;

    float accuracyWP = 5.0f;

    Vector3 yOffset = new Vector3(0, 1, 0);

    RaycastHit frontInfo, rightInfo, leftInfo;

    Vector3 forward, right, left;

    Animator anim;

    public Transform target;
    public Transform[] waypoints;
    public Transform player;
    public int currentWP;
    public GameObject weapon;
    public GameObject HookRightHand;
    public GameObject HookBack;

    public float frontObstacle, rightObstacle, leftObstacle;

    public bool hitFront, hitFrontRight, hitFrontLeft;


    public float distance = 10.0f;




    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

    }



    // Update is called once per frame
    void Update()
    {

        Vector3 direction = target.position - transform.position;

        getDirections();

        drawRays();

        direction.y = 0;



        angle = Vector3.Angle(direction, transform.forward);

        switch (state)
        {
            case "patrol":
                if (waypoints.Length > 0)
                {
                    target = waypoints[currentWP];
                    anim.SetBool("Movement", true);
                    if (Vector3.Distance(target.position, transform.position) < accuracyWP)
                    {
                        currentWP++;
                        if (currentWP + 1 > waypoints.Length)
                            currentWP = 0;

                    }


                    hitFront = Physics.Raycast(transform.position + yOffset, forward, out frontInfo, forward.magnitude) &&
                        frontInfo.collider.tag == "enviroment";

                    hitFrontRight = Physics.Raycast(transform.position + yOffset, right, out rightInfo, right.magnitude) &&
                        rightInfo.collider.tag == "enviroment";

                    hitFrontLeft = Physics.Raycast(transform.position + yOffset, left, out leftInfo, left.magnitude) &&
                        leftInfo.collider.tag == "enviroment";

                    if (hitFront || hitFrontLeft || hitFrontRight)
                        avoidObstacles();
                    else
                        followTarget(direction, target);



                }

                if (Vector3.Distance(player.position, transform.position) < 20 && angle < 60)
                {
                    anim.SetBool("Run", true);
                    state = "pursuit";

                }



                break;

            case "pursuit":

                
                

                if (hitFront || hitFrontLeft || hitFrontRight)
                    avoidObstacles();
                else
                    followTarget(direction, player.transform);


                if (Vector3.Distance(transform.position, player.position) < 2)
                {
                    anim.SetBool("Run", false);
                    anim.SetBool("Movement", false);
                    anim.SetTrigger("Arm/Disarm");
                    state = "attack";
                }


                if (Vector3.Distance(player.position, transform.position) > 20)
                {
                    
                    anim.SetBool("Run", false);
                    anim.SetTrigger("Arm/Disarm");
                    target = waypoints[currentWP];

                    state = "patrol";
                }
                break;

            case "attack":

                followTarget(player.transform.position, player.transform);

               
               
               


                if (Vector3.Distance(transform.position, player.position) > 2)
                {
                    
                    anim.SetBool("Movement", true);
                   

                    state = "pursuit";
                }
                break;
        }



    }

    void avoidObstacles()
    {



        if (hitFront)
            frontObstacle = 100 - (frontInfo.distance / forward.magnitude) * 100;
        else
            frontObstacle = 0;

        if (hitFrontRight)
            rightObstacle = 100 - (rightInfo.distance / right.magnitude) * 100;
        else
            rightObstacle = 0;

        if (hitFrontLeft)
            leftObstacle = 100 - (leftInfo.distance / left.magnitude) * 100;
        else
            leftObstacle = 0;


        if (frontObstacle > leftObstacle && frontObstacle > rightObstacle)
            transform.Rotate(Vector3.up, 1);

        if (leftObstacle > rightObstacle && leftObstacle > frontObstacle)
            transform.Rotate(Vector3.up, 1);

        if (rightObstacle > leftObstacle && rightObstacle > frontObstacle)
            transform.Rotate(Vector3.up, -1);




    }

    void drawRays()
    {

        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), forward, Color.green);
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), right, Color.blue);
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), left, Color.red);
    }

    void getDirections()
    {

        forward = transform.TransformDirection(Vector3.forward) * distance;
        right = transform.TransformDirection(Vector3.Normalize(new Vector3(1, 0, 2))) * distance;
        left = transform.TransformDirection(Vector3.Normalize(new Vector3(-1, 0, 2))) * distance;
    }

    void followTarget(Vector3 direction, Transform target)
    {
        direction = target.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
    }

    void Equip()
    {

        weapon.transform.parent = HookRightHand.transform;
       

    }
    void Unequip()
    {

        weapon.transform.parent = HookBack.transform;

            

    }
    void ResetTrig()
    {
        anim.ResetTrigger("Arm/Disarm");

        anim.ResetTrigger("Attack 0");





    }

}
