using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Task {
    public Selector(params Task[] list) : base(list)
    {
    }

    override public bool  Run()
    {

        Debug.Log("Selector");
        foreach (Task c in this.children)
            if (c.Run())
                return true;
        return false;

    }
   /* public override void SetResult(TaskState r)
    {
        if (r == TaskState.SUCCESS)
            isFinished = true;
    }
    public override IEnumerator RunTask()
    {
        foreach (Task t in children)
  
            yield return instance.StartCoroutine(t.RunTask());

        
            
    }*/

}
