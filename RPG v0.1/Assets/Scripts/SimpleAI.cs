using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour {


     

        
    public string state = "patrol";
    public int fightDir = 1;

    public Transform target;
    public  Transform[] waypoints;
   

    public Transform player;
    
    public int currentWP;

    public float viewAngle;

    float attack_vertical=0, attack_horizontal=0;
   
    Animator anim;

   public  MovingAI moveAI;

    RPGStats stats;

    WeaponStats weapon;

    EnemyGroup group;

    Animator playerAnimator;

    AnimatorStateInfo playerState,animState;

    Task btTreeStrike,btTreePosition;

    DTNode dialogTree;

    DialogueManager manager;

    DialoguesScript dialogue;

    




    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

        moveAI = GetComponent<MovingAI>();

        stats = GetComponent<RPGStats>();

        weapon = GetComponentInChildren<WeaponStats>();

        playerAnimator = player.GetComponent<Animator>();
        
        group = GetComponentInParent<EnemyGroup>();

        dialogue = GetComponent<DialoguesScript>();

        btTreeStrike =
                  new Sequence(new Selector(new CloseEnough(this, player), new MoveCloser(this, anim))
                  , new Sequence(new Selector(new CanAttack(this, player), new CircleAround(this, anim, player)),
                  new Attack(this, anim)));
       
        btTreePosition = new Sequence(new Selector(new AwayEnough(this, player),
                   new Selector(new MoveAway(this, anim, player))), new CircleAround(this, anim, player),btTreeStrike);


        manager = GameObject.Find("UI").GetComponent<DialogueManager>();

        dialogTree = new DecideDialogOption(stats.courage, new ShowDialogueOption(manager.option1),
            new ShowDialogueOption(manager.option2),
            new ShowDialogueOption(manager.option3));



    }


   
   
    // Update is called once per frame
    void Update()
    {

        Vector3 direction = player.position - transform.position;
        

        direction.y = 0;

        viewAngle = Vector3.Angle(direction, transform.forward);

        playerState =playerAnimator.GetCurrentAnimatorStateInfo(0);

        animState = anim.GetCurrentAnimatorStateInfo(0);


        anim.SetFloat("Idle", attack_horizontal);
        anim.SetFloat("Attack_Vertical", attack_vertical);







        switch (state)
        {
            case "patrol":

                if (moveAI.hitFront || moveAI.hitFrontLeft || moveAI.hitFrontRight)
                    moveAI.avoidObstacles();
                else

                moveAI.patrol(waypoints,ref currentWP, ref target);
                                                        

                   

                   
                   




                

                if (Vector3.Distance(player.position, transform.position) < 20 && viewAngle < 60 && viewAngle>-60 
                    && stats.courage<0)

                
                {
                    moveAI.pursuit(player);
                    
                    state = "pursuit";

                }

                if (Vector3.Distance(transform.position, player.position) < 2 && stats.courage > 0)
                {
                    manager.ShowWindow();
                    manager.AssignDialogue(dialogue.getNameOfDialog());
                    state = "talk";

                }


                break;

            case "pursuit":

               

                

                if (moveAI.hitFront || moveAI.hitFrontLeft || moveAI.hitFrontRight)
                    moveAI.avoidObstacles();
                else
                {
                    moveAI.avoidCollision();
                    moveAI.followTarget( player.transform);
                }
                    

            
                if (Vector3.Distance(transform.position, player.position) < 5 && stats.courage<0)
                
                {
                    anim.SetFloat("Speed", 0.0f);
                    anim.SetBool("Movement", false);
                    if (!anim.GetBool("Armed"))
                    {
                        anim.SetTrigger("Arm/Disarm");
                        anim.SetBool("Armed", true);
                    }


                    state = "attack";
                }

                if (Vector3.Distance(transform.position, player.position) < 2 && stats.courage > 0)
                {
                    manager.ShowWindow();
                    manager.AssignDialogue(dialogue.getNameOfDialog());
                    state = "talk";

                }


                    if (Vector3.Distance(player.position, transform.position) > 20)
                
                {
                    if (anim.GetBool("Armed"))
                    {
                        anim.SetTrigger("Arm/Disarm");
                        anim.SetBool("Armed", false);
                    }

                    anim.SetFloat("Speed", 0.0f);
                    
                    target = waypoints[currentWP];

                    state = "patrol";
                }
                break;

            case "attack":


                

               


                btTreeStrike.Run();


                if (Vector3.Distance(transform.position, player.position) > 8)
               
                {

                    anim.SetBool("Movement", true);
                    anim.SetFloat("Speed", 1.0f);


                    state = "pursuit";
                }
                break;

            case "talk":

                moveAI.followTarget(player.transform);
                anim.SetBool("Movement", false);

                
                dialogTree = new DecideDialogOption(stats.courage, new ShowDialogueOption(manager.option1),
           new ShowDialogueOption(manager.option2),
           new ShowDialogueOption(manager.option3));



                dialogTree.Decide();


                if (Vector3.Distance(transform.position, player.position) > 5.0f)
                {
                    target = waypoints[currentWP];

                    state = "patrol";
                }

                if (stats.courage < 0)
                {
                    if (!anim.GetBool("Armed"))
                    {
                        anim.SetTrigger("Arm/Disarm");
                        anim.SetBool("Armed", true);
                    }

                    state = "attack";

                }



                break;
        }



    }


    public void FightTree()
    {

        if (stats.health > stats.maxHealth / 10)

            
            if (!(playerState.IsName("Attack_Left") || playerState.IsName("Attack_Right")))

                if (Vector3.Distance(transform.position, player.position) < weapon.range*2.5)
                {
                    if (viewAngle > -15 && viewAngle < 15)
                        if (!(animState.IsName("Attack_Left") ||
                            animState.IsName("Attack_Right")))
                        {

                            if (playerState.IsName("Block"))
                            {
                                anim.SetBool("Movement", false);

                                StartCoroutine(smoothValueAnimator("Idle", playerAnimator.GetFloat("Idle"), 0.05f));
                                StartCoroutine(smoothValueAnimator("Attack_Vertical", Random.Range(-0.8f, 0.8f), 0.05f));

                               



                                

                                anim.SetTrigger("Attack 0");


                            }

                            else
                            {
                                anim.SetBool("Movement", false);


                                 StartCoroutine(smoothValueAnimator("Idle", Random.Range(-0.8f, 0.8f), 0.05f));
                                 StartCoroutine(smoothValueAnimator("Attack_Vertical", Random.Range(-0.8f, 0.8f), 0.05f));
                                

                                anim.SetTrigger("Attack 0");
                            }





                        }
                        else
                            return;
                    else
                        moveAI.followTarget(player);


                }

                else
                {
                    moveAI.followTarget(player);
                    anim.SetBool("Movement", true);


                    
                    StartCoroutine(smoothValueAnimator("Forward", 1f,0.3f));

                }

            else
            {
                moveAI.followTarget(player);
                anim.SetBool("Movement", true);
                StartCoroutine(smoothValueAnimator("Forward", -1f,0.3f));

            }

        else
        {
            moveAI.runFromTarget(player);
            StartCoroutine(smoothValueAnimator("Forward", 1f,0.3f));
        }
            
    }

    public IEnumerator smoothValue(float value1,float value1Target, float value2, float value2Target)
    {
        if (value1 < value1Target)
            for (; value1 < value1Target; value1 += 0.05f)
                yield return null;

        if (value1 > value1Target)
            for (; value1 > value1Target; value1 -= 0.05f)
                yield return null;

        if (value2 < value2Target)
            for (; value2 < value2Target; value2 += 0.05f)
                yield return null;

        if (value2 > value2Target)
            for (; value2 > value2Target; value2 -= 0.05f)
                yield return null;



    }

    public IEnumerator smoothValueAnimator(string name,float targetValue, float step)
    {
        float value = anim.GetFloat(name);
        if (value< targetValue)
            for (; value < targetValue; value += step)
            {
                anim.SetFloat(name, value);
                yield return null;
            }

        
               

        if (value > targetValue)
            for (; value > targetValue; value -= step)
            {
                anim.SetFloat(name, value);
                yield return null;
            }
    }

   public Animator GetAnimator()
    {
        return anim;
    }

    
   


}
