using UnityEngine;
using System.Collections;

public class CircleAround : Task
{

    SimpleAI ai;
    Animator anim;
    Transform player;
    float forward, side;

    public CircleAround(SimpleAI ai, Animator anim, Transform player)
    {
        this.ai = ai;
        this.anim = anim;
        this.player = player;

    }


     public override bool Run()
     {
        forward = anim.GetFloat("Forward");
        side = anim.GetFloat("Side");

         Debug.Log("CircleAround");
         ai.moveAI.followTarget(ai.player);
         anim.SetBool("Movement", true);
        //ai.smoothValueAnimator("Side", 1f, 0.1f);
        if (forward > 0)
            forward -= 0.05f;
        if (forward < 0)
            forward += 0.05f;
        if(ai.fightDir>0)
        if (side<1)
            side += 0.05f;

        if (ai.fightDir < 0)
            if (side > -1)
                side -= 0.05f;


        anim.SetFloat("Side", side);
        anim.SetFloat("Forward", forward);
       // ai.smoothValueAnimator("Forward", 0f, 0.1f);



        return false;



     }

  /*  public override IEnumerator Run()
    {
        

        isFinished = false;
        Debug.Log("CircleAround");

        ai.moveAI.followTarget(ai.player);
        anim.SetBool("Movement", true);

        ai.smoothValueAnimator("Side", 1f, 0.1f);
        //anim.SetFloat("Side", 0f);
        //anim.SetFloat("Forward", 1f);
        ai.smoothValueAnimator("Forward", 0f, 0.1f);
        yield return base.Run();

    }*/
}
