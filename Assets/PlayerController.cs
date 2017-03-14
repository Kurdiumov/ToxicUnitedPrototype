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
    private Vector3 moveVector;
    ///CharacterController controller;
    //public CharacterController characterController { get { return controller; } }
    float ladderCd;


    //   Camera:
    public float mouseSensitivity = 100.0f;
    private float rotationX;
    private float rotationY;
    public Camera cam;
    //public Camera minimapCam;
    public float viewRange = 20.0f;
    public Vector3 roomView = new Vector3(0.0f , 90.0f, -6.0f);
    Vector3 playerView = new Vector3(0.0f , 0.5f , 0.0f);

    RaycastHit ceiling;
    RaycastHit hitItem;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        moveMode = MovingModes.Player;
        //controller = GetComponent<CharacterController>();
        Cursor.visible = false;
        moveVector = Vector3.zero;
    }
    void Update()
    {
        //showhideCursor();
        if(moveMode == MovingModes.Player)
        {
            //Move();
            cameraControl();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchControl();
        }

        
    }

    void OnTriggerEnter(Collider other)
    {
    }


    void OnTriggerExit(Collider other)
    {
    }
    /*
    void Move()
    {
        if (controller.isGrounded && moveMode == MovingModes.Player)
        {
            moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveVector = transform.TransformDirection(moveVector);

            moveVector *= speed;
            /*if (Input.GetButton("Fire3"))
            {
                moveVector *= sprintSpeed;
            }
            else
            {
                moveVector *= speed;
            }*/

            /*
            if (Input.GetButton("Jump"))
            {
                moveVector.y = jumpSpeed;
            }



        }
        controller.Move(moveVector * Time.deltaTime);
    }*/

    void RoomMove()
    {

    }

    void cameraControl()
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
            else
            {
                if (rotationY < -80)
                {
                    rotationY = -80;

                }
            }

            Quaternion rotatePlayer = Quaternion.Euler(0.0f, rotationX, 0.0f);
            Quaternion rotateCam = Quaternion.Euler(rotationY, rotationX, 0.0f);
           
            transform.rotation = rotatePlayer;
            cam.transform.rotation = rotateCam;
            //minimapCam.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);


            //Debug.Log(rotationY);



            Vector3 rotray = new Vector3(0.0f, 0.0f, 1.0f);
            // Vector3 raypose = new Vector3(0.0f, 0.0f, 0.0f);





            
        }

        /// Third person
        if (moveMode ==  MovingModes.Strategic)
        {
            if (rotationY > 80)
            {
                rotationY = 80;

            }
            else
            {
                if (rotationY < 10)
                {
                    rotationY =  10;

                }
            }

            Quaternion rotatePlayer = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            //Quaternion rotateCam = Quaternion.Euler( rotationY, rotationX, 0.0f);
            Quaternion rotateCam = Quaternion.Euler(60.0f, 0.0f, 0.0f);

            transform.rotation = rotatePlayer;
            cam.transform.rotation = rotateCam;

            Vector3 rotray = new Vector3(0.0f, 0.0f, 1.0f);
            

            //Interaktcja w widoku z gory
            /*
            Debug.DrawRay(transform.position, rotatePlayer * rotray * interactDistance, Color.magenta);
            if (Physics.Raycast(transform.position, rotatePlayer * rotray, out hitItem, interactDistance))
            {
                if (hitItem.collider.tag == "Interactive")
                {
                    UIEvents.TurnInteraction(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        //Debug.Log("Cos niby kliklem");
                        Interact();
                    }
                }

            }
            else
            {
                UIEvents.TurnInteraction(false);
            }*/
        }
        
    }

    void SwitchControl()
    {
        //Debug.Log("Tryb z gory");
        if(moveMode == MovingModes.Player)
        {
            moveMode = MovingModes.Strategic;
            this.playerView = cam.transform.localPosition;
            //Debug.Log(playerView);
            cam.transform.localPosition = this.roomView;
            cam.transform.LookAt(LookTarget, Vector3.up);

            moveVector = Vector3.zero;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            moveMode = MovingModes.Player;
            cam.transform.localPosition = this.playerView;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void showhideCursor()
    {
        if (Input.GetButtonDown("Fire2") && Cursor.visible == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            if (Input.GetButtonDown("Fire2") && Cursor.visible == true)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    

    private IEnumerator turnOffCam()
    {
        cam.enabled = false;
        yield return new WaitForSeconds(1.0f);
        cam.enabled = true;
        yield return 0;
    }
}