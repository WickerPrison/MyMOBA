using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CameraController : MonoBehaviour
{
    InputManager im;
    TurnManager tm;
    PlayerScript playerScript;
    Camera cam;
    GameMode gameMode;
    float zoom;
    float zoomSpeed = 15;
    Vector2 camMove;
    float moveSpeed = 10;

    private void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        playerScript = GetComponent<PlayerScript>();
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        tm = im.gameObject.GetComponent<TurnManager>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        SetupInputs();
    }

    private void Update()
    {
        if(zoom < 0 && cam.orthographicSize < gameMode.zoomOutCameraBoundary)
        {
            cam.orthographicSize += zoomSpeed * Time.deltaTime;
        }
        else if(zoom > 0 && cam.orthographicSize > gameMode.zoomInCameraBoundary)
        {
            cam.orthographicSize -= zoomSpeed * Time.deltaTime;
        }

        Vector2 camMoveVec2 = camMove * Time.deltaTime * moveSpeed;
        cam.transform.position += new Vector3(camMoveVec2.x, camMoveVec2.y, 0);
        if(cam.transform.position.x > gameMode.xCameraBoundary)
        {
            cam.transform.position = new Vector3(gameMode.xCameraBoundary, cam.transform.position.y, cam.transform.position.z);
        }
        else if(cam.transform.position.x < -gameMode.xCameraBoundary)
        {
            cam.transform.position = new Vector3(-gameMode.xCameraBoundary, cam.transform.position.y, cam.transform.position.z);
        }

        if(cam.transform.position.y > gameMode.yCameraBoundaryPos)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, gameMode.yCameraBoundaryPos, cam.transform.position.z);
        }
        else if(cam.transform.position.y < gameMode.yCameraBoundaryNeg)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, gameMode.yCameraBoundaryNeg, cam.transform.position.z);
        }
    }

    void SetupInputs()
    {
        im.controls[tm.players.IndexOf(playerScript)].CameraControls.Zoom.performed += ctx => zoom = ctx.ReadValue<float>();
        im.controls[tm.players.IndexOf(playerScript)].CameraControls.Move.performed += ctx => camMove = ctx.ReadValue<Vector2>();
        im.controls[tm.players.IndexOf(playerScript)].CameraControls.Move.canceled += ctx => camMove = Vector2.zero;
    }
}
