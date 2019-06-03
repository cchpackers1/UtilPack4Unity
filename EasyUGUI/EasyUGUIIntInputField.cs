using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyUGUI
{
    public class EasyUGUIIntInputField : EasyUGUIControl
    {
        [SerializeField]
        InputField inputField;

        public override void SetValue(object value)
        {
            SetValue((int)value);
        }

        protected void SetValue(int value)
        {
            inputField.text = value.ToString();
        }

        public void OnValueChanged(string value)
        {
            var v = int.Parse(value);
            base.OnValueChanged(v);
        }
    }
}