using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class IgnoreFog : MonoBehaviour
{
    Camera m_cam;
    private void Awake() => m_cam = GetComponent<Camera>();

    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += OnCameraBeginRendering;
        RenderPipelineManager.endCameraRendering += OnCameraEndRendering;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnCameraBeginRendering;
        RenderPipelineManager.endCameraRendering -= OnCameraEndRendering;
    }

    void OnCameraBeginRendering(ScriptableRenderContext _context, Camera _cam)
    {
        if (_cam == m_cam) RenderSettings.fog = false;
    }

    void OnCameraEndRendering(ScriptableRenderContext _context, Camera _cam)
    {
        if (_cam == m_cam) RenderSettings.fog = true;
    }
}
