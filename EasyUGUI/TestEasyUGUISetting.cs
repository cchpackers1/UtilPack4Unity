using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUGUI;

[CreateAssetMenu(fileName = "TestEasyUGUISetting.asset", menuName = "Custom/Create TestEasyUGUISetting")]
public class TestEasyUGUISetting : EasyUGUISetting
{
    [EasyUGUIControllable]
    public float foo;
    [EasyUGUIControllable]
    [SerializeField]
    private int bar;

    public enum Type
    {
        AAAA,
        BBBB,
        CCCC
    }
    [EasyUGUIControllable]
    [SerializeField]
    Type type;
}
