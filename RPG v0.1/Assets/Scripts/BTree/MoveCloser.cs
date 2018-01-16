using UnityEngine;

using System.Collections;

public class MoveCloser :Task
{
    SimpleAI ai;
    Animator anim;
    float forward, side;

    public MoveCloser(SimpleAI ai, Animator anim)
    {
        this.ai = ai;
        this.anim = anim;

    }

     private void Awake()
     {

         anim = ai.GetAnimator();
     }

     public override bool Run()
     {
         Debug.Log("MoveCloser");

        forward = anim.GetFloat("Forward");
        side = anim.GetFloat("Side");

        ai.moveAI.followTarget(ai.player);
         anim.SetBool("Movement",true);

        if (forward < 1)
            forward += 0.05f;
        if (forward >1)
            forward -= 0.05f;
        if (side > 0)
            side -= 0.05f;
        if (side < 0)
            side += 0.05f;
      
        anim.SetFloat("Side", side);
        anim.SetFloat("Forward",forward);

         return false;

     }
    /*public override IEnumerator Run()
    {


        isFinished = false;
        Debug.Log("MoveCloser");

        ai.moveAI.followTarget(ai.player);
        anim.SetBool("Movement", true);

        ai.smoothValueAnimator("Side", 0f, 0.1f);
        //anim.SetFloat("Side", 0f);
        //anim.SetFloat("Forward", 1f);
        ai.smoothValueAnimator("Forward", 1f, 0.1f);
        return base.Run();

    }*/
}