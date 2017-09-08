using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour {

    int targetIndex;


    string state = "patrol";
    
    float rotSpeed = 1f;


    float accuracyWP = 5.0f;

    Vector3 currentTarget;
    Vector3 yOffset = new Vector3(0, 1, 0);
    RaycastHit frontInfo, rightInfo, leftInfo;

    Vector3 forward, right, left; 

    Animator anim;

    public Transform target;
    public Transform[] waypoints;
    public Transform player;
    public LayerMask ignoreHitMask;
    public int currentWP;

    public float frontObstacle, rightObstacle, leftObstacle;

    public bool hitFront, hitFrontRight, hitFrontLeft;


    public float farDistance = 10.0f;

   
    
   
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
       


    }

   
   
    // Update is called once per frame
    void Update()
    {

        Vector3 direction = target.position - transform.position;

        forward= transform.TransformDirection(Vector3.forward) * farDistance;
        right = transform.TransformDirection(Vector3.Normalize(new Vector3(1, 0, 2))) * farDistance;
        left = transform.TransformDirection(Vector3.Normalize(new Vector3(-1, 0, 2))) * farDistance;




       


        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), forward, Color.green);
        
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), right, Color.blue);
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), left, Color.red);

        direction.y = 0;



        float angle = Vector3.Angle(direction, transform.forward);

        switch (state)
        {
            case "patrol":
                if (waypoints.Length > 0)
                {
                    target = waypoints[currentWP];
                    anim.SetBool("Walk", true);
                    if (Vector3.Distance(target.position, transform.position) < accuracyWP)
                    {
                        currentWP++;
                        if (currentWP+1 > waypoints.Length)
                            currentWP = 0;

                    }
                    

                  

                    hitFront = Physics.Raycast(transform.position + yOffset, forward, out frontInfo, forward.magnitude) && frontInfo.collider.tag == "enviroment";
                    //Box version
                    /* Physics.BoxCast(transform.position + yOffset,
                        new Vector3(0.5f,0.5f,forward.magnitude/2),
                        forward,out frontInfo, transform.rotation,forward.magnitude) && frontInfo.collider.tag == "enviroment";*/
                    hitFrontRight = Physics.Raycast(transform.position + yOffset, right, out rightInfo, right.magnitude) && rightInfo.collider.tag == "enviroment";
                    hitFrontLeft = Physics.Raycast(transform.position + yOffset, left, out leftInfo, left.magnitude) && leftInfo.collider.tag == "enviroment";

                    if (hitFront || hitFrontLeft || hitFrontRight)
                        avoidObstacles();
                    else
                    {
                        direction = target.position - transform.position;
                        transform.rotation = Quaternion.Slerp(transform.rotation,
                            Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
                    }




                }

                if (Vector3.Distance(player.position, transform.position) < 20 && angle < 60)

                    state = "pursuit";





                break;
            case "pursuit":

                anim.SetBool("Chase", true);
                anim.SetBool("Attack", false);

                if (hitFront || hitFrontLeft || hitFrontRight)
                    avoidObstacles();
                else
                {
                    direction = player.position - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
                }

              /*  direction = player.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);*/


                if (Vector3.Distance(transform.position, player.position) < 2)
                    state = "attack";


                if (Vector3.Distance(player.position, transform.position) > 20)
                {
                    anim.SetBool("Attack", false);
                    anim.SetBool("Chase", false);
                    target = waypoints[currentWP];
                  
                    state = "patrol";
                }
                break;

            case "attack":
                transform.rotation = Quaternion.Slerp(transform.rotation,
                   Quaternion.LookRotation(player.transform.position), rotSpeed * Time.deltaTime);
                anim.SetBool("Attack", true);
                anim.SetBool("Chase", false);
                anim.SetBool("Walk", false);

                if (Vector3.Distance(transform.position, player.position) > 2)
                {

                    state = "pursuit";
                }
                break;
        }



    }

    void avoidObstacles() {



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
            transform.Rotate(Vector3.up,1);

        if (leftObstacle > rightObstacle && leftObstacle > frontObstacle)
            transform.Rotate(Vector3.up, 1);
            
        if(rightObstacle > leftObstacle && rightObstacle > frontObstacle)
            transform.Rotate(Vector3.up, -1);

       






        /*  if (hitFront)
              if (hitFrontLeft)
                  if (hitFrontRight)

                  transform.rotation = Quaternion.Slerp(transform.rotation,
                   Quaternion.AngleAxis(180, Vector3.up), rotSpeed * Time.deltaTime);
                  else

               transform.rotation = Quaternion.Slerp(transform.rotation,
              Quaternion.AngleAxis(180, Vector3.up), rotSpeed * Time.deltaTime);
              else
              if (hitFrontRight)

              transform.rotation = Quaternion.Slerp(transform.rotation,
              Quaternion.AngleAxis(-180, Vector3.up), rotSpeed * Time.deltaTime);
              else

           transform.rotation = Quaternion.Slerp(transform.rotation,
              Quaternion.AngleAxis(180, Vector3.up), rotSpeed * Time.deltaTime);

          else if (hitFrontLeft)
              if (hitFrontRight)
                  return;
              else

           transform.rotation = Quaternion.Slerp(transform.rotation,
              Quaternion.AngleAxis(180, Vector3.up), rotSpeed * Time.deltaTime);
          else if (hitFrontRight)
              if (hitFrontLeft)
                  return;
              else

          transform.rotation = Quaternion.Slerp(transform.rotation,
             Quaternion.AngleAxis(-180, Vector3.up), rotSpeed * Time.deltaTime);

      */


    }
   
}
