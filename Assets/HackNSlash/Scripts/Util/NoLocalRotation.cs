using System;
using UnityEngine;

namespace HackNSlash.Scripts.Util
{
    public class NoLocalRotation : MonoBehaviour
    {
        private Quaternion _lastParentRotation;

        private void Start()
        {
            _lastParentRotation = Quaternion.Inverse(transform.parent.root.localRotation);
        }

        void Update()
        {
            transform.localRotation = Quaternion.Inverse(transform.parent.localRotation) *
                                      _lastParentRotation *
                                      transform.localRotation;
            
            _lastParentRotation = transform.parent.localRotation;
        }
    }
}
