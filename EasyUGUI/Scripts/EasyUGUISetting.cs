using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EasyUGUISetting : ScriptableObject
{
    public event Action UpdateEvent;
    public void OnUpdate()
    {
        UpdateEvent?.Invoke();
    }
}
