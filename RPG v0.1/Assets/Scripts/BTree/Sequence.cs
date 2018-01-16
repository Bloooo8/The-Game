using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Task{
    public Sequence(params Task[] list) : base(list)
    {
    }

     override public bool Run()
     {

         Debug.Log("Sequence");
         foreach (Task c in children)
             if (!c.Run())
                 return false;
         return true;

     }
   /*public override void SetResult(TaskState r)
    {
        if (r == TaskState.FAILURE)
            isFinished = true;
    }
    public override IEnumerator RunTask()
    {
        
        foreach (Task t in children)
            yield return instance.StartCoroutine(t.RunTask());
    }*/
}
