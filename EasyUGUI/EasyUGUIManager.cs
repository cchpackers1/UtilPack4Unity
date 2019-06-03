using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq;

namespace EasyUGUI
{
    public class EasyUGUIManager : MonoBehaviour
    {
        [SerializeField]
        private EasyUGUIPrefabSetting prefabSetting;
        [SerializeField]
        protected EasyUGUISetting setting;
        List<FieldInfo> fieldInfoList;

        List<EasyUGUIControl> uiControls;
        [SerializeField]
        string fileName;
        [SerializeField]
        private Transform uiParent;

        private void Start()
        {
            print(prefabSetting);
            Init();
        }

        private void Init()
        {
            var fields = setting.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            uiControls = new List<EasyUGUIControl>();
            fieldInfoList = new List<FieldInfo>();
            foreach (var field in fields)
            {
                var controllableAttribute = field.GetCustomAttribute<EasyUGUIControllableAttribute>();
                if (controllableAttribute == null) continue;
                var type = field.FieldType;
                var rangeAttribute = field.GetCustomAttribute<RangeAttribute>();
                if (type.Name == typeof(float).Name)
                {
                    if (rangeAttribute != null)
                    {
                        var go = Instantiate(prefabSetting.FloatSliderPrefab) as GameObject;
                        go.transform.SetParent(uiParent, false);
                        var component = go.GetComponent<EasyUGUIControl>();
                        component.Id = field.Name;
                        uiControls.Add(component);
                        fieldInfoList.Add(field);
                    }
                }
                else if (type.Name == typeof(int).Name)
                {
                    if (rangeAttribute != null)
                    {
                        var go = Instantiate(prefabSetting.IntSliderPrefab) as GameObject;
                        go.transform.SetParent(uiParent, false);
                        var component = go.GetComponent<EasyUGUIControl>();
                        component.Id = field.Name;
                        uiControls.Add(component);
                        fieldInfoList.Add(field);
                    }
                }
                else if (type.Name == typeof(string).Name)
                {

                }
                else if (type.Name == typeof(bool).Name)
                {

                }
            }

            foreach (var control in uiControls)
            {
                control.ValueChangedEvent += Control_ValueChangedEvent;
            }
        }

        private void Control_ValueChangedEvent(EasyUGUIControl control, object value)
        {
            var field = fieldInfoList.FirstOrDefault(e => e.Name == control.Id);
            if (field == null) return;
            print("change");
            field.SetValue(setting, value);
            setting.OnUpdate(control.Id, value);
        }

        public void Restore()
        {

        }

        public void Save()
        {

        }
    }
}