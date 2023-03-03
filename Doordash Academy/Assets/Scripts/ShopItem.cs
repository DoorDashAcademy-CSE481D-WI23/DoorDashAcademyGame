using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public string itemName;
    public int itemCost;

    void Start() {
        try {
            gameObject.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text =
            PlayerPrefs.GetInt("has" + itemName, 0) == 1 ? "owned" : "$" + itemCost;
        } catch (System.Exception e) {
            Debug.Log("tried to set cost label, but couldn't find cost text : ");
            Debug.Log(e);
        }
    }
}
