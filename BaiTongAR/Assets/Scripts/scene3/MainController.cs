using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using UnityEngine.SceneManagement;
using System.IO;
public class MainController : MonoBehaviour {


    public Sprite[] leftOrRightArr;
    public Button LeftOrRightButton;
   

    bool isRight = false;
    public GameObject penal;
    public GameObject TishiInfoButton;

	public Animator PanelAnimator;
    public Animator InfoPanelAnimator;


    float time;

    void Start()
    {

        TishiInfoButton.SetActive(false);
        StartCoroutine(loaduiimage());
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(DynamicSettingImageTarget);
    }

    IEnumerator loaduiimage()
    {
        var path = Application.streamingAssetsPath + "/ui/tishi.png";
        WWW www = new WWW(path);
        yield return www;

        if(www.error == null)
        {
            var sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height),
                                        new Vector2(0.5f, 0.5f));
            TishiInfoButton.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
        }
    }



    void Update () {
        if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
            Quit();
            
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            ZhuYe();
        }
    }

    public string logStr;


    /// <summary>
    /// 动态设置 ImageTarget
    /// </summary>
     void DynamicSettingImageTarget()
    {
        var arImageTargetRootObj = GameObject.Find("ARImageTargetRoot");
        foreach (var tbs in TrackerManager.Instance.GetStateManager().GetTrackableBehaviours())
        {
            if (tbs.name == "New Game Object")
            {
                tbs.gameObject.transform.parent = arImageTargetRootObj.transform;
                tbs.gameObject.name = tbs.TrackableName + "ImageTarget";
                tbs.gameObject.AddComponent<TrackableEventHandler>();
                tbs.gameObject.AddComponent<TurnOffBehaviour>();

                var loadobj = Resources.Load<GameObject>("BaiTongAR/Models/" + tbs.TrackableName);
                if(loadobj)
                {
                    var obj = Instantiate(loadobj);
                    obj.name = tbs.TrackableName + "Model";
                    obj.transform.parent = tbs.gameObject.transform;
                    obj.transform.localPosition = new Vector3(0, 0.5f, 0);
                    obj.transform.localRotation = Quaternion.identity;
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    if (tbs.TrackableName == "dahuang")
                        obj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    if (tbs.TrackableName == "qinghao")
                        obj.transform.localScale = new Vector3(2f, 2f, 2f);
                    if (tbs.TrackableName == "zexie")
                        obj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                    // obj.AddComponent<BoxCollider>();
                    obj.AddComponent<MaterlController>();

                    if(obj.transform.childCount == 0)
                    {
                        var mr = obj.GetComponent<MeshRenderer>();
                        if (mr)
                        {
                            obj.AddComponent<MeshCollider>();
                            obj.tag = "trackableObject";

                        }
                    }
                    else
                    {
                        for(var i = 0; i < obj.transform.childCount; ++i)
                        {
                            var c = obj.transform.GetChild(i).gameObject;
                            if (c.GetComponent<MeshRenderer>())
                            {
                                c.AddComponent<MeshCollider>();
                                c.name = obj.name;
                                c.tag = "trackableObject";
                            }
                        }
                    }

                    obj.SetActive(true);

                }
            }
        }




        // 摄像头自动对焦
        if(!CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO))
        {
            if(!CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO))
            {
                logStr = "设置对焦失败!";
                
            }else
            {
                logStr = "触摸对焦成功!";
            }
        }else
        {
            logStr = "自动对焦成功!";
        }
        TrackableEventManager.Instance.cameraLogStr = logStr;
    }




    // --------------------  退出、提示、主页、截屏、隐藏和拉出 按钮点击事件  ------------------

    public void Quit()
    {
        Application.Quit();
    }
    public void Tishi()
    {
        TishiInfoButton.SetActive(true);
    }
    public void TishiButtonClose()
    {
        TishiInfoButton.SetActive(false);
    }

    public void ZhuYe()
    {

        CameraDevice.Instance.Stop();


        foreach(var tbs in TrackerManager.Instance.GetStateManager().GetTrackableBehaviours())
        {
            for (var i = 0; i < tbs.transform.childCount; ++i)
                DestroyImmediate(tbs.transform.GetChild(i).gameObject);
        }

        DestroyImmediate(GameObject.Find("ARImageTargetRoot"));

        SceneManager.LoadScene("launch", LoadSceneMode.Single);
    }


    IEnumerator jietu(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);


		System.DateTime now = new System.DateTime();
		now = System.DateTime.Now;
		string filename = string.Format("image{0}{1}{2}{3}{4}{5}.png",
										now.Year, now.Month, now.Day,
										now.Hour, now.Minute, now.Second);

		Rect rect = new Rect(0, 0, Display.main.systemWidth, Display.main.systemHeight);
		// 创建空纹理
		Texture2D screenShotTexture2d = new Texture2D((int)rect.width, (int)rect.height,
													  TextureFormat.RGB24, false);

		// 读取屏幕像素信息并存储为纹理数据
		screenShotTexture2d.ReadPixels(rect, 0, 0);
		screenShotTexture2d.Apply();

		// 转换为png
		byte[] bytes = screenShotTexture2d.EncodeToPNG();

		// 保存至本地
		string savepath = "/sdcard/百通神器",
		savepath1 = "/sdcard/Pictures/Screenshots",
		savepath2 = "/sdcard/DCIM/Screenshots";
		if (!Directory.Exists(savepath1))
		{
			if (!Directory.Exists(savepath2))
			{
				if (!Directory.Exists(savepath))
					Directory.CreateDirectory(savepath);
			}
			else
			{
				savepath = savepath2;
			}
		}
		else
		{
			savepath = savepath1;
		}

		savepath += "/" + filename;
		File.WriteAllBytes(savepath, bytes);
    }

    public void JieTu()
    {
        LeftOrRight();
        StartCoroutine(jietu(0.5f));
    }



	public void LeftOrRight()
	{

		Debug.Log(isRight);
		if (isRight)
		{
			isRight = false;
			LeftOrRightButton.image.sprite = leftOrRightArr[1];
			PanelAnimator.Play("panel");
		}
		else
		{
			isRight = true;
			LeftOrRightButton.image.sprite = leftOrRightArr[0];

			PanelAnimator.Play("showPanel");
		}
	}






    // ----------------------  底部信息UI的返回按钮点击事件  -------------------

    public void button()
    {
        var animaInfo = InfoPanelAnimator.GetCurrentAnimatorStateInfo(0);
        if(!animaInfo.IsName("hideInfoPanel"))
        {
			InfoPanelAnimator.Play("hideInfoPanel");
        }
       
    }
}
