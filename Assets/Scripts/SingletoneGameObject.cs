﻿
using System;
using UnityEngine;

namespace PolygonCrazyShooter
{
    public class SingletoneGameObject<T> : MonoBehaviour where T: SingletoneGameObject<T>
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    var holderObject = new GameObject($"Sinhleton_{typeof(T)}");
                    _instance = holderObject.AddComponent<T>();
                    DontDestroyOnLoad(holderObject);
                }

                return _instance;
            }
        }

        public static T TryInstance()
        {
            return _instance;
        }

        private static T _instance = null;
        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
                throw new Exception("Singleton two init!!!");

            _instance = (T)this;

            DontDestroyOnLoad(this.gameObject);
        }
    }
}
