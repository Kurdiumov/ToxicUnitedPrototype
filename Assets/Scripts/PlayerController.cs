using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public Camera SniperCamera;
    private readonly float mouseSensitivity = 100.0f;
    private float _rotationX;
    private float _rotationY;

    private List<GameObject> StrategyViewUIList;

    private AudioSource sniperShootSound;

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController charakterController;
    private PlayerController playerController;
    private Canvas SniperSightCanvas;

    void Start()
    {
        
        moveMode = MovingModes.FirstPerson;
        StrategicCamera.enabled = false;
        SniperCamera.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StrategyViewUIList = new List<GameObject>();
        charakterController = gameObject.GetComponent<CharacterController>();
        playerController = gameObject.GetComponent<PlayerController>();
        sniperShootSound = gameObject.GetComponent<AudioSource>();
        SniperSightCanvas = transform.Find("SniperSightCanvas").gameObject.GetComponent<Canvas>();
        SniperSightCanvas.enabled = false;
        
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
            SniperSightCanvas.enabled = false;
            if (Input.GetButtonDown("Fire1") && Input.GetMouseButton(1))
            {
                SniperSightCanvas.enabled = true;
                if (GetActiveCamera() == SniperCamera)
                    Shoot();
            }
            if (Input.GetMouseButton(1))
            {
                SniperSightCanvas.enabled = true;
                ToggleCamera(SniperCamera);
            }
            else
            {
                ToggleCamera(FirstPersonCamera);
            }
            
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

            Quaternion rotatePlayer = Quaternion.Euler(_rotationY, _rotationX, 0.0f);
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
            ToggleCamera(StrategicCamera);
            moveMode = MovingModes.Strategic;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            //Switch to Firs Person view
            foreach (var gameObject in StrategyViewUIList)
                gameObject.SetActive(false);
            ToggleCamera(FirstPersonCamera);
            moveMode = MovingModes.FirstPerson;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void ToggleCamera(Camera camera)
    {
        StrategicCamera.enabled = false;
        FirstPersonCamera.enabled = false;
        SniperCamera.enabled = false;
        camera.enabled = true;
   }

    private Camera GetActiveCamera()
    {
        if (StrategicCamera.enabled)
            return StrategicCamera;
        if (FirstPersonCamera.enabled)
            return FirstPersonCamera;
        if (SniperCamera.enabled)
            return SniperCamera;
        return null;
    }

    private void Shoot()
    {
        sniperShootSound.PlayOneShot(sniperShootSound.clip);
        Debug.Log("Shoot");
    }
}