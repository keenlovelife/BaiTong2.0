using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AboutController : MonoBehaviour {


    public Button AboutButton;
    public Sprite AboutSprite;
    public GameObject Content;
    void Start () {
        StartCoroutine(loaduiimage());
    }

    IEnumerator loaduiimage()
    {
        var path = Application.streamingAssetsPath + "/ui/about2.png";
        WWW www = new WWW(path);
        yield return www;

        if (www.error == null)
        {
            AboutSprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height),
                                        new Vector2(0.5f, 0.5f));
            AboutButton.image.sprite = AboutSprite;
			AboutButton.transform.GetComponent<RectTransform>().sizeDelta = new
			   Vector2(Display.main.systemWidth,
					   Display.main.systemWidth * (AboutSprite.textureRect.size.y / (float)AboutSprite.textureRect.size.x));
			var tr = Content.GetComponent<RectTransform>();
			tr.sizeDelta = AboutButton.transform.GetComponent<RectTransform>().sizeDelta;
			tr.anchoredPosition3D = new Vector3(0, 0, 0);
        }
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Scenes/1", LoadSceneMode.Single);
    }
}
