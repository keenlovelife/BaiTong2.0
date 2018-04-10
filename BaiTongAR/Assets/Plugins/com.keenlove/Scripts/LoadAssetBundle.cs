using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace keenlove
{
    public class LoadAssetBundle : MonoBehaviour
    {
        public static LoadAssetBundle Instance { get { return instance; } }
        static LoadAssetBundle instance;
        private void Awake(){  instance = this; }
        private void Start()
        {
            
        }

        public static void Load(string path, string who, Action_1<object> callback)
        {
            Instance.StartCoroutine(Instance._Load(path, who, callback));
        }
        public static void LoadAsync(string path, string who, Action_1<object> callback)
        {
           // Instance.StartCoroutine(Instance._LoadAsync(path, who, callback));
        }
        IEnumerator _Load(string path, string who, Action_1<object> callback)
        {
            Debug.Log("===> LoadAssetBundle.Load path:" + path + " who:" + who);
            var www = new WWW(path);
            yield return www;
            Debug.Log("===> LoadAssetBundle.Load 完成");
            if (www.error == null)
            {
                var obj = www.assetBundle.LoadAsset(who);
                if (callback != null)
                    callback(obj);
            }else
            {
                if (callback != null)
                    callback(null);
            }
           
        }
        IEnumerator _LoadAsync(string path, string who, Action_1<WWW> callback)
        {
            Debug.Log("===> LoadAssetBundle.LoadAsync path:" + path + " who:" + who);
            var www = new WWW(path);
            yield return www;
            if (callback != null)
                callback(www);
        }
    }
}
