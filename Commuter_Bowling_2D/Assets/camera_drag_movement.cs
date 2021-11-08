using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_drag_movement : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private Vector3 movement;
    private Vector3 resetPos;

    private float zDistance = -10f;
    public float moveSpeed = 3f;
    public float vertLimit = 0.6f;

    private Camera cam;
    private float targetZoom;
    public float zoomFactor = 6;
    public float zoomLerp = 10;

    public GameObject pClown;
    public GameObject bBallCamRef;

    private void Start() {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;

        resetPos = Camera.main.transform.position;

        if(moveSpeed <= 0) {
            moveSpeed = 3;
            Debug.LogWarning("Camera drag speed was lower or equal to 0!");
        }
        if(!rb2d) {
            rb2d = GetComponent<Rigidbody2D>();
        }
        if (!pClown) {
            pClown = GameObject.FindGameObjectWithTag("Player");
        }
        if (!bBallCamRef) {
            bBallCamRef = GameObject.FindGameObjectWithTag("bowlingBall");
        }
        transform.position = new Vector3(pClown.transform.position.x, pClown.transform.position.y, zDistance);
    }

    private void LateUpdate() {
        //camera zoom
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 4f, 12f);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomLerp);


        //camera movement
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), zDistance);
        if (movement.x != 0 && movement.y != 0) {
            movement.x *= vertLimit;
            movement.y *= vertLimit;
        }

        rb2d.AddForce(movement, ForceMode2D.Impulse);

        //rb2d.velocity = movement * moveSpeed; //fallback code

        if (Input.GetKeyDown(KeyCode.C)) {//reset the camera to its spawn position
            Camera.main.transform.position = new Vector3(pClown.transform.position.x, pClown.transform.position.y, zDistance);
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            Camera.main.transform.position = new Vector3(bBallCamRef.transform.position.x, bBallCamRef.transform.position.y, zDistance);
        }

    }
}
