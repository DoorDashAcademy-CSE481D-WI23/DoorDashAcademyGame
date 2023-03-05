using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;// Required when using Event data.
using TMPro;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tooltip;
    GameObject tooltipPopup;
    GameObject background;
    public void OnPointerEnter(PointerEventData eventData) {
        if (tooltipPopup == null) {
            tooltipPopup = new GameObject("tooltipPopup");
            TextMeshProUGUI textComponent = tooltipPopup.AddComponent<TextMeshProUGUI>();
            tooltipPopup.transform.parent = transform;
            RectTransform rectTransformComponent = tooltipPopup.GetComponent<RectTransform>();
            rectTransformComponent.anchorMax = new Vector2(1f,0.5f);
            rectTransformComponent.anchorMin = new Vector2(1f,0.5f);
            rectTransformComponent.pivot = new Vector2(0f, 0.5f);
            textComponent.alignment = TextAlignmentOptions.MidlineLeft;
            rectTransformComponent.anchoredPosition = new Vector2(0,0);
            textComponent.raycastTarget = false;
            textComponent.text = tooltip;
            textComponent.ForceMeshUpdate();

            background = new GameObject("tooltipPopupBackground");
            background.transform.parent = transform;
            background.transform.SetSiblingIndex(0);
            Image imageComponent = background.AddComponent<Image>();
            imageComponent.raycastTarget = false;
            imageComponent.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);
            RectTransform backgroundRectTransform = background.GetComponent<RectTransform>();
            backgroundRectTransform.anchorMax = new Vector2(1f,0.5f);
            backgroundRectTransform.anchorMin = new Vector2(1f,0.5f);
            backgroundRectTransform.pivot = new Vector2(0f, 0.5f);
            backgroundRectTransform.sizeDelta = textComponent.GetRenderedValues();
            backgroundRectTransform.anchoredPosition = new Vector2(0,0);
        } else {
            tooltipPopup.SetActive(true);
            background.SetActive(true);
        }
    }

  public void OnPointerExit(PointerEventData eventData)
  {
        tooltipPopup.SetActive(false);
        background.SetActive(false);
  }
}
