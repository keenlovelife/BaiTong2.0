using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using keenlove;

public class LaunchController : MonoBehaviour
{

    public GameObject GuidePanel, PagesPanel;
    public Image PointsImage;
    public Sprite[] PointsSprites;

    void Start()
    {
        layout();
    }
    float standardWidth = 1152;
    float standardHeight = 2048;
    void layout()
    {
        var height = (float)Screen.height;
        var width = (standardWidth / standardHeight) * height;
        if (width < Screen.width)
        {
            width = Screen.width;
            height = (standardHeight / standardWidth) * width;
        }
        PagesPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(width * PagesPanel.transform.childCount, height);
        PagesPanel.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width, height);
        GuidePanel.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 0.5f);
        currentNor = GuidePanel.GetComponent<ScrollRect>().normalizedPosition;
        var pointsHeight = KeenLove.AdaptedDistance(8, KeenLove.StandardSize.y, false);
        var pointsWidth = KeenLove.AdaptedDistance((KeenLove.StandardSize.x - 158 * 2), KeenLove.StandardSize.x);
        var pointsPoy = KeenLove.AdaptedDistance(40, KeenLove.StandardSize.y, false);
        PointsImage.rectTransform.sizeDelta = new Vector2(pointsWidth, pointsHeight);
        PointsImage.rectTransform.anchoredPosition3D = new Vector3(0, pointsPoy, 0);
    }
    Vector2 dragnor;
    bool dragcan = false;
    Vector2 nor;
    bool can = false;
    Vector2 currentNor;
    float t = 0;
    public float maxWaitTime = 0f;
    public float timeInterval = 0.5f;
    void OnTap(TapGesture gesture)
    {
        Debug.Log("===> 轻点！");
        if (currentNor.x >= 1)
        {
            t = 0;
            loadNextScene("2");
        }
    }
    public void OnSwipe(SwipeGesture gesture)
    {
        Debug.Log("===> 轻扫！" + " move:" + gesture.Move + " dir:" + gesture.Direction + " v:" + gesture.Velocity);

        if (gesture.Direction == FingerGestures.SwipeDirection.Right || gesture.Direction == FingerGestures.SwipeDirection.LowerRightDiagonal || gesture.Direction == FingerGestures.SwipeDirection.UpperRightDiagonal)
        {
            var guidepanelpos = GuidePanel.GetComponent<ScrollRect>().normalizedPosition;
            var d = (int)(guidepanelpos.x / 0.5f);
            nor = new Vector2(d == 0 ? 0 : d - 0.5f, 0.5f);
            if (guidepanelpos.x >= 0)
                can = true;
            else
                can = false;

        }
        else if (gesture.Direction == FingerGestures.SwipeDirection.Left || gesture.Direction == FingerGestures.SwipeDirection.LowerLeftDiagonal || gesture.Direction == FingerGestures.SwipeDirection.UpperLeftDiagonal)
        {
            var guidepanelpos = GuidePanel.GetComponent<ScrollRect>().normalizedPosition;
            var d = (int)(guidepanelpos.x / 0.5f);
            nor = new Vector2(d == 2 ? 1 : (d + 0.5f) > 1 ? 1 : (d + 0.5f), 0.5f);
            if(guidepanelpos.x == 1)
            {
                can = false;
                t = 0;
                loadNextScene("2");

            }
            else
            {
                can = true;
            }
        }
    }
    void OnDrag(DragGesture gesture)
    {
        Debug.Log("===> 拖动！");
        if (gesture.Phase == ContinuousGesturePhase.Started)
        {
            can = false;
            dragcan = false;
            autocan = false;
        }
        if (gesture.Phase == ContinuousGesturePhase.Ended)
        {
            dragnor = GuidePanel.GetComponent<ScrollRect>().normalizedPosition;
            var dir = gesture.Position.x - gesture.StartPosition.x;
            if (dir > 0)
            {// 右
                var d = (int)(dragnor.x / 0.5f);
                nor = new Vector2(d == 0 ? 0 : d - 0.5f, 0.5f);
            }
            else if (dir < 0)
            {// 左
                var d = (int)(dragnor.x / 0.5f);
                nor = new Vector2(d == 2 ? 1 : (d + 0.5f) > 1 ? 1 : (d + 0.5f), 0.5f);
            }
            if (dragnor.x < 0 || dragnor.x > 1)
                dragcan = false;
            else
                dragcan = true;

            if (dragcan)
            {
                if (Mathf.Abs(dir) < (Screen.width / 10.0f))
                {
                    nor = currentNor;
                }
            }
            else
            {
                if (dragnor.x > 1)
                {
                    t = 0;
                    loadNextScene("2");
                }
            }
        }
    }

    void loadNextScene(string scenename)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scenename, UnityEngine.SceneManagement.LoadSceneMode.Single);

    }

    bool autocan = false;
    bool autocanlerp = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (!can || !dragcan)
        {
            t += Time.deltaTime;
            if (!autocan && t >= maxWaitTime)
            {
                autocan = true;
                t = 0;
            }

            if (autocan && t >= timeInterval)
            {
                nor = new Vector2(currentNor.x + 0.5f, currentNor.y);
                if (nor.x > 1)
                {
                    loadNextScene("2");
                    return;
                }
                autocanlerp = true;
            }
        }

        if (can || dragcan || autocanlerp)
        {
            t = 0;
            if (PointsImage.sprite != PointsSprites[(int)(nor.x / 0.5f)])
                PointsImage.sprite = PointsSprites[(int)(nor.x / 0.5f)];
            GuidePanel.GetComponent<ScrollRect>().normalizedPosition = Vector2.Lerp(GuidePanel.GetComponent<ScrollRect>().normalizedPosition, nor, 8 * Time.deltaTime);
            if (Mathf.Abs(GuidePanel.GetComponent<ScrollRect>().normalizedPosition.x - nor.x) <= 0.0005f)
            {
                can = false;
                dragcan = false;
                autocanlerp = false;
                GuidePanel.GetComponent<ScrollRect>().normalizedPosition = nor;
                currentNor = nor;
            }
        }
    }

}
