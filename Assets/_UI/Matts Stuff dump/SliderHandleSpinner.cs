using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderHandleSpinner : MonoBehaviour
{
    Slider m_slider;
    [SerializeField] float m_spinAmount = 3;

    private void Awake() => m_slider = GetComponent<Slider>();

    public void OnSliderValueChanged()
    {
        RectTransform rt = m_slider.targetGraphic.rectTransform;
        rt.eulerAngles = new Vector3(rt.eulerAngles.x, rt.eulerAngles.y, -m_slider.value * 360 * m_spinAmount);
        //m_slider.targetGraphic.rectTransform.Rotate(-Vector3.forward * m_spinAmount);
    }


}
