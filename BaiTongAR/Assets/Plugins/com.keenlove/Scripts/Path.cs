using System.Collections;
using UnityEngine;

namespace keenlove
{
    public class Path : MonoBehaviour
    {
        public static string StreamingAssets(string path)
        {
            var p = Application.streamingAssetsPath + "/" + path;
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.IPhonePlayer)
                p = "file://" + p;
            return p;
        }
    }
}