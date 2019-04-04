using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class UniHttpRequestHandler : MonoBehaviour
{
    [SerializeField]
    protected string path;
    [SerializeField]
    protected UniHttpServer server;

    protected virtual void Start()
    {
        server.AddListener(path, Server_OnRequest);
    }

    protected virtual void Server_OnRequest(System.Net.HttpListenerContext context)
    {
        var response = context.Response;
        var message = "OnPost";
        var bytes = Encoding.UTF8.GetBytes(message);
        response.OutputStream.Write(bytes, 0, bytes.Length);
        response.Close();
    }

    protected virtual void OnError(System.Net.HttpListenerContext context)
    {
        var response = context.Response;
        var message = "error";
        var bytes = Encoding.UTF8.GetBytes(message);
        response.OutputStream.Write(bytes, 0, bytes.Length);
        response.Close();
    }
}
