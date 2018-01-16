using UnityEngine;
using System.Collections;

public class CanAttack : Task
{

    SimpleAI ai;

    Transform player;

    public CanAttack(SimpleAI ai, Transform player)
    {
        this.ai = ai;
        this.player = player;
    }

   
    public override bool Run()
    {



        if (Mathf.Abs(Vector3.Angle(ai.transform.position-player.transform.position, player.forward)) > 45)
        {

            Debug.Log("CanAttack");
            return true;
        }
        else
        {
            Debug.Log("StillCircle");
            return false;
        }

    }

    

}
