using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUGUI;

[CreateAssetMenu(fileName = "TestEasyUGUISetting.asset", menuName = "Custom/Create TestEasyUGUISetting")]
public class TestEasyUGUISetting : EasyUGUISetting
{
    [EasyUGUIControllable]
    [Range(0, 5)]
    public float foo;
    [EasyUGUIControllable]
    [SerializeField]
    [Range(0, 1)]
    private int bar;
}
