using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSelector : MonoBehaviour
{
    RawImage button;
    public Texture2D swipe;
    public Texture2D tilt;
    int isSwipeEnabled;
    // Start is called before the first frame update
    void Start()
    {
        isSwipeEnabled = PlayerPrefs.GetInt("Swipe", 0);
        button = GetComponent<RawImage>();
        RenderButton();
    }

    private void RenderButton()
    {
        if (isSwipeEnabled == 0)
        {
            button.texture = tilt;
        }
        else if (isSwipeEnabled == 1)
        {
            button.texture = swipe;
        }
    }

    public void ChangeControl()
    {
        isSwipeEnabled = isSwipeEnabled == 1 ? 0 : 1;
        PlayerPrefs.SetInt("Swipe", isSwipeEnabled);
        RenderButton();
    }
}
