using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperControl : MonoBehaviour
{
    public Camera SniperCamera;
    private PlayerController _playerController;
    private AudioSource _sniperShootSound;
    private Canvas _sniperSightCanvas;

    void Start()
    {
        _playerController = gameObject.GetComponent<PlayerController>();
        _sniperShootSound = gameObject.GetComponent<AudioSource>();

        _sniperSightCanvas = transform.Find("SniperSightCanvas").gameObject.GetComponent<Canvas>();
        _sniperSightCanvas.enabled = false;
        SniperCamera.enabled = false;
    }

    void Update()
    {
        if (PlayerController.moveMode == PlayerController.Mode.Sniper)
        {
            if (Input.GetMouseButton(1))
            {
                _playerController.PlayerMove();
                _playerController.CameraControl();
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
        _sniperShootSound.PlayOneShot(_sniperShootSound.clip);
        Debug.Log("Shoot");
    }

    public void Enable()
    {
        _sniperSightCanvas.enabled = true;
    }

    public void Disable()
    {
        _sniperSightCanvas.enabled = false;
    }
}
