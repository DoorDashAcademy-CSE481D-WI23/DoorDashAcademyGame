using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class ShopManager : MonoBehaviour
{
    private GameObject[] purchasables;

    void Start()
    {
        //find purchasable items in the scene
        purchasables = GameObject.FindGameObjectsWithTag("purchasable");

        //display money??

        //update buttons as interactable or not based on amount of money
        updateButtons();
    }

    private void updateButtons() {
        int money = PlayerPrefs.GetInt("money", 0);
        foreach (GameObject item in purchasables) {
            TextMeshProUGUI costText = item.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            int cost = item.GetComponent<ShopItem>().itemCost;
            if (PlayerPrefs.GetInt("has" + item.GetComponent<ShopItem>().itemName, 0) == 1) {  // already owned
                costText.color = Color.white;
                costText.text = "owned";
                item.GetComponentInChildren<Button>().interactable = false;
            } else if (money >= cost) {
                costText.color = Color.green;
                item.GetComponentInChildren<Button>().interactable = true;
            } else {
                costText.color = Color.red;
                item.GetComponentInChildren<Button>().interactable = false;
            }

        }
    }

    public void buyItem(ShopItem item) {

        if (PlayerPrefs.GetInt("has" + item.itemName, 0) == 1) return;  // player has already purchased item

        // TODO: we should definitely add a logging event to this so we can see if people are buying upgrades

        PlayerPrefs.SetInt("has" + item.itemName, 1);
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) - item.itemCost);
        PlayerPrefs.Save();

        updateButtons();
        //TODO: play audio
    }

}
