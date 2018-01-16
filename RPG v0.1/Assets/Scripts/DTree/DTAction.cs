using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTAction : DTNode {


   
    public virtual void DoSomething()
    {

    }

    public override DTNode Decide()
    {
        DoSomething();
        return this;
    }

   
}
