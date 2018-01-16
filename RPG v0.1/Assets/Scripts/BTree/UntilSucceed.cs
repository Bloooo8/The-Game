using UnityEngine;
using System.Collections;

public class UntilSucceed : Decorator
{

    bool result;

    public UntilSucceed(Task child) : base(child)
    {
    }

    public override bool Run()
    {
        while (true)
        {
            result = child.Run();

            if (result)
                break;

        }
        return true;
    }
}
