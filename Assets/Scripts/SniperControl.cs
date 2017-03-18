using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SniperControl : MonoBehaviour
{
    public Camera SniperCamera;
    public int Ammo;
    public int Attack;
    public int TimeBetweenShoots;
    public float KnockbackAmount;
    public int ZoomScaleFactor;
    public int MaxZoomScale;
    public int MinZoomScale;
    public AudioSource SniperShootSound;
    public AudioSource SniperOutOfAmmoSound;
    

    private float _knockbackOffset;
    private PlayerController _playerController;
    private Canvas _sniperSightCanvas;
    private Text ammoText;
    private Image _sniperImage;
    private readonly float _mouseSensitivity = 30;
    private float _timeFromLastShoot;
    
    void Start()
    {
        _playerController = gameObject.GetComponent<PlayerController>();
        _sniperSightCanvas = transform.Find("SniperSightCanvas").gameObject.GetComponent<Canvas>();

        SniperCamera.enabled = false;
        ammoText = _sniperSightCanvas.GetComponentInChildren<Text>();
        ammoText.text = "Ammo: " + Ammo;
        _sniperImage = _sniperSightCanvas.GetComponentInChildren<Image>();
        _sniperImage.enabled = false;
    }

    void Update()
    {
        _timeFromLastShoot -= Time.deltaTime;

        if (PlayerController.moveMode == PlayerController.Mode.Sniper)
        {
            if (Input.GetMouseButton(1))
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (_timeFromLastShoot <= 0)
                        Shoot();
                }
                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    SetScroll(Input.GetAxis("Mouse ScrollWheel"));
                }
                _playerController.PlayerMove();
                CameraControl();
            }
            else
            {
                _playerController.SwitchControl(PlayerController.Mode.FirstPerson);
            }
        }
    }


    private void Shoot()
    {
        if (this.Ammo > 0)
        {
            _timeFromLastShoot = TimeBetweenShoots;
            SniperShootSound.PlayOneShot(SniperShootSound.clip);
            Debug.Log("Shoot");
            this.Ammo--;
            ammoText.text = "Ammo: " + Ammo;
            _knockbackOffset = KnockbackAmount;

            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(SniperCamera.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                //TODO:
                //For new attack insert code here
                if (hitInfo.transform.name == "TestUnitObject")
                {
                    Unit unit = (Unit) hitInfo.transform.GetComponent<Unit>();
                    unit.GetDamage(this.Attack);
                    Debug.Log("Hit comrad unit. Unit Health now is: " + unit.Health);
                } 
                else if (hitInfo.transform.gameObject.tag == "Enemy" && hitInfo.collider == hitInfo.transform.gameObject.GetComponent<CapsuleCollider>())
                {
                    Enemy enemy = (Enemy)hitInfo.transform.GetComponent<Enemy>();
                    enemy.GetDamage(this.Attack);
                    Debug.Log("Hit Enemy unit. Unit Health now is: " + enemy.Health);
                }
            }
        }
        else
        {
            SniperOutOfAmmoSound.PlayOneShot(SniperOutOfAmmoSound.clip);
            Debug.Log("Out of ammo");
        }

    }


    public void Enable()
    {
        _sniperImage.enabled = true;
    }

    public void Disable()
    {
        _sniperImage.enabled = false;
    }

    public void EnableText()
    {
        ammoText.enabled = true;
    }

    public void DisableText()
    {
        ammoText.enabled = false;
    }

    public void CameraControl()
    {
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        if (PlayerController.moveMode == PlayerController.Mode.FirstPerson ||
            PlayerController.moveMode == PlayerController.Mode.Sniper)
        {
            _playerController.RotationX += mouseX * GetSensetiity() * Time.deltaTime;
            _playerController.RotationY += mouseY * GetSensetiity() * Time.deltaTime;


            //knockback when shooting
            if (_timeFromLastShoot == TimeBetweenShoots)
            {
                _playerController.RotationY -= _knockbackOffset;
            }
            _playerController.RotationY = Mathf.Lerp(_playerController.RotationY, _playerController.RotationY + _knockbackOffset, 0.1f);
            _knockbackOffset = Mathf.Lerp(_knockbackOffset, 0f, 0.1f);


            
            if (_playerController.RotationY > 80)
            {
                _playerController.RotationY = 80;
            }
            else if (_playerController.RotationY < -80)
            {
                _playerController.RotationY = -80;
            }

            Quaternion rotatePlayer = Quaternion.Euler(_playerController.RotationY, _playerController.RotationX, 0.0f);
            transform.rotation = rotatePlayer;
        }
    }

    private void SetScroll(float direction)
    {

        if (direction < 0)
        {
            if (SniperCamera.fieldOfView < MaxZoomScale)
               SniperCamera.fieldOfView += ZoomScaleFactor;
        }
        else
        {
            if (SniperCamera.fieldOfView > MinZoomScale)
                SniperCamera.fieldOfView -= ZoomScaleFactor;
        }
    }

    private float GetSensetiity()
    {
        int i = MinZoomScale;
        int j = ZoomScaleFactor-1;
        for (; i < SniperCamera.fieldOfView; i += 4, j++) ;
            return j + i;

        //2   -> 5
        //6   -> 10
        //10  -> 15
        //14  -> 20 
        //18  -> 25
        //22  -> 30
    }
}
