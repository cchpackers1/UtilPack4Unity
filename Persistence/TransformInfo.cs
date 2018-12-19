using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistence
{
    public class TransformInfo
    {
        public TypeUtils.Json.Vec3 Position;
        public TypeUtils.Json.Vec3 Angles;
        public TypeUtils.Json.Vec3 Scale;
        public static TransformInfo identity
        {
            get {
                return new TransformInfo(Vector3.zero, Vector3.zero, Vector3.one);
            }
        }

        public TransformInfo()
        {

        }

        public TransformInfo(Transform transform)
        {
            this.Position = TypeUtils.Json.Convert.Vector3ToVec3(transform.position);
            this.Angles = TypeUtils.Json.Convert.Vector3ToVec3(transform.localEulerAngles);
            this.Scale = TypeUtils.Json.Convert.Vector3ToVec3(transform.localScale);
        }

        public TransformInfo(Vector3 position, Vector3 angles, Vector3 scale)
        {
            this.Position = TypeUtils.Json.Convert.Vector3ToVec3(position);
            this.Angles = TypeUtils.Json.Convert.Vector3ToVec3(angles);
            this.Scale = TypeUtils.Json.Convert.Vector3ToVec3(scale);
        }
    }

}
