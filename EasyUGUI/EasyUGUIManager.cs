using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq;
using UtilPack4Unity;

namespace EasyUGUI
{
    public class EasyUGUIManager : MonoBehaviour
    {
        [SerializeField]
        private EasyUGUIPrefabSetting prefabSetting;
        [SerializeField]
        protected EasyUGUISetting setting;
        List<FieldControlPair> pairList;

        [SerializeField]
        string fileName;
        [SerializeField]
        private Transform uiParent;

        private void Start()
        {
            print(prefabSetting);
            Init();
            Restore();
        }

        private void Init()
        {
            var fields = setting.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            pairList = new List<FieldControlPair>();
            foreach (var field in fields)
            {
                var controllableAttribute = field.GetCustomAttribute<EasyUGUIControllableAttribute>();
                if (controllableAttribute == null) continue;
                var type = field.FieldType;
                var rangeAttribute = field.GetCustomAttribute<RangeAttribute>();
                if (type.IsEnum)
                {

                }
                else if (type.Name == typeof(float).Name)
                {
                    if (rangeAttribute != null)
                    {
                        var go = Instantiate(prefabSetting.FloatSliderPrefab) as GameObject;
                        go.transform.SetParent(uiParent, false);
                        var component = go.GetComponent<EasyUGUIFloatSlider>();
                        component.MinValue = rangeAttribute.min;
                        component.MaxValue = rangeAttribute.max;
                        component.Id = field.Name;
                        pairList.Add(new FieldControlPair(field, component));
                    }
                }
                else if (type.Name == typeof(int).Name)
                {
                    if (rangeAttribute != null)
                    {
                        var go = Instantiate(prefabSetting.IntSliderPrefab) as GameObject;
                        go.transform.SetParent(uiParent, false);
                        var component = go.GetComponent<EasyUGUIIntSlider>();
                        component.Id = field.Name;
                        component.MinValue = (int)rangeAttribute.min;
                        component.MaxValue = (int)rangeAttribute.max;
                        pairList.Add(new FieldControlPair(field, component));
                    }
                }
                else if (type.Name == typeof(string).Name)
                {

                }
                else if (type.Name == typeof(bool).Name)
                {

                }
            }

            foreach (var pair in pairList)
            {
                pair.easyUGUIControl.ValueChangedEvent += Control_ValueChangedEvent;
            }
        }

        private void Control_ValueChangedEvent(EasyUGUIControl control, object value)
        {
            var field = pairList.FirstOrDefault(e => e.fieldInfo.Name == control.Id).fieldInfo;
            if (field == null) return;
            print("change");
            field.SetValue(setting, value);
            setting.OnUpdate(control.Id, value);
        }

        public void Restore()
        {
            var list = IOHandler.LoadJson<List<FieldInfomation>>(IOHandler.IntoStreamingAssets(fileName));
            if (list == null) return;

            foreach (var elm in list)
            {
                var pair = pairList.FirstOrDefault(e => e.fieldInfo.Name == elm.FieldName && e.fieldInfo.FieldType.Name == elm.TypeName);
                var value = Convert.ChangeType(elm.Value, pair.fieldInfo.FieldType);
                pair.fieldInfo.SetValue(setting, value);
                pair.easyUGUIControl.SetValue(value);
                //var value = 
            }
        }

        [ContextMenu("Save")]
        public void Save()
        {

            var list = new List<FieldInfomation>();
            foreach (var pair in pairList)
            {
                var value = pair.fieldInfo.GetValue(setting);

                var typeName = pair.fieldInfo.FieldType.Name;
                var fieldName = pair.fieldInfo.Name;
                var info = new FieldInfomation(fieldName, typeName, value);
                list.Add(info);
            }
            IOHandler.SaveJson(IOHandler.IntoStreamingAssets(fileName), list);
        }

        public class FieldControlPair
        {
            public FieldInfo fieldInfo { get; set; }
            public EasyUGUIControl easyUGUIControl { get; set; }
            public FieldControlPair() { }
            public FieldControlPair(FieldInfo fieldInfo, EasyUGUIControl easyUGUIControl)
            {
                this.fieldInfo = fieldInfo;
                this.easyUGUIControl = easyUGUIControl;
            }
        }

        public class FieldInfomation
        {
            public string TypeName;
            public string FieldName;
            public object Value;

            public FieldInfomation() { }
            public FieldInfomation(string fieldName, string typeName, object value)
            {
                this.TypeName = typeName;
                this.FieldName = fieldName;
                this.Value = value;
            }
        }
    }
}