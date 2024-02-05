using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourControl : MonoBehaviour
{
    [SerializeField] private CustomisationTab _cartModel;
    private CustomisationTab _cartColor;

    private void Awake()
    {
        _cartColor = GetComponent<CustomisationTab>();
    }

    private void Update()
    {
        DisableColorSwitch();
    }

    private void DisableColorSwitch()
    {
        if (_cartModel.GetIndex() != 0 && _cartColor.canClick != false)
        {
            _cartColor.canClick = false;
        }
        else if (_cartModel.GetIndex() == 0 && _cartColor.canClick != true)
        {
            _cartColor.SetColour();
            _cartColor.canClick = true;
        }
        
    }
}
