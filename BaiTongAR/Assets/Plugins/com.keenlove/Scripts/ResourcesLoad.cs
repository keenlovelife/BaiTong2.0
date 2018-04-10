using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace keenlove
{

    public class ResourcesLoad : MonoBehaviour
    {
        static ResourceRequest rr;
        static Action_3<float, Object, ResourceRequest> cb;
        public static void Async<T>(string path, Action_3<float, Object, ResourceRequest> callback) where T : Object
        {
            cb = callback;
            rr = Resources.LoadAsync<T>(path);
        }


        void Update()
        {
            if(rr != null && !rr.isDone)
            {
                if (cb != null)
                    cb(rr.progress, null,rr);
            }else if(rr != null)
            {
                if (cb != null)
                    cb(1, rr.asset, null);
                rr = null;
            }
        }
    }
}