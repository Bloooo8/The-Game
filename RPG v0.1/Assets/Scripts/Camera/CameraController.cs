using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public float turnSpeed = 4.0f;

    public Transform player;

    private Vector3 offset;

    private Vector3 target;

   

    private Vector3 velocity = Vector3.zero;







    void Start()
    {

        offset = new Vector3(0,  3.5f,  -3.5f);
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (offset.z > 0)

            offset.z -= Input.mouseScrollDelta.y / 10;

        if (offset.z < 0)

            offset.z += Input.mouseScrollDelta.y / 10;

        

        target = player.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head).position;


       

        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) *
            Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") , transform.right)*
            offset;

       
           

       

        transform.position = player.position + offset;

        

      

        transform.LookAt(target);
    }
}