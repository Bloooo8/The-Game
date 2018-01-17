using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingAI : MonoBehaviour
{


    public Transform target;

    public bool onPosition = false;


    

    public float rotSpeed = 3f;



    float accuracyWP = 5.0f;

    Animator anim;

    EnemyGroup group;

    WeaponStats weapon;

    /*   Obstacle Avoidance  */

    Vector3 yOffset = new Vector3(0, 1, 0);   //offset of rays origin

    RaycastHit frontInfo, rightInfo, leftInfo;  //informations from rays

    Vector3 forward, right, left;    // directions of rays

    public float frontObstacle, rightObstacle, leftObstacle;   // information about how close is obstacle (1 - 100)

    public bool hitFront, hitFrontRight, hitFrontLeft;    // position of obstacle relative to character

    public float distance = 10.0f;                   // length of middle ray


    /* Collision Avoidance */

    List<Transform> possibleCollisions = new List<Transform>();   // list of characters in collission range

    Transform nearestCollision;   // character that is the most possible to collide with

    float collisionRadius = 3f;

    float shortestTimeToCollision = 2f;

    float timeToCollision;

    float lowestDistanceToCollision;

    float relativeSpeed;

    float minSeparation, collisionMinSeparation;

    float dist, collisionDist;

    public bool isColliding = false;



    Vector3 relativePosition, relativeVelocity;
    Vector3 collisionRelativePosition, collisionRelativeVelocity;


    //     A* pathfinding        //

    public Vector3[] path;

    int targetIndex;

    Vector3 currentTarget, direction;
    new string tag = "enviroment";

    float speed = 0f;

    SimpleAI ai;




    private void Start()
    {
        anim = GetComponent<Animator>();

        group = GetComponentInParent<EnemyGroup>();

        weapon= GetComponentInChildren<WeaponStats>();

        ai = GetComponent<SimpleAI>();

        PathManager.RequestPath(transform.position, target.position, OnPathFound);

        if (group == null)
            onPosition = true;

    }


    private void Update()
    {

        

         direction = target.position - transform.position;

        getDirections();

        drawRays();

        direction.y = 0;










        
        hitFront = Physics.Raycast(transform.position + yOffset, forward, out frontInfo, forward.magnitude) &&
                       frontInfo.collider.tag == tag;

        /*  hitFront=Physics.CapsuleCast(transform.position + yOffset, transform.position + yOffset + forward * forward.magnitude, 0.1f, forward, out frontInfo, forward.magnitude) &&
              frontInfo.collider.tag == "enviroment";*/


        hitFrontRight = Physics.Raycast(transform.position + yOffset + transform.right * transform.GetComponent<CapsuleCollider>().radius, right, out rightInfo, right.magnitude) &&
            rightInfo.collider.tag == tag;

        /*   hitFrontRight= Physics.CapsuleCast(transform.position + yOffset+transform.right * transform.GetComponent<CapsuleCollider>().radius,
              transform.position + yOffset + transform.right * transform.GetComponent<CapsuleCollider>().radius + right.normalized * right.magnitude, 0.1f, right, out rightInfo, right.magnitude) &&
               rightInfo.collider.tag == "enviroment";*/

        hitFrontLeft = Physics.Raycast(transform.position + yOffset + transform.right * -transform.GetComponent<CapsuleCollider>().radius, left, out leftInfo, left.magnitude) &&
            leftInfo.collider.tag == tag;

        /* hitFrontLeft = Physics.CapsuleCast(transform.position + yOffset + transform.right * -transform.GetComponent<CapsuleCollider>().radius,
       transform.position + yOffset + transform.right * -transform.GetComponent<CapsuleCollider>().radius + left.normalized * left.magnitude, 0.1f, left, out leftInfo, left.magnitude) &&
        leftInfo.collider.tag == "enviroment";*/
    }


    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        currentTarget = path[0];
        while (true)
        {
            if (Vector3.Distance(currentTarget, transform.position) < 2.0)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    targetIndex = 0;
                    path = new Vector3[0];
                    yield break;
                }
                currentTarget = path[targetIndex];
            }


            yield return null;

        }
    }

    public void avoidObstacles()
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

    public void getDirections()
    {

        forward = transform.TransformDirection(Vector3.forward) * distance;
        right = transform.TransformDirection(Vector3.Normalize(new Vector3(1, 0, 2))) * distance;
        left = transform.TransformDirection(Vector3.Normalize(new Vector3(-1, 0, 2))) * distance;
    }

    public void followTarget( Transform target)
    {

        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction,transform.up), rotSpeed * Time.deltaTime);
    }

    public void followTarget(Vector3 target)
    {

        Vector3 direction = target - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction, transform.up), rotSpeed * Time.deltaTime);
    }

    public void runFromTarget(Transform target)
    {

        Vector3 direction =  transform.position - target.position;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction, transform.up), rotSpeed * Time.deltaTime);

        anim.SetBool("Movement", true);
        anim.SetFloat("Speed", 0.9f);
    }

    public void moveToTargetArmed(Vector3 position)
    {
        anim.SetBool("Movement",true);
        Vector3 direction = position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction, transform.up), rotSpeed * Time.deltaTime);


    }

    public void circleFormation(Transform target)
    {
        int index=0;

        for(int  i = 0; i < group.group.Length; i++)
        {
            if (transform == group.group[i])
                index = i;

        }

        Vector3 targetPoint=Vector3.zero;
        if (index == 0)
            targetPoint = target.position + target.forward * 5;
        if(index==1)
            targetPoint = target.position + target.right * 5;
        if (index == 2)
            targetPoint = target.position - target.forward * 5;
        if (index == 3)
            targetPoint = target.position - target.right * 5;


        moveToTargetArmed(targetPoint);

        if (Vector3.Distance(transform.position, targetPoint) < 0.5f)
        {
            anim.SetBool("Movement", false);
            onPosition = true;


        }

        else onPosition = false;
            

    }


    public void avoidCollision()
    {





        if (!isColliding)

            foreach (Transform collision in possibleCollisions)
            {


                relativePosition = collision.position - transform.position;

                relativeVelocity = collision.GetComponent<Rigidbody>().velocity - transform.GetComponent<Rigidbody>().velocity;

                relativeSpeed = relativeVelocity.magnitude;

                timeToCollision = Vector3.Dot(relativePosition, relativeVelocity) / relativeSpeed * relativeSpeed;

                dist = relativePosition.magnitude;

                minSeparation = dist - relativeSpeed * shortestTimeToCollision;



                if (minSeparation > collisionRadius || timeToCollision < 0)

                    continue;



                if (timeToCollision > 0 && timeToCollision < shortestTimeToCollision)
                {
                    shortestTimeToCollision = timeToCollision;
                    nearestCollision = collision;
                    collisionMinSeparation = minSeparation;
                    collisionDist = dist;
                    collisionRelativePosition = relativePosition;
                    collisionRelativeVelocity = relativeVelocity;




                }



            }

        if (nearestCollision == null)
            return;


        if (collisionMinSeparation <= 0 || collisionDist < collisionRadius)
        {
            relativePosition = nearestCollision.position - transform.position;



        }
        else

        {
            relativePosition = collisionRelativePosition + collisionRelativeVelocity * shortestTimeToCollision;



        }

        /* Avoid nearestCollision */


        if (Vector3.Distance(transform.position, transform.TransformPoint(relativePosition)) < collisionRadius)
        {

            Debug.Log(transform.GetComponent<Rigidbody>().velocity.magnitude);

            if (relativePosition.x > 0)

                transform.Rotate(Vector3.up, -10f);

            else if (relativePosition.x < 0)

                transform.Rotate(Vector3.up, 10f);




        }

        else

            transform.Rotate(Vector3.up, 1f);

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "character" && !other.isTrigger)
            possibleCollisions.Add(other.transform);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "character" && !other.isTrigger)
            possibleCollisions.Remove(other.transform);


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "character" && !collision.collider.isTrigger)
            isColliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "character" && !collision.collider.isTrigger)
            isColliding = false;
    }

    public void drawRays()
    {

        Debug.DrawRay(transform.position + yOffset, forward, Color.green);
        Debug.DrawRay(transform.position + yOffset + transform.right*transform.GetComponent<CapsuleCollider>().radius, right, Color.blue);
        Debug.DrawRay(transform.position + yOffset - transform.right * transform.GetComponent<CapsuleCollider>().radius, left, Color.red);
    }

    public void patrol(Transform[] waypoints, ref int currentWP, ref Transform target)
    {
        if (waypoints.Length > 0)
        {
            
            

            anim.SetBool("Movement", true);
            if (Vector3.Distance(target.position, transform.position) < accuracyWP)
            {
                currentWP++;
                if (currentWP + 1 > waypoints.Length)
                    currentWP = 0;
                target = waypoints[currentWP];
                PathManager.RequestPath(transform.position, target.position, OnPathFound);



            }

            followTarget(currentTarget);
            avoidCollision();
        }
    }

    public void pursuit(Transform target)
    {
        
        if (target.GetComponent<Rigidbody>().velocity.magnitude != 0)
            speed = target.GetComponent<Rigidbody>().velocity.magnitude*0.9f;
        else
            speed = 0.5f;
        anim.SetFloat("Speed", speed);
        anim.SetFloat("Forward", 0.8f);
    }

    public void MoveCloser(Transform target)
    {
        Debug.Log("MoveCloser");

        followTarget(target);
        anim.SetBool("Movement", true);

        ai.smoothValueAnimator("Side",0f,0.1f);
        //anim.SetFloat("Side", 0f);
        //anim.SetFloat("Forward", 1f);
        ai.smoothValueAnimator("Forward", 1f, 0.1f);
    }

    public void MoveAway(Transform target)
    {
        Debug.Log("MoveAway");
        followTarget(target);
        anim.SetBool("Movement", true);
        ai.smoothValueAnimator("Side", 0f, 0.1f);
        ai.smoothValueAnimator("Forward", -1f, 0.1f);
        
        //anim.SetFloat("Side", 0f);
       // anim.SetFloat("Forward", -1f);

    }

    public void MoveSide(Transform target)
    {
        Debug.Log("CircleAround");
        followTarget(target);
        anim.SetBool("Movement", true);
        ai.smoothValueAnimator("Forward", 0, 0.1f);
        ai.smoothValueAnimator("Side", 1f, 0.1f);
        
       // anim.SetFloat("Forward", 0f);
       // anim.SetFloat("Side", 1f);

    }


    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
