using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    enum MovingModes
    {
        Player,
        Strategic,
        Blocked,
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
    public Camera camera;
 
    //public Camera minimapCam;
    public float viewRange = 20.0f;
    public Vector3 roomView = new Vector3(0.0f, 90.0f, -6.0f);
    Vector3 playerView = new Vector3(0.0f, 0.5f, 0.0f);

    public RaycastHit Ceiling { get; private set; }
    public RaycastHit HitItem { get; private set; }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        moveMode = MovingModes.Player;
        Cursor.visible = false;
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

    void RoomMove()
    {

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
            Quaternion rotateCam = Quaternion.Euler(rotationY, rotationX, 0.0f);

            transform.rotation = rotatePlayer;
            camera.transform.rotation = rotateCam;
        }
    }

    void SwitchControl()
    {
        if (moveMode == MovingModes.Player)
        {
            moveMode = MovingModes.Strategic;
            this.playerView = camera.transform.localPosition;
            camera.transform.localPosition = this.roomView;
            camera.transform.LookAt(LookTarget, Vector3.up);

            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            moveMode = MovingModes.Player;
            camera.transform.localPosition = this.playerView;

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
        camera.enabled = false;
        yield return new WaitForSeconds(1.0f);
        camera.enabled = true;
        yield return 0;
    }
}