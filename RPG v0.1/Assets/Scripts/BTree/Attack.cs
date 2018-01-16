using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Task
{

    Animator anim;

    RPGStats stats;

    SimpleAI ai;

    


    public Attack(SimpleAI ai, Animator anim)
    {
        this.ai = ai;
        this.anim = anim;

    }




     override public bool Run()
     {
       
            Debug.Log("Attack");

            anim.SetBool("Movement", false);
            anim.SetFloat("Idle", 1f);

            anim.SetTrigger("Attack 0");
           
        


         if (anim.GetBool("Already_hit"))
             return true;

         return false;

     }
   /* public override IEnumerator Run()
    {
       

        isFinished = false;
        Debug.Log("Attack");

        anim.SetBool("Movement", false);

        anim.SetFloat("Idle", 1f);
        anim.SetTrigger("Attack 0");

        
        return base.Run();

    }*/

}
