using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using UnityEngine;

namespace ValheimInternalCSharp
{
    public class Loader
    {
        public static GameObject _Load;
        public static void Load()
        {

            _Load = new GameObject();
            _Load.AddComponent<Cheat>();
            GameObject.DontDestroyOnLoad(_Load);

        }
        public static void Unload()
        {

            _Unload();
        }
        private static void _Unload()
        {
            GameObject.Destroy(_Load);
        }
        //private GameObject _gameObject;
    }
}