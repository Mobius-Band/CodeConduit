using System;
using UnityEngine;

namespace HackNSlash.Scripts.GamePlayFlowManagement
{
    [Serializable]
    public class TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public static implicit operator TransformData(Transform transform)
        {
            return new TransformData
            {
                position = transform.position,
                rotation = transform.rotation,
                scale = transform.localScale
            };
        }

        public void SetTransformData(Transform transformToSet)
        {
            transformToSet.position = position;
            transformToSet.rotation = rotation;
            transformToSet.localScale = scale;
        }
    }
}