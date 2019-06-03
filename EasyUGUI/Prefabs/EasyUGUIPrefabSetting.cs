using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUGUI {
    [CreateAssetMenu(fileName = "EasyUGUIPrefabSetting.asset", menuName = "Custom/Create EasyUGUIPrefabSetting")]
    public class EasyUGUIPrefabSetting : ScriptableObject
    {
        [SerializeField]
        private GameObject floatSliderPrefab;
        public GameObject FloatSliderPrefab
        {
            get
            {
                return floatSliderPrefab;
            }
        }

        [SerializeField]
        private GameObject intSliderPrefab;
        public GameObject IntSliderPrefab
        {
            get
            {
                return intSliderPrefab;
            }
        }
    }
}
