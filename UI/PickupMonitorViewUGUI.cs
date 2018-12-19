using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupMonitorViewUGUI : MonoBehaviour {
    [SerializeField]
    MultiMonitorManager monitorManager;
    [SerializeField]
    RawImage rawImage;

    private void Awake()
    {
        monitorManager.ClickEvent += MonitorManager_ClickEvent;
    }

    private void MonitorManager_ClickEvent(MonitorView monitorView)
    {
        rawImage.texture = monitorView.Texture;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
