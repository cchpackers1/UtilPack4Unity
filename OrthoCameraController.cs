using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Persistence;

public class OrthoCameraController : MonoBehaviour
{
    Camera cam;
    public Camera Cam
    {
        get
        {
            return cam;
        }
    }

    [SerializeField]
    float minSize = 0.01f;
    [SerializeField]
    float maxSize = 100f;
    [SerializeField]
    KeyCode keyCode;
    public KeyCode KeyCode
    {
        get {
            return this.keyCode;
        }
    }
    private Vector3 preMousePosition;
    public float moveSpeed = 1f;
    public float zoomSpeed = 1f;
    
    private Vector3 defaultPosition;
    public float defaultSize = 1f;
    [SerializeField]
    KeyCode resetKey;

    public bool IsEnableControl = true;

    [SerializeField]
    string settingFileName;

    private void Awake()
    {
        defaultPosition = this.transform.localPosition;
        cam = GetComponent<Camera>();
    }
    // Use this for initialization
    void Start()
    {
        Restore();
    }

    public void Clear()
    {
        this.cam.orthographicSize = defaultSize;
        this.transform.localPosition = defaultPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsEnableControl) return;
        if (Input.GetKeyDown(resetKey))
        {
            Clear();
        }
        if (Input.GetKey(keyCode))
        {
            ChangeSize();
            Move();
        }
        preMousePosition = Input.mousePosition;
    }

    void ChangeSize()
    {
        var size = cam.orthographicSize;
        size -= Input.mouseScrollDelta.y*zoomSpeed*Time.deltaTime;
        size = Mathf.Clamp(size, minSize, maxSize);
        this.cam.orthographicSize = size;
    }

    void Move()
    {
        if (Input.GetMouseButton(1))
        {
            var pos = this.transform.position;
            var mouseMove = Input.mousePosition - preMousePosition;
            mouseMove.z = 0f;
            mouseMove *= -1f;
            var positionMove = this.transform.right * mouseMove.x + this.transform.up * mouseMove.y;
            this.transform.localPosition += positionMove * moveSpeed * Time.deltaTime*(cam.orthographicSize/defaultSize);
        }
    }

    public void  Restore()
    {
        var info = IOHandler.LoadJson<OrthograhicCameraInfo>(IOHandler.IntoStreamingAssets(settingFileName));
        if (info == null) return;
        this.transform.position = info.transformInfo.Position.ToVector3();
        this.transform.eulerAngles = info.transformInfo.EulerAngles.ToVector3();
        this.cam.orthographicSize = info.OrthograhicSize;
    }

    public void Save()
    {
        var info = new OrthograhicCameraInfo();
        info.OrthograhicSize = cam.orthographicSize;
        info.transformInfo = new TransformInfo(this.transform);
        IOHandler.SaveJson(IOHandler.IntoStreamingAssets(settingFileName), info);
    }
}

public class OrthograhicCameraInfo
{
    public TransformInfo transformInfo;
    public float OrthograhicSize;
}
