using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SniperControl : MonoBehaviour
{
    public Camera SniperCamera;
    public int Ammo;
    public AudioSource SniperShootSound;
    public AudioSource SniperOutOfAmmoSound;

    private PlayerController _playerController;
    private Canvas _sniperSightCanvas;
    private Text  ammoText;
    private Image _sniperImage;
    private readonly float _mouseSensitivity = 30;

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
        if (PlayerController.moveMode == PlayerController.Mode.Sniper)
        {
            if (Input.GetMouseButton(1))
            {
                _playerController.PlayerMove();
                CameraControl();
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
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
            SniperShootSound.PlayOneShot(SniperShootSound.clip);
            Debug.Log("Shoot");
            this.Ammo--;
            ammoText.text = "Ammo: " + Ammo;
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

        if (PlayerController.moveMode == PlayerController.Mode.FirstPerson || PlayerController.moveMode == PlayerController.Mode.Sniper)
        {
            _playerController.RotationX += mouseX * _mouseSensitivity * Time.deltaTime;
            _playerController.RotationY += mouseY * _mouseSensitivity * Time.deltaTime;

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
}
