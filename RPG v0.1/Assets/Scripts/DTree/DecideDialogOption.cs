using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecideDialogOption : DTDecision  {


    int attitude;
    public DecideDialogOption(int attitude,params DTNode[] list)
    {
        this.attitude = attitude;
        childNodes.AddRange(list);

    }

    public override DTNode getBranch()
    {
        if(attitude<=25)
        return childNodes[2];

        if (attitude <= 75 && attitude>25)
            return childNodes[1];

        if (attitude > 75)
            return childNodes[0];

        else return childNodes[0];

    }
}
