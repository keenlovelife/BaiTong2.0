using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterlController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        setShaderArg(gameObject);

	}

    void setShaderArg(GameObject go)
    {
        var tf = go.GetComponent<Transform>();
        if(tf.childCount == 0)
        {
            var mr = go.GetComponent<MeshRenderer>();
            if(mr)
            {
                mr.material.SetFloat("_Glossiness", 0.0f);
            }

        }else
        {
            for (var i = 0; i < tf.childCount;++i)
            {
                setShaderArg(tf.GetChild(i).gameObject);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	}
}
