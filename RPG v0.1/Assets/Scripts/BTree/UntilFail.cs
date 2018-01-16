using UnityEngine;
using System.Collections;

public class UntilFail : Decorator
{

    bool result;

    public UntilFail(Task child) : base(child)
    {
    }

      public override bool Run()
      {
          while(true)
          {
              result = child.Run();

              if (!result)
                  break;

          }
          return true;
      }
   /* public override void SetResult(bool r)
    {
        if (r == true)
            isFinished = true;
    }*/
   /* public override IEnumerator RunTask()
    {

        while (true)
        {
            result = child.Run();
        }
           
        /* foreach (Task t in children)
             yield return coroutinesController.StartCoroutine(t.RunTask());
    }*/
}
