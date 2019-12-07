using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace huynhdo
{
    public class Demo : MonoBehaviour
    {
        [SerializeField] private Showing3DModelInUIHelper Hero;
        [SerializeField] private GameObject Object3D;

        private void Start()
        {
            Hero.Init(Object3D, 1f);
        }
    }
}