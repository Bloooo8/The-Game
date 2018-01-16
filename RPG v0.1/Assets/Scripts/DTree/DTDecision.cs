using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DTDecision : DTNode
{
    protected List<DTNode> childNodes=new List<DTNode>();


    public DTDecision(params DTNode[] list)
    {
        childNodes.AddRange(list);

    }

    public virtual DTNode getBranch()
    {
        return childNodes[0];
    }

    public override DTNode Decide()
    {
        DTNode branch = getBranch();
        return branch.Decide();
    }


}
