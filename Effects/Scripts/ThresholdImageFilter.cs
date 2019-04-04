using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UtilPack4Unity
{
    public class ThresholdImageFilter : GrabbableImageFilter
    {
        [SerializeField]
        float threshold;

        private void Reset()
        {
            this.shader = Shader.Find("UtilPack4Unity/Filter/ThresholdImageFilter");
        }
        void Update()
        {
            material.SetFloat("_Threshold", threshold);
        }
    }
}
