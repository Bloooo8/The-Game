using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour {

    public int quality;
    public int sharpness;
    public int weight;
    public float range;

    Animator anim,otherAnim;

    AudioSource audio;

    AnimatorStateInfo state,otherState;
    bool hit;
    
    RaycastHit hitInfo;

    private void Start()
    {
         anim = GetComponent<Transform>().GetComponentInParent(typeof(Animator)) as Animator;

        anim.SetFloat("Block_speed", 1f);

        audio = GetComponent<AudioSource>();


    }

    private void Update()
    {
        state = anim.GetCurrentAnimatorStateInfo(0);

        hit = Physics.Raycast(transform.position, transform.up, out hitInfo, range) &&
                       (hitInfo.collider.tag == "player" || hitInfo.collider.tag == "character")&&
                       hitInfo.transform!=anim.transform;

        drawRays();

      /*  if (hit)
        {

            otherAnim = hitInfo.collider.GetComponent<Transform>().GetComponentInParent(typeof(Animator)) as Animator;

            otherState = otherAnim.GetCurrentAnimatorStateInfo(0);



            if (state.IsName("Attack_Right") || state.IsName("Attack_Left"))
            {
                if (!anim.GetBool("Already_hit"))
                {
                    dealDamage(hitInfo.collider.GetComponent<Transform>());


                    anim.SetBool("Already_hit", true);
                }
                

                if (hitInfo.collider.tag == "weapon" && otherState.IsName("Block"))
                {
                    Debug.Log("block");

                    anim.SetFloat("Block_speed", -1f);



                    // anim.SetTrigger("Blocked");

                }
            }

        }*/
    }
    public void dealDamage(Transform target)
    {
        Debug.Log("hit");

        audio.Play();

        target.GetComponent<Animator>().SetTrigger("Hit");

        target.GetComponent<Animator>().SetFloat("Hit_Direction",-anim.GetFloat("Idle")) ;



        target.GetComponent<RPGStats>().health -= 0.01f * quality +
            0.03f * sharpness;
    }

     private void OnTriggerEnter(Collider other)
     {
          otherAnim = GetComponent<Transform>().GetComponentInParent(typeof(Animator)) as Animator;

          otherState = otherAnim.GetCurrentAnimatorStateInfo(0);



         if (state.IsName("Attack_Right") || state.IsName("Attack_Left"))
         {
             if( (other.tag == "character" || other.tag == "player")&& other.transform!=anim.transform)
             {

                if (!anim.GetBool("Already_hit"))
                {
                    dealDamage(other.GetComponent<Transform>());


                    anim.SetBool("Already_hit", true);
                }
                
             }

             if (other.tag == "weapon" &&  otherState.IsName("Block"))
             {
                 Debug.Log("block");

                 anim.SetFloat("Block_speed", -1f);



                 // anim.SetTrigger("Blocked");

             }
         }
     }

    public void drawRays()
    {

        Debug.DrawRay(transform.position,transform.up.normalized*range, Color.green);
    }

}

