using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallImpact : MonoBehaviour
{
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private ParticleSystem _wallImpact;
    [SerializeField] private ParticleSystem _wham;
    [SerializeField] private float _hitForce = 100;
    [SerializeField] private Rigidbody _playerRB;

    private void Awake()
    {
        _soundManager = FindObjectOfType<SoundManager>();
        _playerRB = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 15)
        {
            Vector3 direction = collision.transform.position.normalized - this.transform.position.normalized;
            direction = -direction.normalized;
            Debug.Log("Wall Hit");
            //_soundManager.Play("WallHit");
            ParticleSystem impact = Instantiate(_wallImpact, this.transform);
            ParticleSystem wham = Instantiate(_wham, this.transform);
            _playerRB.AddForce(direction * _hitForce, ForceMode.Impulse);
        }
    }
}
