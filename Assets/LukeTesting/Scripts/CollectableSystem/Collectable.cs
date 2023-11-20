using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [field: SerializeField] public char _collectibleLetter { get; private set; }
    [field: SerializeField] public int _letterPos { get; private set; }
    private DisplayCollectables _collectables;

    private void Awake()
    {
        _collectables = FindObjectOfType<DisplayCollectables>();
        this.gameObject.name = _collectibleLetter.ToString();
    }

    private void Update()
    {
        transform.Rotate(0, 0.75f, 0);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit Letter");
            _collectables.SetCollectableActive(this);
            AudioManager.Instance.PlaySFX("Collectable");
            Destroy(gameObject);
        }
    }
}
