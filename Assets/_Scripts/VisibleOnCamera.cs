using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Renderer))]
public class VisibleOnCamera : MonoBehaviour
{
    [SerializeField] Renderer m_renderer;
    public List<Camera> camerasDisplaying = new();

    private void OnEnable() => RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    private void OnDisable() => RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;

    void OnBeginCameraRendering(ScriptableRenderContext _context, Camera _camera)
    {
        if (camerasDisplaying.Contains(_camera)) m_renderer.enabled = true;
        else m_renderer.enabled = false;
    }

    public void AddInteractor(Interact _interactor) => camerasDisplaying.Add(_interactor.cam);
    public void RemoveInteractor(Interact _interactor) => camerasDisplaying.Remove(_interactor.cam);
}
