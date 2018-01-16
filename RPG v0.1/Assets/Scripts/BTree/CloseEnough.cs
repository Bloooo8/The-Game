using UnityEngine;
using System.Collections;

public class CloseEnough : Task
{

    SimpleAI ai;

    Transform player;

   public  CloseEnough(SimpleAI ai,Transform player)
    {
        this.ai = ai;
        this.player = player;
    }

     private void Awake()
     {
         player = ai.player;
     }
     public override bool Run()
     {



         if (Vector3.Distance(ai.transform.position, player.position) < 2f)
         {

             Debug.Log("CloseEnough");
             return true;
         }
         else {
             Debug.Log("NotCloseEnough");
             return false;
         }

     }

   /* public override IEnumerator Run()
    {
        Debug.Log("CloseEnough");
        isFinished = false;
        TaskState  r = TaskState.RUNNING;
        if (Vector3.Distance(ai.transform.position, player.position) < 1.5f)
        {

            Debug.Log("CloseEnough");
            r=TaskState.SUCCESS;
        }
        else
        {
            Debug.Log("NotCloseEnough");
            r=TaskState.FAILURE;
        }
        SetResult(r);
        yield break;
       
    }*/

}
