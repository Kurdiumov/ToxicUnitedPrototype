using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    enum MovingModes
    {
        Player,
        Strategic,
    };

    public Transform LookTarget;

    MovingModes moveMode;

    //Interaction
    public float interactDistance = 3.0f;

    // Control:
    public float speed = 6.0F;
    public float sprintSpeed = 12.0f;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;

    //   Camera:
    public float mouseSensitivity = 100.0f;
    private float rotationX;
    private float rotationY;
    public Camera FirstPersonCamera;
    public Camera StrategicCamera;

    //public Camera minimapCam;
    public float viewRange = 20.0f;
    public Vector3 roomView = new Vector3(0.0f, 90.0f, -6.0f);
    
    public RaycastHit Ceiling { get; private set; }
    public RaycastHit HitItem { get; private set; }
    private List<GameObject> StrategyViewUIList;
    
    void Start()
    {
        StrategicCamera.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        moveMode = MovingModes.Player;
        Cursor.visible = false;
        StrategyViewUIList = new List<GameObject>();

        foreach (var gameObject in GameObject.FindGameObjectsWithTag("StrategyView"))
        {
            StrategyViewUIList.Add(gameObject);
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (moveMode == MovingModes.Player)
        {
            CameraControl();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchControl();
        }
    }

    void CameraControl()
    {
        float mouseX;
        float mouseY;

        mouseX = Input.GetAxis("Mouse X");
        mouseY = -Input.GetAxis("Mouse Y");

        if (moveMode != MovingModes.Strategic)
        {
            rotationX += mouseX * mouseSensitivity * Time.deltaTime;
            rotationY += mouseY * mouseSensitivity * Time.deltaTime;
        }

        //First Person
        if (moveMode == MovingModes.Player)
        {
            if (rotationY > 80)
            {
                rotationY = 80;
            }
            else if (rotationY < -80)
            {
                rotationY = -80;
            }

            Quaternion rotatePlayer = Quaternion.Euler(0.0f, rotationX, 0.0f);
            
            transform.rotation = rotatePlayer;
        }
    }

    void SwitchControl()
    {
        if (moveMode == MovingModes.Player)
        {
            foreach (var gameObject in StrategyViewUIList)
                gameObject.SetActive(true);
            ToggleCamera();
            moveMode = MovingModes.Strategic;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            foreach (var gameObject in StrategyViewUIList)
                gameObject.SetActive(false);
            ToggleCamera();
            moveMode = MovingModes.Player;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void ToggleCursor()
    {
        if (Input.GetButtonDown("Fire2") && Cursor.visible == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetButtonDown("Fire2") && Cursor.visible == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private IEnumerator TurnOffCam()
    {
        GetComponent<Camera>().enabled = false;
        yield return new WaitForSeconds(1.0f);
        GetComponent<Camera>().enabled = true;
        yield return 0;
    }

    private void ToggleCamera()
    {
        if (StrategicCamera.enabled)
        {
            StrategicCamera.enabled = false;
            FirstPersonCamera.enabled = true;
        }
        else if (FirstPersonCamera.enabled)
        {
            StrategicCamera.enabled = true;
            FirstPersonCamera.enabled = false;
        }
        else
        {
            throw new ArgumentException();
        }
    }

    private Camera GetActiveCamera()
    {
        if (StrategicCamera.enabled)
            return StrategicCamera;
        if (FirstPersonCamera.enabled)
            return FirstPersonCamera;
        return null;
    }
}