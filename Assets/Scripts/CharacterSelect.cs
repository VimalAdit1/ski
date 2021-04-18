using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterSelect : MonoBehaviour
{
    public GameObject vcam;
    public GameObject[] skins;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        int selected = PlayerPrefs.GetInt("Skin", 0);
        Debug.Log(selected);
        skins[selected].SetActive(true);
        CinemachineVirtualCamera cam = vcam.GetComponent<CinemachineVirtualCamera>();
        cam.Follow = skins[selected].transform;
        gameManager.player = skins[selected];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
