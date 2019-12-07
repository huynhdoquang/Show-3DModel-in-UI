using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace huynhdo
{
    public class Model3DVisionManager : MonoBehaviour
    {
        public static Model3DVisionManager Inst;
        public Camera Model3DCamera;

        private List<Showing3DModelInUIHelper> _listModel3D;
        private List<Showing3DModelInUIHelper> ListModel3D
        {
            get
            {
                if (_listModel3D == null)
                    _listModel3D = new List<Showing3DModelInUIHelper>();
                return _listModel3D;
            }
        }

        int model3DCreateIndex;

        private void Awake()
        {
            Inst = this;
        }

        /// <summary>
        /// Register model 3D
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isNeedChangeCameraPos"></param>
        public void RegisterModel3D(Showing3DModelInUIHelper model, bool isNeedChangeCameraPos = true)
        {
            //TODO: reset local of 3d model, must change by model size
            model.Model3D.transform.localPosition = new Vector3(model3DCreateIndex, 0, -20);
            model3DCreateIndex++;

            if (ListModel3D.Contains(model) || model == null)
            {
                UpdateCameraPosition(model);
                return;
            }
            if (isNeedChangeCameraPos)
            {
                Vector2 modelPos = new Vector2(0, model.RenderCamera.orthographicSize);
                modelPos.y = 0.12f * (1 - model.Pivot.y);
                modelPos.x = model.RenderCamera.orthographicSize * (model.Pivot.x - 0.5f);
                model.RenderCamera.transform.localPosition = new Vector3(modelPos.x + model.Model3D.transform.localPosition.x, modelPos.y, model.RenderCamera.transform.localPosition.z);
            }
            ListModel3D.Add(model);
        }

        void UpdateCameraPosition(Showing3DModelInUIHelper model)
        {
            Vector2 modelPos = new Vector2(0, model.RenderCamera.orthographicSize);
            modelPos.y = 0.12f * (1 - model.Pivot.y);
            modelPos.x = model.RenderCamera.orthographicSize * (model.Pivot.x - 0.5f);
            model.RenderCamera.transform.localPosition = new Vector3(modelPos.x + model.Model3D.transform.localPosition.x, modelPos.y, model.RenderCamera.transform.localPosition.z);
        }

        public void UnRegisterModel3D(Showing3DModelInUIHelper model)
        {
            if (ListModel3D.Contains(model))
                ListModel3D.Remove(model);
        }
    }
}
