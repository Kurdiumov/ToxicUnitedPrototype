using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum MovingModes
    {
        FirstPerson,
        Strategic
    };

    public Transform LookTarget;
    public MovingModes moveMode;

    //Camera:
    public Camera FirstPersonCamera;
    public Camera StrategicCamera;
    private readonly float mouseSensitivity = 100.0f;
    private float _rotationX;
    private float _rotationY;

    private List<GameObject> StrategyViewUIList;



    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController charakterController;
    private PlayerController playerController;

    void Start()
    {
        moveMode = MovingModes.FirstPerson;
        StrategicCamera.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StrategyViewUIList = new List<GameObject>();
        charakterController = gameObject.GetComponent<CharacterController>();
        playerController = gameObject.GetComponent<PlayerController>();
        foreach (var gameObj in GameObject.FindGameObjectsWithTag("StrategyView"))
        {
            StrategyViewUIList.Add(gameObj);
            gameObj.SetActive(false);
        }
    }

    void Update()
    {
        if (moveMode == MovingModes.FirstPerson)
        {
            CameraControl();
            PlayerMove();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchControl();
        }
    }

    void PlayerMove()
    {
        if (charakterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        charakterController.Move(moveDirection * Time.deltaTime);

        playerController.speed = 6;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerController.speed = 10;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            charakterController.height = 1;
            playerController.speed = 1;

        }
        else if (charakterController.height < 2)
        {
            charakterController.height += 0.05f;
        }
    }

    void CameraControl()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        if (moveMode != MovingModes.Strategic)
        {
            _rotationX += mouseX * mouseSensitivity * Time.deltaTime;
            _rotationY += mouseY * mouseSensitivity * Time.deltaTime;
        }

        //First Person
        if (moveMode == MovingModes.FirstPerson)
        {
            if (_rotationY > 80)
            {
                _rotationY = 80;
            }
            else if (_rotationY < -80)
            {
                _rotationY = -80;
            }

            Quaternion rotatePlayer = Quaternion.Euler(0.0f, _rotationX, 0.0f);
            transform.rotation = rotatePlayer;
        }
    }

    void SwitchControl()
    {
        if (moveMode == MovingModes.FirstPerson)
        {
            //Switch to Strategic view
            foreach (var gameObject in StrategyViewUIList)
                gameObject.SetActive(true);
            ToggleCamera();
            moveMode = MovingModes.Strategic;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            //Switch to Firs Person view
            foreach (var gameObject in StrategyViewUIList)
                gameObject.SetActive(false);
            ToggleCamera();
            moveMode = MovingModes.FirstPerson;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
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