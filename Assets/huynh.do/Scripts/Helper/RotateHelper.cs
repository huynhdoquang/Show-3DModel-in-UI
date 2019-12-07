using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace huynhdo
{
    public class RotateHelper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        Vector3 objEul;
        float angularSpeed;
        GameObject rotateObj;
        public bool IsDragging
        {
            get;
            private set;
        }
        void Awake()
        {
            angularSpeed = 0.1f;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            objEul = rotateObj.transform.localEulerAngles;
            IsDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            objEul.y -= eventData.delta.x * angularSpeed;
            rotateObj.transform.localEulerAngles = objEul;
            IsDragging = true;
        }

        public void Init(GameObject obj, float angularSpeed)
        {
            this.rotateObj = obj;
            this.angularSpeed = angularSpeed;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            IsDragging = false;
        }
    }
}