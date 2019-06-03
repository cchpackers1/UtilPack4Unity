using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyUGUI
{
    public class EasyUGUITextInputField : EasyUGUIControl
    {
        [SerializeField]
        InputField inputField;

        public override void SetValue(object value)
        {
            SetValue((string)value);
        }

        protected void SetValue(string value)
        {
            inputField.text = value;
        }

        public void OnValueChanged(string value)
        {
            base.OnValueChanged(value);
        }
    }
}