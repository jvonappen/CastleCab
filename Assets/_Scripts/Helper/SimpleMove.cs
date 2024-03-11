using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [SerializeField] Vector3 m_velocity;
    private void Update() { transform.position += m_velocity * Time.deltaTime; }
    public static void Begin(GameObject _go, Vector3 _velocity)
    {
        if (!_go.TryGetComponent(out SimpleMove simpleMove)) _go.AddComponent<SimpleMove>().m_velocity = _velocity;
    }
}