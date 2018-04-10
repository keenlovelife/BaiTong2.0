using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace keenlove
{
    public delegate void Action_1<T>(T obj);
    public delegate void Action_2<T1, T2>(T1 obj1, T2 obj2);
    public delegate void Action_3<T1, T2,T3>(T1 obj1, T2 obj2,T3 obj3);

    public class KeenLove : MonoBehaviour
    {
        static KeenLove instance;
        public static KeenLove Instance { get { return instance; } }
        private void Awake()
        {
            instance = this;
        }

        public static Vector2 StandardSize = new Vector2(375, 667);
        public static float AdaptedDistance(float designDistance,float standardDistance, bool isWidth = true)
        {
            if (isWidth)
                return (designDistance / standardDistance) * Screen.width;
            else
                return (designDistance / standardDistance) * Screen.height;
        }

        void Start()
        {
            if (GameObject.Find("com.keenlove") == null)
            {
                DontDestroyOnLoad(gameObject);
                if (gameObject.GetComponent<LoadAssetBundle>() == null)
                    gameObject.AddComponent<LoadAssetBundle>();
                if (gameObject.GetComponent<ResourcesLoad>() == null)
                    gameObject.AddComponent<ResourcesLoad>();
            }
        }

        void Update()
        {

        }
    }
}