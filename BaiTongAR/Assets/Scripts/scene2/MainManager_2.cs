using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager_2 : MonoBehaviour {

    public Image bgImang;
    public Image diquanImage;
    public Image image;

    public Text text;
    public Image GuangeQuanImage;
    AsyncOperation asyc;

    float time = 0.0f, wiatTime = 1.0f;
    float pre = 0.0f;

    AsyncOperation ao;
    void Start () {
        StartCoroutine(loaduiimage("/ui/Loading-Bg.png",bgImang));
        StartCoroutine(loaduiimage("/ui/diquan.png", diquanImage));
        StartCoroutine(loaduiimage("/ui/shangquan.png", image));


		image.fillAmount = 0;
        text.text = "0%";

        ao = SceneManager.LoadSceneAsync("Scenes/3",LoadSceneMode.Single);
        ao.allowSceneActivation = false;
    }

    IEnumerator loaduiimage(string imagepath, Image img)
    {
        var path = Application.streamingAssetsPath + imagepath;
        WWW www = new WWW(path);
        yield return www;

        if (www.error == null)
        {
            img.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height),
                                        new Vector2(0.5f, 0.5f));
        }
    }

    private void Update()
    {
		if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
			SceneManager.LoadScene("Scenes/1", LoadSceneMode.Single);
        

        if (time < wiatTime)
        {
            
            time += Time.deltaTime;
            image.fillAmount = (time / wiatTime) * 0.9f;
            text.text = (int)(image.fillAmount * 100) + "%";
            guangquan();
        }
        else if (ao.progress == 0.9f)
        {
            image.fillAmount = 1;
            text.text = "100%";
            guangquan();
            //SceneManager.LoadScene("Scenes/3", LoadSceneMode.Single);
            ao.allowSceneActivation = true;
		}
    }
    
    /// <summary>
    /// 处理光圈image 的旋转
    /// </summary>
    void guangquan()
    {
        var point = new Vector2(image.transform.position.x, image.transform.position.y);
        GuangeQuanImage.transform.RotateAround(point, new Vector3(0, 0, 1), -(image.fillAmount - pre) * 360);
        pre = image.fillAmount;
    }


}
