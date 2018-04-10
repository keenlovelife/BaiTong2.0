using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixSquare : MonoBehaviour {

    
    void Start () {
        int width = 0;
        if (Display.main.systemHeight > Display.main.systemWidth)
            width = (int)GetComponent<RectTransform>().rect.width;
       else
            width = (int)GetComponent<RectTransform>().rect.height;
        gameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(width, width);
	}


    void Update () {
		
	}
}
