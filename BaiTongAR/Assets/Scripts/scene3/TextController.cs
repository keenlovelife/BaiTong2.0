using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextController : MonoBehaviour {

    Text text;
    void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        text.text = TrackableEventManager.Instance.LogStr;

	}
}
