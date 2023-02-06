using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class TutorialManager : LevelManager
{
    public GameObject[] focusObjects;
    public string[] text;
    public GameObject focuser;
    public Camera cam;
    public GameObject MinimapFrame;
    public GameObject BottomUserInfo;

    private bool isFocused;
    private HashSet<int> shownTips;
    private  AnimationCurve curve = AnimationCurve.EaseInOut(0,0,1,1);
    private GameObject continueText;


    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        shownTips = new HashSet<int>();
        isFocused = false;
        continueText = focuser.transform.Find("Text Background").Find("Text (1)").gameObject;
        showTip(0);
    }

    protected new void Update()
    {
        base.Update();
        if (isFocused) {
            if (Input.anyKeyDown) {
                isFocused = false;
                focuser.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }


    private void showTip(int tipNum) {
        if (shownTips.Contains(tipNum)) return;
        if (tipNum == 2) {
            foodTemp = 100f;
            BottomUserInfo.SetActive(true);
        } else if (tipNum == 3) {
            MinimapFrame.SetActive(true);
        }
        StartCoroutine(focusOn(focusObjects[tipNum]));
        focuser.transform.Find("Text Background").GetChild(0).GetComponent<TextMeshProUGUI>().text = text[tipNum];
        shownTips.Add(tipNum);
    }

    IEnumerator focusOn(GameObject obj)
    {
        // reset focuser position
        continueText.SetActive(false);
        Transform focusArea = focuser.transform.Find("FocusArea");

        yield return new WaitForEndOfFrame();

        focuser.SetActive(true);
        Vector3 endingPosition;
        Vector3 endingScale;
        if (obj.GetComponent<RectTransform>() == null) {  // not a ui element
            Vector3 screenPoint = cam.WorldToViewportPoint(obj.transform.position);
            endingPosition = new Vector3(screenPoint.x * cam.pixelWidth, screenPoint.y * cam.pixelHeight, 0);
            endingScale = new Vector3(1,1,1);
        } else { // a ui element
            endingPosition = obj.transform.position;
            Vector2 targetSizeDelta = obj.GetComponent<RectTransform>().sizeDelta * obj.GetComponent<RectTransform>().localScale;
            Vector2 FocusAreaSizeDelta = focusArea.GetComponent<RectTransform>().sizeDelta;
            endingPosition -= (Vector3)((obj.GetComponent<RectTransform>().pivot * 2f - new Vector2(1f,1f)) * targetSizeDelta / 2f);
            endingScale = new Vector3(targetSizeDelta.x / FocusAreaSizeDelta.x * 1.5f, targetSizeDelta.y / FocusAreaSizeDelta.y * 1.5f, 1);
        }

        Vector3 startingPosition = new Vector3(cam.pixelWidth * 0.5f, cam.pixelHeight * 0.5f, 0);
        Vector3 startingScale = new Vector3(0,0,0);

        Time.timeScale = 0.0f;

        float time = 0;
        float duration = 0.6f;
        float startTime = Time.realtimeSinceStartup;
        float endTime = startTime + duration;
        while (time < duration)
        {
            focusArea.position = startingPosition + curve.Evaluate(time/duration) * (endingPosition-startingPosition);
            focusArea.localScale = startingScale + curve.Evaluate(time/duration) * (endingScale-startingScale);
            time = Time.realtimeSinceStartup - startTime;
            yield return new WaitForSecondsRealtime(1f/30f);
        }

        focusArea.position = endingPosition;
        focusArea.localScale = endingScale;
        isFocused = true;
        continueText.SetActive(true);
    }

    public new void enteredTrigger(GameObject triggerObj) {
        base.enteredTrigger(triggerObj);
        Transform parent = triggerObj.transform.parent;
        if (parent != null && parent.name == "triggers") {
            showTip(triggerObj.transform.GetSiblingIndex() + 1);
        }
    }

    private static void SetPadding(RectTransform rect, float left, float top, float right, float bottom) {
        rect.offsetMax = new Vector2(-right, -top);
        rect.offsetMin = new Vector2(left, bottom);
    }
}
