using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallImpact : MonoBehaviour
{
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private ParticleSystem _wallImpact;
    [SerializeField] private ParticleSystem _wham;
    [SerializeField] private float _hitForce = 100;
    [SerializeField] private Rigidbody _playerRB;
    [SerializeField] private Transform _whamPos;
    [SerializeField] private List<GameObject> _whams;

    private void Awake()
    {
        _soundManager = FindObjectOfType<SoundManager>();
        _playerRB = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 15)
        {
            _soundManager.Play("WallImpact");
            ParticleSystem impact = Instantiate(_wallImpact, this.transform); //destroy by itself
            ParticleSystem wham = Instantiate(_wham, _whamPos);
            _whams.Add(wham.gameObject); //add to list to be destroyed
            _playerRB.AddForce(collision.contacts[0].normal * _hitForce, ForceMode.Impulse); //knockback player
        }
    }

    private void Update()
    {
        if (_whams.Count > 0) //destroy instantiated whams
        {
            foreach (var wham in _whams.ToList())
            {
                Destroy(wham, 1);
                if (wham == null) _whams.Remove(wham);
            }
        }
    }
}
