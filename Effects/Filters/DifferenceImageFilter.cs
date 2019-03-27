﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferenceImageFilter : GrabbableImageFilter {
    [SerializeField]
    protected float threshold;

    protected RenderTexture[] rts;
    public void Capture()
    {
        Graphics.Blit(rts[0], rts[1]);
    }

    private void Update()
    {
        this.material.SetFloat("_Threshold", threshold);
        this.material.SetTexture("_CacheTex",rts[1]);
    }

    public override void Filter(Texture source, RenderTexture destination)
    {
        SecureRenderTexures(source);
        Graphics.Blit(source, rts[0]);
        base.Filter(rts[0], destination);
    }

    private void SecureRenderTexures(Texture texture)
    {
        if (rts == null)
        {
            InitRenderTextures(texture);
        }
        else if (texture.width != this.rts[0].width || texture.height != this.rts[0].height)
        {
            InitRenderTextures(texture);
        }
    }

    private void InitRenderTextures(Texture texture)
    {
        Release();
        this.rts = new RenderTexture[] { new RenderTexture(texture.width, texture.height, 24),
                 new RenderTexture(texture.width, texture.height, 24) };
    }



    void Release()
    {
        if (this.rts != null)
        {
            for (var i = 0; i < rts.Length; i++)
            {
                rts[i].Release();
                DestroyImmediate(rts[i]);
                rts[i] = null;
            }
            this.rts = null;
        }
    }

    protected override void OnDestroy()
    {
        Release();
        base.OnDestroy();
    }
}
