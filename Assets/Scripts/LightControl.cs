using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    Light2D globalLight;
    float lightLerpSpeed = .0005f;
    float lerpTime=0;
    bool lightSwitch=false;
    public Color startColor;
    public Color endColor;
    // Start is called before the first frame update
    void Start()
    {
        globalLight = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lightSwitch)
        {
            if (lerpTime > 0)
            {
                lerpTime -= lightLerpSpeed;
            }
            else
            {
                lightSwitch = false;
            }
        }
        else {
            if (lerpTime < 1)
            {
                lerpTime += lightLerpSpeed;
            }
            else
            {
                lightSwitch = true;
            }
        }
        
        globalLight.color = Color.Lerp(startColor, endColor,lerpTime);
    }
}
