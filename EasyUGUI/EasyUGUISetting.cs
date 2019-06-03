using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EasyUGUISetting : ScriptableObject
{
    public event Action<EasyUGUISetting, string, object> UpdateEvent;
    public void OnUpdate(string id, object value)
    {
        UpdateEvent?.Invoke(this, id, value);
    }
}
