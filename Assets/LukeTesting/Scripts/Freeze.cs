using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    [field: SerializeField] public float _duration { get; private set; } = 0.2f;
    [SerializeField] private float _pendingFreezeDuration = 0;
    [SerializeField] private bool _isFrozen = false;

    private void Update()
    {
        if (_pendingFreezeDuration > 0  && !_isFrozen)
        {
            StartCoroutine(DoFreeze());
        }
    }

    public void Freezer()
    {
        _pendingFreezeDuration = _duration;
    }

    IEnumerator DoFreeze()
    {
        _isFrozen = true;
        var originalTimescale = Time.timeScale;
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(_duration);

        Time.timeScale = originalTimescale;
        _pendingFreezeDuration = 0;
        _isFrozen = false;
    }
}
