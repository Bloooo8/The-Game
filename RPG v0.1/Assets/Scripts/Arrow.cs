using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    //Public Vars
   
    public float speed;
    
    public Vector3 eulerRotation;

    //Private Vars
    public Vector3 mousePosition,prmousePosition;
   
   public Camera cam;
    public float  prx, pry,prz,rotx,roty;
    void Start()
    {
        prx = Input.mousePosition.x;
        pry = Input.mousePosition.y;
        prz = Input.mousePosition.z;
        



    }


    void FixedUpdate()
    {

       

            //Grab the current mouse position on the screen
            mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - cam.transform.position.z));
        prmousePosition = cam.ScreenToWorldPoint(new Vector3(prx, pry, prz - cam.transform.position.z));
        //Rotates toward the mouse
        if (Mathf.Atan2((mousePosition.y - prmousePosition.y), (mousePosition.x - prmousePosition.x)) * Mathf.Rad2Deg != 0)
        {
            Quaternion tmp = Quaternion.LookRotation(cam.transform.forward);
            rotx = tmp.eulerAngles.x;
            roty = tmp.eulerAngles.y;
            Quaternion angle = Quaternion.Euler(rotx,
                roty,
                Mathf.Atan2((mousePosition.y - prmousePosition.y), (mousePosition.x - prmousePosition.x)) * Mathf.Rad2Deg);
            
              eulerRotation = angle.eulerAngles;
            if(eulerRotation.z>30&&eulerRotation.z<90)
                angle = Quaternion.Euler(rotx,roty,60);
            if (eulerRotation.z > 90 && eulerRotation.z < 150)
                angle = Quaternion.Euler(rotx, roty, 120);
            if (eulerRotation.z < 30 || eulerRotation.z > 330)
                angle = Quaternion.Euler(rotx, roty, 0.1f);
            if (eulerRotation.z > 150 && eulerRotation.z < 210)
                angle = Quaternion.Euler(rotx, roty,180);
            if (eulerRotation.z > 210 && eulerRotation.z < 270)
                angle = Quaternion.Euler(rotx, roty, 240);
            if (eulerRotation.z > 270 && eulerRotation.z < 330)
                angle = Quaternion.Euler(rotx, roty, 300);

            transform.rotation = Quaternion.Lerp(transform.rotation,angle,0.6f);
          

        }
        if ((Input.mousePosition.x!=prx)|| (Input.mousePosition.y != pry) || (Input.mousePosition.z != prz) ) {
            prx = Input.mousePosition.x;
            pry = Input.mousePosition.y;
            prz = Input.mousePosition.z;
            
        }

    }
    





}
     
 