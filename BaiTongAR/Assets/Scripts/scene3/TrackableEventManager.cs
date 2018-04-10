using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Vuforia;

public class TrackableEventManager : MonoBehaviour
{
#region 处理成一个单例
    /// <summary>
    /// 单例
    /// </summary>
    static TrackableEventManager _instance;
    public static TrackableEventManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion


	public string YaoCaiInfo = string.Empty;

    public GameObject tishiButton;

	/// <summary>
	/// 是否找到一个图片目标
	/// </summary>
	public bool IsFoundOneImageTarget = false;
    /// <summary>
    /// 当前与图片目标相对应显示的模型对象
    /// </summary>
    public GameObject CurrentShowModelObjectOfImageTarget;


    /// <summary>
    /// 用于计算缩放的基础数据
    /// </summary>
    float disNow = 0.0f, standDis = 0.0f; Vector3 standScaol;

    /// <summary>
    /// 用于计算旋转的基础数据
    /// </summary>
    float x = 0.0f, y = 0.0f, speed = 8.0f;


    /// <summary>
    /// 解决穿透 UI 问题
    /// </summary>
    EventSystem eventSystem;
   public  GraphicRaycaster graphicRaycaster;


    /// <summary>
    /// 屏幕输出 Log 信息需要用到的基础数据
    /// </summary>
    public string LogStr;
    public string cameraLogStr;
    public string trackableLogStr;
    string dxLog;


    private void Awake()
    {
        _instance = this;
    }
    void Start () {
    }


    bool CheckGUIRaycastObjects()
    {
        bool hasGuiobj = false;

        foreach (var touch in Input.touches)
        {

            var data = new PointerEventData(eventSystem);
            data.pressPosition = touch.position;
            data.position = touch.position;
            var list = new List<RaycastResult>();
            graphicRaycaster.Raycast(data, list);
            if (list.Count > 0)
            {
                hasGuiobj = true;
                break;
            }
        }

        return hasGuiobj;

    }

  


    void Update () {

        if (CheckGUIRaycastObjects())
        {
            x = y = standDis = 0.0f;
            standScaol = new Vector3(1, 1, 1);
            prePosition = new Vector3(0, 0, -1);
            return;
		}
            

        if (IsFoundOneImageTarget && CurrentShowModelObjectOfImageTarget)
        {
            if (2 == Input.touchCount)
            {
                disNow = System.Math.Abs(Vector2.Distance(Input.touches[0].position, Input.touches[1].position));
                if (standDis == 0.0f)
                {
                    standDis = disNow;
                    standScaol = CurrentShowModelObjectOfImageTarget.transform.localScale;
                }

            }
            else
            {
                if (standDis != 0.0f)
                    standDis = 0.0f;
            }


            if (1 == Input.touchCount)
            {
                var touche = Input.touches[0];

    //            var del = Input.touches[0].deltaPosition;

				//x = del.y;
                //z = -del.x;


				//dxLog = "移动差值: " + del;

                if(prePosition.z != 0)
                {
                    prePosition = new Vector3(touche.position.x, touche.position.y, 0);
                }
                var dx = touche.position - new Vector2(prePosition.x, prePosition.y);
                dxLog = "移动差值: " + dx;
                x = dx.x;
                y = dx.y;
            }else
            {
                if (prePosition.z == 0)
                    prePosition = new Vector3(0, 0, -1);
            }

        }
    }

    Vector3 prePosition = new Vector3(0,0,-1024);

    void LateUpdate()
    {

        if (CheckGUIRaycastObjects()) return;

        if(IsFoundOneImageTarget && CurrentShowModelObjectOfImageTarget)
        {
           touchHandle();
        }

		
        LogStr = cameraLogStr + "\n" + trackableLogStr +"\n" + dxLog + "\n";

	}

    /// <summary>
    ///   处理手指点击、滑动、旋转、缩放
    /// </summary>
    void touchHandle()
    {
        
        if (1 == Input.touchCount)
        {
            //----------------         旋转模型        ---------------------
            CurrentShowModelObjectOfImageTarget.GetComponent<Transform>().Rotate(new Vector3(y * Time.deltaTime * speed,
                                                                                             -x * Time.deltaTime * speed,
                                                                                             0),
                                                                                 Space.World);
            prePosition = Input.touches[0].position;
        }


        if (2 == Input.touchCount)
        {
            var dx = disNow < standDis ?
                disNow / standDis :  1 + (disNow - standDis) / standDis;

            // -------------         缩放模型        -------------------
            CurrentShowModelObjectOfImageTarget.transform.localScale = standScaol * dx;
            
        }
    }
}
