using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace huynhdo
{
    public class Showing3DModelInUIHelper : MonoBehaviour, UnityEngine.EventSystems.IPointerClickHandler
    {
        /// <summary>
        /// is enable rorate helper
        /// </summary>
        [SerializeField] private bool IsEnableRotate;

        /// <summary>
        /// Angular Speed of rorate
        /// </summary>
        [Range(0.1f, 10)]
        [SerializeField] private float AngularSpeed;

        /// <summary>
        /// is enable callback when touch on raw-img
        /// </summary>
        [SerializeField] private bool IsEnanbleTouch = true;

        float textureSize = 1024;

        private float DefaultCameraFieldView = 12;
        public Camera RenderCamera { get; private set; }

        /// <summary>
        /// raw img
        /// </summary>
        private RawImage RawImage;

        /// <summary>
        /// render texture of raw image
        /// </summary>
        private RenderTexture renderTexture { get; set; }

        /// <summary>
        /// pivot of raw img
        /// </summary>
        public Vector2 Pivot { get; private set; }

        /// <summary>
        /// Scale of 3d model
        /// </summary>
        public float Scale
        {
            get
            {
                //return RawImage.transform.localScale.x;
                return RawImage.rectTransform.rect.size.y / textureSize;
            }
            set
            {
                //RawImage.rectTransform.sizeDelta = new Vector2(textureSize * value, textureSize * value);
                //RawImage.transform.localScale = new Vector3(value, value, value);
            }
        }

        /// <summary>
        /// name(id) of model to get from resource
        /// </summary>
        string modelName;

        /// <summary>
        /// 3d model
        /// </summary>
        public GameObject Model3D { get; private set; }

        /// <summary>
        /// rotate Helper
        /// </summary>
        RotateHelper rotateHelper;

        /// <summary>
        /// Click raw image Action
        /// </summary>
        public System.Action<Showing3DModelInUIHelper> OnClickAction;

        /// <summary>
        /// default animaion (trigger name)
        /// </summary>
        string defaultAnim;
        private void OnEnable()
        {
            if (Model3D != null)
            {
                Model3D.gameObject.SetActive(true);
                if (!string.IsNullOrEmpty(defaultAnim))
                    Model3D.GetComponent<Animator>().SetTrigger(defaultAnim);
                RenderCamera.gameObject.SetActive(true);
                Model3DVisionManager.Inst.RegisterModel3D(this);
            }
        }

        private void OnDisable()
        {
            if (Model3D != null)
            {
                Model3D.gameObject.SetActive(false);
                RenderCamera.gameObject.SetActive(false);
                Model3DVisionManager.Inst.UnRegisterModel3D(this);
            }
        }

        private void OnDestroy()
        {
            if (Model3D != null)
            {
                Destroy(Model3D.gameObject);
                Destroy(RenderCamera.gameObject);
            }
        }

        public void Init(GameObject loadedModel, float scale = 1, int antiAliasing = 0)
        {
            bool isFirstTime = Model3D == null;
            Model3D = loadedModel;
            Model3D.transform.SetParent(Model3DVisionManager.Inst.transform);
            RawImage = GetComponent<RawImage>();
            Pivot = RawImage.rectTransform.pivot;
            if (RawImage == null)
            {
                var prefabRI = Resources.Load<UnityEngine.UI.RawImage>("Local/3DModels/3DModelRawImage");
                RawImage = Instantiate(prefabRI);
            }
            if (antiAliasing > 8)
                antiAliasing = 8;
            else if (antiAliasing < 1)
                antiAliasing = 1;
            if (renderTexture == null)
            {
                var prefabRT = Resources.Load<RenderTexture>("Local/3DModels/3DModelRenderTexture");
                renderTexture = Instantiate(prefabRT);
                if (antiAliasing >= 1)
                    renderTexture.antiAliasing = antiAliasing;
                renderTexture.Create();
            }
            else if (antiAliasing >= 1)
                renderTexture.antiAliasing = antiAliasing;

            if (Model3DVisionManager.Inst.Model3DCamera != null)
            {
                RenderCamera = Model3DVisionManager.Inst.Model3DCamera;
            }
            else
            {
                if (RenderCamera == null)
                {
                    var prefabCamera = Resources.Load<Camera>("Local/3DModels/3DModelUICamera");
                    RenderCamera = Instantiate(prefabCamera, Model3DVisionManager.Inst.transform);
                }
            }

            RenderCamera.targetTexture = renderTexture;
            RawImage.texture = renderTexture;
            RenderCamera.fieldOfView = DefaultCameraFieldView / scale;
            this.Scale = scale;
            EnableRotate(IsEnableRotate, AngularSpeed);

            Model3DVisionManager.Inst.RegisterModel3D(this);
            //this.defaultAnim = defaultAnim;
            //if (isResetPose || isFirstTime)
            //{
            //    loadedModel.GetComponent<Animator>().SetTrigger(defaultAnim);
            //}


            // Model3D.transform.position = new Vector3(0, 0, -20f);
        }

        void Init(string modelName, int antiAliasing = 0, float scale = 1, bool isResetPose = false)
        {
            Init(modelName, IsEnableRotate, AngularSpeed, antiAliasing, scale, isResetPose);
        }

        GameObject Init(string modelName, bool enableRotate, float angularSpeed, int antiAliasing, float scale = 1, bool isResetPose = false, Camera defaultCamera = null)
        {
            if (Model3D != null && this.modelName.Equals(modelName))
            {
                if (isResetPose)
                {
                    Model3D.transform.localEulerAngles = Vector3.zero;
                }
                return Model3D;
            }
            this.modelName = modelName;
            if (Model3D != null)
            {
                Destroy(Model3D.gameObject);
                //Destroy(RenderCamera.gameObject);
                Model3D = null;
                //RenderCamera = null;
            }

            //TODO : Get 3d model
            //Model3D = Instantiate(ResourceManager.Inst.Get3DModel(modelName), Model3DVisionManager.Inst.transform);
            RawImage = GetComponent<RawImage>();
            Pivot = RawImage.rectTransform.pivot;
            if (RawImage == null)
            {
                var prefabRI = Resources.Load<UnityEngine.UI.RawImage>("Local/3DModels/3DModelRawImage");
                RawImage = Instantiate(prefabRI);
            }
            if (antiAliasing > 8)
                antiAliasing = 8;
            else if (antiAliasing < 1)
                antiAliasing = 1;
            if (renderTexture == null)
            {
                var prefabRT = Resources.Load<RenderTexture>("Local/3DModels/3DModelRenderTexture");
                renderTexture = Instantiate(prefabRT);
                if (antiAliasing >= 1)
                    renderTexture.antiAliasing = antiAliasing;
                renderTexture.Create();
            }
            else if (antiAliasing >= 1)
                renderTexture.antiAliasing = antiAliasing;

            if (Model3DVisionManager.Inst.Model3DCamera != null)
            {
                RenderCamera = Model3DVisionManager.Inst.Model3DCamera;
            }
            else
            {
                if (defaultCamera != null)
                    RenderCamera = defaultCamera;
                if (RenderCamera == null)
                {
                    var prefabCamera = Resources.Load<Camera>("Local/3DModels/3DModelUICamera");
                    RenderCamera = Instantiate(prefabCamera, Model3DVisionManager.Inst.transform);
                }
            }

            var mat = Resources.Load<Material>("Local/3DModels/3DModelRawImage");
            //mat.mainTexture = RenderTexture;
            // RawImage.material = mat;
            RenderCamera.targetTexture = renderTexture;
            RawImage.texture = renderTexture;
            //RenderCamera.fieldOfView = DefaultCameraFieldView / scale;
            this.Scale = scale;
            EnableRotate(enableRotate, angularSpeed);
            return Model3D;
        }

        public void ScaleTo(float scale)
        {
            this.Scale = scale;
        }

        public void EnableRotate(bool enableRotate, float angularSpeed = 0.1f)
        {
            if (Model3D == null)
                return;
            this.IsEnableRotate = enableRotate;
            this.AngularSpeed = angularSpeed;
            if (enableRotate)
            {
                if (rotateHelper == null)
                    rotateHelper = gameObject.AddComponent<RotateHelper>();
                rotateHelper.Init(Model3D, angularSpeed);
            }
            else
            {
                Destroy(rotateHelper);
                rotateHelper = null;
            }
        }

        private void Update()
        {
            if (renderTexture != null)
                renderTexture.Release();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (rotateHelper != null && rotateHelper.IsDragging)
                return;
            if (OnClickAction != null)
                OnClickAction.Invoke(this);
            if (Model3D == null || !IsEnanbleTouch)
                return;
            var animator = Model3D.GetComponent<Animator>();
            if (animator != null)
            {
                //animator.SetTrigger(AnimatorParameter.Deploy);
            }
        }
    }
}