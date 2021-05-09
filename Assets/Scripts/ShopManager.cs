using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopManager : MonoBehaviour
{
    public GameObject[] skins;
    public Button right;
    public Button left;
    public Button buy;
    public Button play;
    public TMP_Text coinsText;
    public TMP_Text scoreText;
    public GameManager gameManager;
    int selected;
    int coins;
    String owned;
    String[] ownedskins;
    HashSet<String> skinsList = new HashSet<string>();
    // Start is called before the first frame update
    void Start()
    {
        selected = PlayerPrefs.GetInt("Skin", 0);
        coins = PlayerPrefs.GetInt("Coins", 0);
        owned = PlayerPrefs.GetString("Owned", "0");
        ownedskins = owned.Split(',');
        
        foreach (String skin in ownedskins)
        {
            skinsList.Add(skin);
        }
        try
        {
            scoreText.text = "HighScore: "+PlayerPrefs.GetInt("HighScore", 0).ToString();
        }
        catch(Exception e)
        {

        }
        changeShip(selected);
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            checkinput();
            right.interactable = !(selected == skins.Length - 1);
            left.interactable = !(selected == 0);
            coinsText.text = coins.ToString();
        }
        catch(Exception e)
        {

        }
    }

    private void checkinput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Right();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Left();
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (play.interactable)
            {
                gameManager.Play();
            }
        }
    }

    void changeShip(int index)
    {
        for (int i = 0; i < skins.Length; i++)
        {
            skins[i].SetActive((i == index));
        }
        if (isSkinUnlocked(selected))
        {
            play.interactable = true;
            buy.gameObject.SetActive(false);
            PlayerPrefs.SetInt("Skin", selected);
        }
        else
        {
            buy.gameObject.SetActive(true);
            int shipPrice = skins[selected].GetComponent<Player>().price;
            buy.GetComponentInChildren<TMP_Text>().text = shipPrice.ToString();
            if (coins >= shipPrice)
            {
                buy.interactable = true;
            }
            else
            {
                buy.interactable = false;
            }
            play.interactable = false;
        }
    }

    private bool isSkinUnlocked(int selected)
    {
        return skinsList.Contains(selected.ToString());
    }

    public void Right()
    {
        if (selected < skins.Length - 1)
        {
            selected++;
            changeShip(selected);
        }
    }
    public void Left()
    {
        if (selected > 0)
        {
            selected--;
            changeShip(selected);
        }
    }
    public void Buy()
    {
        int shipPrice = skins[selected].GetComponent<Player>().price;
        coins = coins - shipPrice;
        PlayerPrefs.SetInt("Coins", coins);
        owned += "," + selected;
        skinsList.Add(selected.ToString());
        PlayerPrefs.SetString("Owned", owned);
        PlayerPrefs.SetInt("Skin", selected);
        buy.gameObject.SetActive(false);
        play.interactable = true;
    }
}
