using UnityEngine;
using System.Collections;

public class MoveAway : Task
{

    SimpleAI ai;
    Animator anim;
    Transform player;
    float forward, side;

    public MoveAway(SimpleAI ai, Animator anim, Transform player)
    {
        this.ai = ai;
        this.anim = anim;
        this.player = player;

    }
    private void Awake()
    {
       
        anim = ai.GetAnimator();
    }

    public override bool Run()
    {
        forward = anim.GetFloat("Forward");
        side = anim.GetFloat("Side");

        Debug.Log("MoveAway");

        ai.moveAI.followTarget(ai.player);
        anim.SetBool("Movement", true);

        if (forward >-1)
            forward -= 0.05f;
        if (forward < -1)
            forward += 0.05f;
        if (side > 0)
            side -= 0.05f;
        if (forward < 0)
            side += 0.05f;
        //ai.smoothValueAnimator("Side", 0f, 0.1f);

        // ai.smoothValueAnimator("Forward", -1f, 0.1f);
        anim.SetFloat("Side",side);
       anim.SetFloat("Forward",forward);



        return false;

        

    }

     /*public override IEnumerator Run()
    {
       

        isFinished = false;
        Debug.Log("MoveAway");

        ai.moveAI.followTarget(ai.player);
        anim.SetBool("Movement", true);

        ai.smoothValueAnimator("Side", 0f, 0.1f);
        //anim.SetFloat("Side", 0f);
        //anim.SetFloat("Forward", 1f);
        ai.smoothValueAnimator("Forward", -1f, 0.1f);


        return base.Run();

    }*/
}
