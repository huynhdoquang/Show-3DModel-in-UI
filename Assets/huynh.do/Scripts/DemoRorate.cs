using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace huynhdo
{
    public class DemoRorate : MonoBehaviour
    {
        public float xAngle, yAngle, zAngle;

        void Update()
        {
            transform.Rotate(xAngle, yAngle, zAngle, Space.World);
        }
    }
}