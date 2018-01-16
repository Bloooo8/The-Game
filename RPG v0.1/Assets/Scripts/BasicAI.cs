using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]

public class BasicAI : MonoBehaviour {

    public Transform target;    
    public Transform[] waypoints;
    public Transform player;
    public Vector3[] path;
    public LayerMask ignoreHitMask;
    public string state = "patrol";



    int targetIndex;
    public bool hit;  
    Animator anim;
    Vector3 currentTarget;
    RaycastHit hitInfo;
    int currentWP = 0;
    float rotSpeed = 1f;
    float accuracyWP = 2.0f;


    // Use this for initialization
    void Start () {
        anim=GetComponent<Animator>();
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);


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
            if (Vector3.Distance(currentTarget, transform.position)<2.0)
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
    // Update is called once per frame
    void Update () {

        Vector3 direction = target.position - transform.position;
        
        direction.y = 0;

        float angle = Vector3.Angle(direction, transform.forward);

        switch (state)
        {
            case "patrol":
                if (waypoints.Length > 0)
                {
                    
                    anim.SetBool("Walk", true);
                    if (Vector3.Distance(target.position, transform.position) < accuracyWP )
                    {
                        currentWP++;
                        
                        if (currentWP > waypoints.Length)
                            currentWP = 0;

                        target = waypoints[currentWP];

                         PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                    }
                  /*  if (Physics.Raycast(transform.position, Vector3.forward, out hitInfo, 5.0f))
                        if (hitInfo.collider.tag == "enviroment")
                        
                         PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);*/
                        
                      

                   

                    direction = currentTarget - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
                }

                if (Vector3.Distance(player.position, transform.position) < 20 && angle < 60 )
                
                    state = "pursuit";





                    break;
            case "pursuit":

                anim.SetBool("Chase", true);
                anim.SetBool("Attack", false);


                PathRequestManager.RequestPath(transform.position, player.position, OnPathFound);
               direction = currentTarget - transform.position;
              
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);

               
                if (Vector3.Distance(transform.position, player.position) < 2)
                    state = "attack";
                

                if (Vector3.Distance(player.position, transform.position) >20)
                {
                    anim.SetBool("Attack", false);
                    anim.SetBool("Chase", false);
                    target = waypoints[currentWP];
                    PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                    state = "patrol";
                }
                break;

            case "attack":
                transform.rotation = Quaternion.Slerp(transform.rotation,
                   Quaternion.LookRotation(player.transform.position-transform.position), rotSpeed * Time.deltaTime);
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
