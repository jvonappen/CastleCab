using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Knockback : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float m_force = 15, m_velY = 10;
    [SerializeField] GameObject m_particle;

    [SerializeField] private AudioGroupDetails audioGroup;

    Transform m_folder;

    private void Awake() => Init();
    protected virtual void Init()
    {
        rb = GetComponent<Rigidbody>();

        GameObject folderObj = GameObject.Find("----Particles");
        if (folderObj) m_folder = folderObj.transform;
    }

    public virtual void KnockBack(Vector3 _dir, Vector3 _origin)
    {
        if (rb.isKinematic) Debug.LogWarning("Cannot knock back kinematic object");

        rb.velocity = _dir * m_force;
        rb.velocity = new Vector3(rb.velocity.x, m_velY, rb.velocity.z);

        if (_origin != Vector3.zero)
        {
            ParticleSystem particle = Instantiate(m_particle, _origin, Quaternion.identity).GetComponent<ParticleSystem>();
            particle.transform.SetParent(m_folder);
            particle.Play();
            if (audioGroup != null) AudioManager.Instance.PlayGroupAudio(audioGroup.audioGroupName);
        }
    }
}
