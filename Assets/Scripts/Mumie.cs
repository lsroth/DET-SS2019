using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Mumie : MonoBehaviour
{
    public float speed = 1f;
    private Vector3 playerPosition;
   // int surfaceHeight = Utils.GenerateHeightMountains(worldX, worldZ);
    // Start is called before the first frame update
    void Start()
    {
        //Camposition = CameraController.position;
        //playerPosition = RigidbodyFirstPersonController.FPposition;
        // Vector3 playerPosition = FirstPersonController.m_Camera.transform.localPosition;
        // playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        // player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = World.playerPosition;
        lookatme();
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
    }
    void lookatme(){
        Debug.Log(playerPosition);
        // Vector3 dir = playerPosition - this.transform.position;
        // this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation (dir), 0.1f);
        this.transform.LookAt(playerPosition);
        this.transform.Rotate(0.0f, 270.0f, 0.0f, Space.Self);
    }
}
