using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using HttpMultipartParser;

public class UniHttpPostRequestHandlerBase : UniHttpRequestHandler
{
    protected override void Server_OnRequest(HttpListenerContext context)
    {
        try
        {
            var parser = new MultipartFormDataParser(context.Request.InputStream);
            OnPost(context, parser);
        }
        catch
        {
            this.OnError(context);
        }
    }

    protected virtual void OnPost(HttpListenerContext context, MultipartFormDataParser parser, string responseMessage = null)
    {

        if (string.IsNullOrEmpty(responseMessage))
        {
            responseMessage = "responseMessage";
        }

        {
            var response = context.Response;
            var bytes = System.Text.Encoding.UTF8.GetBytes(responseMessage);
            response.OutputStream.Write(bytes, 0, bytes.Length);
            response.Close();
        }

    }
}
