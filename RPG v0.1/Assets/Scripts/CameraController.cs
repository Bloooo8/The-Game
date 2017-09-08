using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public float turnSpeed = 4.0f;
    public Transform player;

    private Vector3 offset;

    private Vector3 target;

    public Vector2 off;

    

    void Start()
    {

        offset = new Vector3(player.position.x, player.position.y + 3.0f, player.position.z -2.5f);
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if(offset.z>0)

            offset.z -= Input.mouseScrollDelta.y/10;

        if(offset.z<0)

            offset.z += Input.mouseScrollDelta.y / 10;

        target = new Vector3(player.position.x, player.position.y + 2.0f, player.position.z);

        if(offset.y>=1 && offset.y<=4)

        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * Quaternion.AngleAxis(Input.GetAxis("Mouse Y") , Vector3.right)*
            offset;

        else
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;

        if (offset.y < 1)
            offset.y = 1;

        if (offset.y > 4)
            offset.y = 4;

        transform.position = player.position + offset;
        transform.LookAt(target);
    }
}