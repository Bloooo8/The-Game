using UnityEngine;
using System.Collections;

public class Limit : Decorator
{

    int runSoFar = 0,runLimit;

    public Limit( int limit,Task child) : base(child)
    {
        runLimit = limit;
    }

    public override bool Run()
     {
         Debug.Log("Limit");

         if (runSoFar >= runLimit)
             return false;

         runSoFar++;
         return child.Run();

     }
    /*public override void SetResult(TaskState r)
    {
        if (r == TaskState.SUCCESS)
            isFinished = true;
    }
    public override IEnumerator RunTask()
    {

        if (runSoFar >= runLimit)
            yield break;

        runSoFar++;
       yield return child.RunTask();
        foreach (Task t in children)
            yield return instance.StartCoroutine(t.RunTask());
    }*/
}
