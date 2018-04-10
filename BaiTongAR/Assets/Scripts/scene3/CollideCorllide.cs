using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Sprites;

public class CollideCorllide : MonoBehaviour {

    EventSystem eventSystem;
    GraphicRaycaster graphicRaycaster;

   public GameObject content;


    /// <summary>
    /// 手势操作用到的基础数据
    /// </summary>
    float beganTouchTime;
    bool isMoved = false;

    void Start () {
        graphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();
        Input.multiTouchEnabled = true;
        beganTouchTime = Time.time;
	}

    // --------------------  解决点击穿透UI问题 -------------------
    bool CheckGuiRaycastObjects()
    {
        var eventData = new PointerEventData(eventSystem);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        var list = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, list);

       // Debug.Log(list.Count);

        return list.Count > 0;
    }

	void Update () {
        

       if (CheckGuiRaycastObjects()) return;


        if(Input.touchCount == 1)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                beganTouchTime =Time.time;
            }

            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                isMoved = true;
                Debug.Log(" 手指在屏幕上移动了!");
            }

            if(Input.touches[0].phase == TouchPhase.Ended)
            {
                var dx = Time.time - beganTouchTime;

				if (!isMoved && dx <= 0.3f)
                {
                    click();
                }
                isMoved = false;
				beganTouchTime = Time.time;
			}
          
        }else
        {
            isMoved = false;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor && Input.GetMouseButtonDown(0))
        {
            click();
        }
    }

    void click()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo))
        {
            if (hitinfo.collider.gameObject.tag == "trackableObject")
            {
                Debug.Log("点击了" + hitinfo.collider.name);
                TrackableEventManager.Instance.cameraLogStr = "点击了" + hitinfo.collider.name;

                // 加载图片信息
                if (TrackableEventManager.Instance.YaoCaiInfo != hitinfo.collider.name)
                {
                    StartCoroutine(loadInfoPic(hitinfo.collider.name));
                }
                else
                {
                    // UI 展示动画
                    var compent = GameObject.Find("script-root").GetComponent<MainController>();
                    var animInfo = compent.InfoPanelAnimator.GetCurrentAnimatorStateInfo(0);
                    if (!animInfo.IsName("hideInfoPanel"))
                    {
                        compent.InfoPanelAnimator.Play("hideInfoPanel");
                    }
                    else
                    {
                        compent.InfoPanelAnimator.Play("infoPanel");
                    }
                }

            }
            else
            {
            }
        }
    }

    IEnumerator loadInfoPic(string hitinfoName)
    {


        var name = hitinfoName.Remove(hitinfoName.Length - 5);
        var path = Application.streamingAssetsPath + "/infoPic/" + name + ".png";
        WWW www = new WWW(path);
        yield return www;

        var iamge = content.GetComponent<Image>();
        if (iamge.sprite != null)
        {
            DestroyImmediate(iamge.sprite, true);
            iamge.sprite = null;
            Resources.UnloadUnusedAssets();
        }

        if (www.error == null)
        {
            var sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
            iamge.sprite = sprite;
            TrackableEventManager.Instance.cameraLogStr = "\n加载到图片：" + name;

			if (Display.main.systemWidth >= www.texture.width)
			{
				iamge.rectTransform.sizeDelta = new Vector2(www.texture.width, www.texture.height);
				var conTransform = content.GetComponent<RectTransform>();
				conTransform.anchoredPosition3D = new Vector3(0, -0.5f * www.texture.height, 0);
			}
			else
			{
				var size = new Vector2((float)Display.main.systemWidth, Display.main.systemWidth * (www.texture.height / (float)www.texture.width));
				iamge.rectTransform.sizeDelta = size;
				var conTransform = content.GetComponent<RectTransform>();
				conTransform.anchoredPosition3D = new Vector3(0, -0.5f * www.texture.height, 0);
			}

            content.transform.Find("Text").gameObject.SetActive(false);
            //Debug.Log("加载到图片:" + hitinfo.collider.name + " UI image 尺寸:" + size + " 图片尺寸:(" + s.width + " " + s.height + ")");
            
            TrackableEventManager.Instance.YaoCaiInfo = hitinfoName;
            // UI 展示动画
            var compent = GameObject.Find("script-root").GetComponent<MainController>();
            var animInfo = compent.InfoPanelAnimator.GetCurrentAnimatorStateInfo(0);
            if (!animInfo.IsName("infoPanel"))
            {
                compent.InfoPanelAnimator.Play("infoPanel");

            }
        }
		else
        {
            var t = content.transform.Find("Text").GetComponent<Text>();
            t.gameObject.SetActive(true);
            t.rectTransform.sizeDelta = new Vector2(0, 24 / 667.0f * Screen.height);
            iamge.rectTransform.sizeDelta = new Vector2(Screen.width, transform.Find("infoPanel").GetComponent<RectTransform>().rect.height);
			
            Debug.Log("加载图片失败：" + path);
            TrackableEventManager.Instance.cameraLogStr = "\n加载图片失败：" + path;
            TrackableEventManager.Instance.YaoCaiInfo = hitinfoName;

            // UI 展示动画
            var compent = GameObject.Find("script-root").GetComponent<MainController>();
            var animInfo = compent.InfoPanelAnimator.GetCurrentAnimatorStateInfo(0);
            if (!animInfo.IsName("infoPanel"))
            {
                compent.InfoPanelAnimator.Play("infoPanel");
            }
        }
    }

}
