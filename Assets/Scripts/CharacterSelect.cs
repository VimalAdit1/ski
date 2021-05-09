using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterSelect : MonoBehaviour
{
    public GameObject vcam;
    public GameObject[] skins;
    public GameManager gameManager;
    public bool isUi;
    int totalSkins;
    // Start is called before the first frame update
    void Start()
    {
        totalSkins = skins.Length;
        int selected = PlayerPrefs.GetInt("Skin", 0);
        for (int i = 0; i < totalSkins; i++)
        {
            skins[i].SetActive(i == selected);
        }
        if (!isUi)
        {
            CinemachineVirtualCamera cam = vcam.GetComponent<CinemachineVirtualCamera>();
            cam.Follow = skins[selected].transform;
            gameManager.player = skins[selected];
        }
    }

    public void reload()
    {
        int selected = PlayerPrefs.GetInt("Skin", 0);
        for (int i = 0; i < totalSkins; i++)
        {
            skins[i].SetActive(i==selected);
        }
    }
}
