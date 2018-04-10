using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour {


    public Image bgImage;

    void Start () {
        StartCoroutine(loaduiimage());
	}

    IEnumerator loaduiimage()
    {
        var path = Application.streamingAssetsPath + "/ui/background.png";
        WWW www = new WWW(path);
        yield return www;

        if (www.error == null)
        {
            bgImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height),
                                        new Vector2(0.5f, 0.5f));
        }
    }
    void Update () {
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
	}

}
