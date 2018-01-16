using UnityEngine;
using System.Collections;

public class Decorator : Task
{

   protected Task child;

    public Decorator(Task child)
    {
        this.child = child;
    }
}
