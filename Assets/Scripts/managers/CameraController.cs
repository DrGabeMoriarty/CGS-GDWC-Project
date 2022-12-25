using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float time;
    private float pos_x;
    private float pos_y;
    private Vector3 velocity = Vector3.zero;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position,new Vector3(transform.position.x,pos_y,transform.position.z),ref velocity, time);
    }

    public void Change_Rooms(Transform new_room){
        pos_x = new_room.position.x;
        pos_y = new_room.position.y;
    }
}
