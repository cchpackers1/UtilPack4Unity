using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System;

public class MonitorSettingsManager : MonoBehaviour
{
    MonitorSettings settings;
    [SerializeField]
    string fileName;

    public event Action<string> OnError;
    
    private void Awake()
    {
        var path = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            settings = JsonConvert.DeserializeObject<MonitorSettings>(json);
            if (!settings.IsEnable)
            {
                return;
            }
            if (settings.TargetMonitor < 0 || settings.TargetMonitor >= Display.displays.Length)
            {
                OnError?.Invoke("選択されたモニター番号が正しくありません。");
                return;
            }

            PlayerPrefs.SetInt("UnitySelectMonitor", settings.TargetMonitor);
            var display = Display.displays[settings.TargetMonitor];
            int w = display.systemWidth;
            int h = display.systemHeight;
            Screen.SetResolution(w, h, Screen.fullScreen);
        }
        else
        {
            OnError?.Invoke("設定ファイルがありません。");
            return;
        }
    }
}
