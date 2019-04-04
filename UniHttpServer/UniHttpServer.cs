using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using System.Net;
using System;
using System.Linq;

public class UniHttpServer : MonoBehaviour {
    private HttpListener httpListener;
    [SerializeField]
    string uri = "http://*";
    [SerializeField]
    int port;
    [SerializeField]
    string path = "/";

    private string url;


    public List<HttpRequestHandlerInfo> HttpRequestHandlerInfoList
    {
        get;
        private set;
    }

    private Task task;
    bool isRunning;
    [SerializeField]
    bool isSync = true;

    Queue queue;


    private void Awake()
    {
        if (isSync)
        {
            queue = new Queue();
            queue = Queue.Synchronized(queue);
        }
        
        HttpRequestHandlerInfoList = new List<HttpRequestHandlerInfo>();
        httpListener = new HttpListener();
        if (!string.IsNullOrEmpty(path))
        {
            if (path.IndexOf('/') != 0)
            {
                path = "/" + path;
            }

            if (path.LastIndexOf('/') != path.Length - 1)
            {
                path += '/';
            }
        }
        else
        {
            path = "/";
        }
        
        url = uri + ":" + port + path;
        httpListener.Prefixes.Add(url);
        this.task = StartServer();
    }

    public bool AddListener(string path,  Action<HttpListenerContext> callback)
    {
        if (HttpRequestHandlerInfoList.Count(e => e.Path == path) > 0)
        {
            return false;
        }
        if (callback == null)
        {
            return false;
        }

        if (!string.IsNullOrEmpty(path))
        {
            if (path.IndexOf('/') != 0)
            {
                path = "/" + path;
            }

            if (path.LastIndexOf('/') != path.Length - 1)
            {
                path += '/';
            }
        }
        HttpRequestHandlerInfoList.Add(new HttpRequestHandlerInfo(path, callback));
        return true;
    }

    public async Task StartServer()
    {
        isRunning = true;
        httpListener.Start();

        while (isRunning)
        {
            var context = await httpListener.GetContextAsync();
            if (isSync)
            {
                queue.Enqueue(context);
            }
            else
            {
                ProcessContext(context);
            }
        }
    }

    private void ProcessContext(HttpListenerContext context)
    {
        var requestRawUrl = context.Request.RawUrl;
        //Debug.Log(requestRawUrl);
        if (!string.IsNullOrEmpty(requestRawUrl))
        {
            if (requestRawUrl.LastIndexOf('/') != requestRawUrl.Length - 1)
            {
                requestRawUrl += '/';
            }
        }


        //var requestPath = requestRawUrl.Replace(this.path, "");
        var requestPath = requestRawUrl;
        if (!string.IsNullOrEmpty(requestPath))
        {
            if (requestPath.IndexOf('/') != 0)
            {
                requestPath = '/' + requestPath;
            }
        }

        print("path : " + requestPath);
        var listener = HttpRequestHandlerInfoList.FirstOrDefault(e => e.Path == requestPath);
        if (listener != null)
        {
            listener.Callback(context);
        }
        else
        {
            var response = context.Response;
            response.StatusCode = 404;
            var message = "Not Found";
            var bytes = System.Text.Encoding.UTF8.GetBytes(message);
            response.OutputStream.Write(bytes, 0, bytes.Length);
            response.Close();
        }
    }
    
    public void StopServer()
    {
        isRunning = false;
        task.GetAwaiter().OnCompleted(
            () =>
            {
                httpListener.Abort();
            }
            );
    }

    private void Update()
    {
        if (!isSync) return;
        while (queue.Count > 0)
        {
            var context = queue.Dequeue() as HttpListenerContext;
            ProcessContext(context);
        }
    }

    void OnDestroy()
    {
        StopServer();
    }

    public class HttpRequestHandlerInfo
    {
        public string Path;
        public Action<HttpListenerContext> Callback;

        public HttpRequestHandlerInfo(string path, Action<HttpListenerContext> callback)
        {
            this.Path = path;
            this.Callback = callback;
        }
    }
}
