using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum TargetType
{
    MainCamera,
    Other
}

public class AlwaysFaceObject : MonoBehaviour
{
    [SerializeField] TargetType m_targetType;
    [SerializeField] [ConditionalEnumHide("m_targetType", 1)] Transform m_target;
    [SerializeField] bool m_useDirection, m_isInverse, m_onUpdate, m_updatePerCam;

    //private void OnWillRenderObject()
    //{
    //    if (m_updatePerCam)
    //    {
    //        if (Camera.current && Camera.current.transform)
    //        {
    //            m_target = Camera.current.transform;
    //            UpdateRotation();
    //        }
    //    }
    //}

    private void Awake()
    {
        if (m_targetType == TargetType.MainCamera) m_target = Camera.main.transform;

        if (m_target) UpdateRotation();
    }
    private void Update() { if (m_onUpdate && !m_updatePerCam) UpdateRotation(); }

    void UpdateRotation()
    {
        if (!m_useDirection) transform.LookAt(m_target);
        else transform.forward = m_target.forward;

        if (m_isInverse) transform.forward = -transform.forward;
    }

    private void OnEnable()
    {
        if (m_updatePerCam) RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }

    private void OnDisable()
    {
        if (m_updatePerCam) RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }

    void OnBeginCameraRendering(ScriptableRenderContext _context, Camera _camera)
    {
        if (_camera.name == "Camera")
        {
            m_target = _camera.transform;
            UpdateRotation();
        }
    }
}
