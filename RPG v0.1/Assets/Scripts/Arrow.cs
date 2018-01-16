using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    //Public Vars
        
    public Vector3 eulerRotation;

    public float speed ;

    //Private Vars
    public Vector3 mousePosition,prmousePosition;
   
    public Camera cam;
    public float  previousXPosition, previousYPosition,previousZPosition,rotationX,rotationY;
    void Start()
    {
        previousXPosition = Input.mousePosition.x;
        previousYPosition = Input.mousePosition.y;
        previousZPosition = Input.mousePosition.z;
        speed = 0.4f;
        

    }


    void FixedUpdate()
    {

       

        //Grab the current mouse position on the screen
        mousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - cam.transform.position.z));
        prmousePosition = cam.ScreenToWorldPoint(new Vector3(previousXPosition, previousYPosition, previousZPosition - cam.transform.position.z));
        //Rotates toward the mouse
        if (Mathf.Atan2((mousePosition.y - prmousePosition.y), (mousePosition.x - prmousePosition.x)) * Mathf.Rad2Deg != 0)
        {
            Quaternion tmp = Quaternion.LookRotation(cam.transform.forward);
            rotationX = tmp.eulerAngles.x;
            rotationY = tmp.eulerAngles.y;
            Quaternion angle = Quaternion.Euler(rotationX,
                rotationY,
                Mathf.Atan2((mousePosition.y - prmousePosition.y), (mousePosition.x - prmousePosition.x)) * Mathf.Rad2Deg);
            
              eulerRotation = angle.eulerAngles;
            if (eulerRotation.z>30 && eulerRotation.z<90)

                angle = Quaternion.Euler(rotationX,rotationY,60);

            if (eulerRotation.z > 90 && eulerRotation.z < 150)

                angle = Quaternion.Euler(rotationX, rotationY, 120);

            if (eulerRotation.z < 30 || eulerRotation.z > 330)

                angle = Quaternion.Euler(rotationX, rotationY, 0.1f);

            if (eulerRotation.z > 150 && eulerRotation.z < 210)

                angle = Quaternion.Euler(rotationX, rotationY,180);

            if (eulerRotation.z > 210 && eulerRotation.z < 270)

                angle = Quaternion.Euler(rotationX, rotationY, 240);

            if (eulerRotation.z > 270 && eulerRotation.z < 330)

                angle = Quaternion.Euler(rotationX, rotationY, 300);

            transform.rotation = Quaternion.Lerp(transform.rotation,angle,speed);
          

        }
        if ((Input.mousePosition.x!=previousXPosition)|| (Input.mousePosition.y != previousYPosition) || (Input.mousePosition.z != previousZPosition) ) {
            previousXPosition = Input.mousePosition.x;
            previousYPosition = Input.mousePosition.y;
            previousZPosition = Input.mousePosition.z;
            
        }

    }
    





}
     
 