using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public enum Mode
    {
        FirstPerson,
        Strategic,
        Sniper
    };

    public Transform LookTarget;
    public static Mode moveMode =  Mode.FirstPerson;

    //Camera:
    public Camera FirstPersonCamera;
    public Camera StrategicCamera;

    private readonly  float _mouseSensitivity = 100.0f;
    public float RotationX;
    public float RotationY;

    private List<GameObject> StrategyViewUIList; 

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController charakterController;
    private PlayerController playerController;
    private SniperControl sniperConrol;

    void Start()
    {
        moveMode = Mode.FirstPerson;
        StrategicCamera.enabled = false;      
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StrategyViewUIList = new List<GameObject>();
        charakterController = gameObject.GetComponent<CharacterController>();
        playerController = gameObject.GetComponent<PlayerController>();
        sniperConrol = gameObject.GetComponent<SniperControl>();


        foreach (var gameObj in GameObject.FindGameObjectsWithTag("StrategyView"))
        {
            StrategyViewUIList.Add(gameObj);
            gameObj.SetActive(false);
        }
    }

    void Update()
    {
        switch (moveMode)
        {
            case Mode.FirstPerson:
                if (Input.GetMouseButton(1))
                {
                    SwitchControl(Mode.Sniper);
                }
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    SwitchControl(Mode.Strategic);
                }
                CameraControl();
                PlayerMove();
                break;
            case Mode.Strategic:
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    SwitchControl(Mode.FirstPerson);
                }
                break;
        }

    }

    public void PlayerMove()
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

    public void CameraControl()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        if (moveMode == Mode.FirstPerson || moveMode == Mode.Sniper)
        {
            RotationX += mouseX * _mouseSensitivity * Time.deltaTime;
            RotationY += mouseY * _mouseSensitivity * Time.deltaTime;

            if (RotationY > 80)
            {
                RotationY = 80;
            }
            else if (RotationY < -80)
            {
                RotationY = -80;
            }

            Quaternion rotatePlayer = Quaternion.Euler(RotationY, RotationX, 0.0f);
            transform.rotation = rotatePlayer;
        }
    }

    public void SwitchControl(Mode mode)
    {
        moveMode = mode;

        switch (moveMode)
        {
            case Mode.FirstPerson:
                sniperConrol.EnableText();  
                sniperConrol.Disable();
                ToggleCamera(FirstPersonCamera);
                //Switch to Firs Person view
                foreach (var gameObject in StrategyViewUIList)
                    gameObject.SetActive(false);

                moveMode = Mode.FirstPerson;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case Mode.Sniper:
                sniperConrol.EnableText();
                sniperConrol.Enable();
                ToggleCamera(sniperConrol.SniperCamera);
                break;
            case Mode.Strategic:
                sniperConrol.Disable();
                sniperConrol.DisableText();
                ToggleCamera(StrategicCamera);
                foreach (var gameObject in StrategyViewUIList)
                    gameObject.SetActive(true);

                moveMode = Mode.Strategic;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }

    private void ToggleCamera(Camera camera)
    {
        StrategicCamera.enabled = false;
        FirstPersonCamera.enabled = false;
        sniperConrol.SniperCamera.enabled = false;
        camera.enabled = true;
    }

    private Camera GetActiveCamera()
    {
        if (StrategicCamera.enabled)
            return StrategicCamera;
        if (FirstPersonCamera.enabled)
            return FirstPersonCamera;
        if (sniperConrol.SniperCamera.enabled)
            return sniperConrol.SniperCamera;
        return null;
    }

}