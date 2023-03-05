using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public string itemName;
    public int itemCost;
    public string tooltip;

    void Start() {
        try {
            gameObject.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text =
            PlayerPrefs.GetInt("has" + itemName, 0) == 1 ? "owned" : "$" + itemCost;
            gameObject.transform.Find("Button").gameObject.AddComponent<Tooltip>().tooltip = tooltip;
        } catch (System.Exception e) {
            Debug.Log(e);
        }
    }


}
