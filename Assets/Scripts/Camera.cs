using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera : MonoBehaviour
{

     CinemachineVirtualCamera camera;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    public static Camera instance;
    float shakeTime;
    float totalTime;
    float startIntensity;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        camera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeTime>0)
        {
            shakeTime -= Time.deltaTime;
            if(shakeTime<=0)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startIntensity,0,1-(shakeTime/totalTime));

            }
        }
    }
    public void ShakeScreen(float intensity, float time)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        startIntensity = intensity;
        shakeTime = time;
        totalTime = time;
    }
}

