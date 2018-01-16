using UnityEngine;
using System.Collections;

public class AwayEnough : Task
{

    SimpleAI ai;

    Transform player;

    public AwayEnough(SimpleAI ai, Transform player)
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



         if (Vector3.Distance(ai.transform.position, player.position) > 2f)
         {

             Debug.Log("AwayEnough");
            
             return true;
         }
         else
         {
             Debug.Log("NotAwayEnough");
             return false;
         }

     }

   /* public override IEnumerator Run()
    {

        isFinished = false;
        TaskState r = TaskState.RUNNING;
        if (Vector3.Distance(ai.transform.position, player.position) > 2.5f)
        {

            Debug.Log("AwayEnough");
            r = TaskState.SUCCESS;
        }
        else
        {
            Debug.Log("NotAwayEnough");
            r = TaskState.FAILURE;
        }
        SetResult(r);
        yield break;

    }*/

}
