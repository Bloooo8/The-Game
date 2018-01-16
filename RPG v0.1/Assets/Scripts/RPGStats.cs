using UnityEngine;
using System.Collections;

public class RPGStats : MonoBehaviour
{


    public float maxHealth=100;

    public float maxStamina=100;

    public float maxArmor;

    public float health;

    public float stamina;

    public float armor;

    public int strength=10;

    public int endurance=10;

    public int agility=10;

    public int intelligence=10;

    public int courage = 50;

    public int level=1;

    public int experience=0;

    public int expToNxtLvl = 100;

    public int swordFightAbility=10;

     
    
    Animator animator;

    AimingCircle aimingCircle;
    public GameObject weapon;


    private void Start()
    {

        animator = GetComponent<Animator>();

        animator.SetBool("Alive", true);

        aimingCircle = GetComponentInChildren<AimingCircle>();

        maxHealth = 50 + 3.5f * strength + 1.5f * endurance;

        maxStamina = 50 + 3.5f * endurance + 1.5f * strength;

        health = maxHealth;

        stamina = maxStamina;

        expToNxtLvl = level * 100;
    }

    private void Update()
    {
        if (health <= 0)
        {
            animator.SetBool("Alive", false);
            
        }
    }

    void SetSwingSpeed()
    {
        animator.speed = 0.8f;
    }

    void SetAttackSpeed()
    {
        animator.speed = 1.5f;
    }

    void ResetSpeed()
    {
        animator.speed = 1f;
    }

    void Equip()
    {



        weapon.transform.parent =animator.GetBoneTransform(HumanBodyBones.RightHand).Find("WeaponHandle").transform;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        SetGlobalScale(weapon.transform, Vector3.one);

        if (transform.tag == "player")
        {

            GetComponentInChildren<AimingCircle>().ifVisible = true;

            StartCoroutine(aimingCircle.inFightMode());
        }



    }
    void Unequip()
    {

        weapon.transform.parent = animator.GetBoneTransform(HumanBodyBones.Chest).Find("WeaponHolster").transform;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        SetGlobalScale(weapon.transform, Vector3.one);

        if (transform.tag == "player")
        {
            GetComponentInChildren<AimingCircle>().ifVisible = false;

            StartCoroutine(aimingCircle.outFightMode());
        }



    }

    public static void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x,
            globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }

    void ResetTrig()
    {
        animator.ResetTrigger("Arm/Disarm");

        animator.ResetTrigger("Attack 0");

        if (animator.GetFloat("Block_speed") == -1)
            animator.SetBool("Blocked",true);

        animator.SetFloat("Block_speed", 1f);

        animator.SetBool("Already_hit",false);

        /* if (transform.tag == "player")
         {


             StopCoroutine(aimingCircle.returnToCenter());
         }*/

    }
}
