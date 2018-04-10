using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class TrackableEventHandler : MonoBehaviour, ITrackableEventHandler {
    
    public string TrackableName;

    TrackableBehaviour mTrackableBehaviour;



    string logStr;
    void Start () {
        if (mTrackableBehaviour = GetComponent<TrackableBehaviour>())
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
	}



    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)

            OnTrackingFund();
        else
            OnTrackingLost();
        
    }


    Dictionary<string, Vector3> targetModelDic = new Dictionary<string, Vector3>();


    /// <summary>
    /// 找到识别物时
    /// </summary>
    private void OnTrackingFund()
    {
        var str = " >> 找到一张识别图:" + mTrackableBehaviour.name + "--" + mTrackableBehaviour.TrackableName;
        Debug.Log(str);
        logStr = str;
		TrackableEventManager.Instance.trackableLogStr = logStr;
		
        TrackableEventManager.Instance.IsFoundOneImageTarget = true;

        if(transform.childCount != 0)
        {

            TrackableEventManager.Instance.CurrentShowModelObjectOfImageTarget = transform.GetChild(0).gameObject;

            if(targetModelDic.ContainsKey(mTrackableBehaviour.TrackableName))
            {
                transform.GetChild(0).gameObject.transform.localScale = targetModelDic[mTrackableBehaviour.TrackableName];

            }else
            {
				var d = transform.GetChild(0).gameObject.transform.localScale;
				targetModelDic[mTrackableBehaviour.TrackableName] = new Vector3(d.x, d.y, d.z);
            }
      
          

//////////////////////////////////////      显示模型        //////////////////////////////////////////////////////////////

            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            foreach (Renderer component in rendererComponents)
                component.enabled = true;
            foreach (Collider component in colliderComponents)
                component.enabled = true;
        }
    }
    /// <summary>
    /// 失去识别物时
    /// </summary>
    private void OnTrackingLost()
    {
        TrackableEventManager.Instance.IsFoundOneImageTarget = false;

        logStr = " >> 正在找下一个目标图片!";
        Debug.Log(logStr);
		TrackableEventManager.Instance.trackableLogStr = logStr;


		if(transform.childCount != 0)
        {


            TrackableEventManager.Instance.CurrentShowModelObjectOfImageTarget = null;




//////////////////////////////////////      隐藏模型        //////////////////////////////////////////////////////////////

            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            foreach (Renderer component in rendererComponents)
                component.enabled = false;
            foreach (Collider component in colliderComponents)
                component.enabled = false;
        }
    }
}
