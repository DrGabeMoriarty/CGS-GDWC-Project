using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private GameObject player;
    [SerializeField] private float cameraDistOffset = 10;

    // Update is called once per frame
    void Awake()
    {
        player = GameObject.Find("Player");
    }
    void Update()
    {
        Vector3 playerInfo = player.transform.transform.position;
        mainCamera.transform.position = new Vector3(playerInfo.x, playerInfo.y, playerInfo.z - cameraDistOffset);
    }
}
