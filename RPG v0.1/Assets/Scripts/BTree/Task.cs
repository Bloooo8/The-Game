using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Task  {

    public List<Task> children=new List<Task>();

    //public TaskState result = TaskState.FAILURE;
   // protected bool isFinished = false;

   // public CoroutinesController instance = CoroutinesController.Instance;

   /* public virtual void SetResult(TaskState r)
    {
        result = r;
        isFinished = true;
    }

    public virtual IEnumerator Run()
    {
        SetResult(TaskState.RUNNING);
        yield break;
    }

    public virtual IEnumerator RunTask()
    {
        yield return instance.StartCoroutine(Run());
    }*/




     public Task(params Task[] list){


             children.AddRange(list);



 }

     public virtual bool Run()
     {
         return false;
     }
}

//public enum TaskState { FAILURE,SUCCESS,RUNNING}
