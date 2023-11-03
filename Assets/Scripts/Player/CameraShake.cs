using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class CameraShake : MonoBehaviour
{
    // end world flip action.
    public event Action OnShakeOver;

    GameController controller;
    public static CameraShake Instance { get; private set; }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private void Awake()
    {
        Instance = this;
       cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float _Intensity, float _Time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _Intensity; // making camera shake.

        shakeTimer = _Time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0.0f)
            {
                // timer over.
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0.0f;

                // set back to normal game mode.
                TriggerOnShakeOver();
            }
        }

    }

    // trigger OnShapeOver.
    public void TriggerOnShakeOver()
    {
       // Debug.Log("Entered TriggerOnShakeOver");

        if (OnShakeOver != null)
        {
            //Debug.Log("Entered TriggerOnShakeOver to invoke");
            OnShakeOver.Invoke();
        }
    }
    //public GameObject ShakeFX;
    //public float ShakeDur;
    //private bool IsCameraShaking = false;

    //public void SetIsCameraShaking(bool _TrueOrFalse)
    //{
    //    IsCameraShaking = _TrueOrFalse;
    //}

    //// Start is called before the first frame update
    //void Start()
    //{
    //    ShakeFX.SetActive(false);  
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        StopAllCoroutines();
    //        StartCoroutine(Shake(ShakeDur));
    //    }
    //}

    //IEnumerator Shake(float t)
    //{
    //    ShakeFX.SetActive(true);
    //    yield return new WaitForSeconds(t);
    //    ShakeFX.SetActive(false);
    //}
}
