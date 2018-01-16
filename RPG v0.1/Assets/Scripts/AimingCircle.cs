using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AimingCircle : MonoBehaviour {

	
	

    public int segments;  //number of segments of circle
    public float outerRadius;
    public float innerRadius;
    public GameObject[] parts;
    public GameObject indicator;
    public Camera cam;
    public float choosePositionX,choosePositionY;
    public bool ifVisible;
    public float step;     //speed of  growing/shrinking animation
    

    
    LineRenderer line;
    Vector3 indicatorSize,indicatorOffset;
    Animator anim;
    RaycastHit item;
    RPGStats charStats;
    bool hit;
    float pickAttackDirection;





    void Start()
    {
        
        segments = 30;
        outerRadius = 0.020f;
        innerRadius = 0.008f;
        ifVisible = false;
        step = 0.003f;

        anim = GameObject.Find("knight").GetComponent<Animator>();
        charStats = GameObject.Find("knight").GetComponent<RPGStats>();
        



    }

    private void Update()
    {
        
        drawParts();
        transform.rotation = Quaternion.LookRotation(cam.transform.forward);
        indicatorSize.Set(innerRadius*0.7f, innerRadius * 0.7f, innerRadius * 0.7f);
        indicator.transform.localScale = indicatorSize;
        
        indicator.SetActive(ifVisible);
        choosePositionX = indicator.transform.localPosition.x / outerRadius;
        choosePositionY = indicator.transform.localPosition.y / outerRadius;


        pickAttackDirection = 500 / (0.3f * charStats.agility + 0.7f * charStats.swordFightAbility) * 10;


        hit =Physics.CapsuleCast(transform.position,transform.position+transform.forward*10,2*outerRadius,transform.forward,out item, 10f) && item.collider.tag != "ground";

        /*if (hit)
            Debug.Log(item.transform.name);*/

        if (Vector3.Distance(indicator.transform.localPosition, new Vector3(0f,0f,0f)) <= ((outerRadius - indicatorSize.x/2) *charStats.stamina/charStats.maxStamina))
        {
            
            indicator.transform.Translate(new Vector3(Input.GetAxis("Mouse X")/pickAttackDirection,
                Input.GetAxis("Mouse Y")/pickAttackDirection, 0f));

           


            GameObject.Find("knight").GetComponentInChildren<CameraController>().turnSpeed = 0.1f; 
        }
        if (Vector3.Distance(indicator.transform.localPosition, Vector3.zero) > ((outerRadius - indicatorSize.x/2) * charStats.stamina / charStats.maxStamina))
        {
            GameObject.Find("knight").GetComponentInChildren<CameraController>().turnSpeed = 4f;

            indicator.transform.localPosition = indicator.transform.localPosition.normalized * ((outerRadius - indicatorSize.x/2) * charStats.stamina / charStats.maxStamina);


        }
            

    }

   

    void drawParts()
    {
        float xOuter, yOuter;
        float xInner,yInner;
        float z = 0f;

        float angle = 0f;

        
         



        for (int i = 0; i < parts.Length; i++)
        {
            line = parts[i].GetComponent<LineRenderer>();
            line.positionCount=2*segments + 3;
            line.useWorldSpace = false;

            angle = 60 * i +5;
            for (int j = 0; j < (segments + 1); j++)
            {
                xOuter = Mathf.Sin(Mathf.Deg2Rad * angle) * outerRadius;
                yOuter = Mathf.Cos(Mathf.Deg2Rad * angle) * outerRadius;

                line.SetPosition(j, new Vector3(xOuter, yOuter, z));


                angle += (50f / segments);
            }
            for (int j = 0; j < (segments + 1); j++)
            {
                xInner = Mathf.Sin(Mathf.Deg2Rad * angle) * innerRadius;
                yInner = Mathf.Cos(Mathf.Deg2Rad * angle) * innerRadius;

                line.SetPosition(j + segments + 1, new Vector3(xInner, yInner, z));
                angle -= (45f / segments);
            }
            angle = 60 * i + 5;
            line.SetPosition(2*segments+2, new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle) * outerRadius, Mathf.Cos(Mathf.Deg2Rad * angle) * outerRadius, z));


        }

    
        }


    public IEnumerator inFightMode() {

        for(outerRadius=0.020f; outerRadius < 0.055f; outerRadius+=step)
        {
            yield return null;
        }
        for (innerRadius = 0.008f; innerRadius < 0.015f; innerRadius += step)
        {
            yield return null;
        }



    }
    public IEnumerator outFightMode() {


        for (innerRadius = 0.015f; innerRadius > 0.008f; innerRadius -= step)
        {
            yield return null;
        }
        for (outerRadius = 0.055f; outerRadius > 0.020f; outerRadius -= step)
        {
            yield return null;
        }
       
    }

    /* public IEnumerator returnToCenter()
    {
        float distance = Vector3.Distance(indicator.transform.localPosition, Vector3.zero);
        for (; Vector3.Distance(indicator.transform.localPosition, Vector3.zero) > 0.01f;
            
            indicator.transform.localPosition += (Vector3.zero - indicator.transform.localPosition).normalized * 0.01f)
        yield return null;

        for (; Vector3.Distance(indicator.transform.localPosition, Vector3.zero) < distance;

            indicator.transform.localPosition += (indicator.transform.localPosition- Vector3.zero ).normalized * 0.01f)
            yield return null;
    }*/


}
