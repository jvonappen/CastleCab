using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class PopupDisplay
{
    static int m_popupCap = 25;
    static List<GameObject> m_popupList = new();

    static GameObject m_lastPopup;

    static GameObject CreatePopup()
    {
        GameObject go = new GameObject("PopupDisplay");
        m_popupList.Add(go);

        go.layer = LayerMask.NameToLayer("UI");
        //go.AddComponent<RectTransform>();
        //go.AddComponent<TextMeshProUGUI>();
        return go;
    }

    static GameObject FindValidPopup(int _index, int _totalRecursions = 0)
    {
        // If looped through all possible popups and none are valid, add a new one to increase the pool
        if (_totalRecursions >= m_popupList.Count && _totalRecursions < m_popupCap) return CreatePopup();

        // Find next possible popup
        int newIndex = _index + 1;
        if (newIndex >= m_popupList.Count) newIndex = 0;

        GameObject nextPopup = m_popupList[newIndex];
        if (nextPopup.activeSelf)
        {
            if (_totalRecursions >= m_popupList.Count) return nextPopup;

            if (m_popupList.Count < m_popupCap) return CreatePopup();
            else return FindValidPopup(newIndex, _totalRecursions + 1);
        }
        else return nextPopup;
    }

    public static GameObject Spawn(Vector3 _position, float _randomRangePos/*, Vector3 _rotation, float _randomRangeRot*/, string _text, float _fontSize, Color _colour, Vector3 _moveVelocity, Transform _parent, Transform _lookAt = null)
    {
        int index = m_popupList.IndexOf(m_lastPopup);
        GameObject go = FindValidPopup(index);

        go.SetActive(true);
        m_lastPopup = go;

        SimpleFadeTMP.Begin(go, 1.5f, FadeEndAction.Inactive);
        SimpleMove.Begin(go, _moveVelocity);

        //RectTransform rectTransform = go.GetComponent<RectTransform>();
        //rectTransform.SetParent(_parent, true);
        //rectTransform.position = GetPointInRange(Camera.main.WorldToScreenPoint(_position), _randomRangePos);
        //rectTransform.localEulerAngles = GetRotationInRange(_rotation, _randomRangeRot);

        //rectTransform.localScale = Vector3.one;

        // Not UI

        go.transform.SetParent(_parent, true);
        go.transform.position = GetPointInRange(_position, _randomRangePos, true);

        if (!_lookAt) go.transform.LookAt(Camera.main.transform);
        else go.transform.LookAt(_lookAt);

        go.transform.forward = -go.transform.forward;

        // End

        TextMeshPro display = go.GetComponent<TextMeshPro>();
        display.text = _text;
        display.fontSize = _fontSize;
        display.color = _colour;

        display.horizontalAlignment = HorizontalAlignmentOptions.Center;
        display.verticalAlignment = VerticalAlignmentOptions.Middle;

        return go;
    }

    static Vector3 GetPointInRange(Vector3 _position, float _randomRange, bool _enableZRange = false)
    {
        float x = _position.x;
        float y = _position.y;
        float z = _position.z;
        
        x = Random.Range(x - _randomRange, x + _randomRange);
        y = Random.Range(y, y + _randomRange);
        if (_enableZRange) z = Random.Range(z - _randomRange, z + _randomRange);

        return new Vector3(x, y, z);
    }

    static Vector3 GetRotationInRange(Vector3 _rotation, float _randomRange)
    {
        float zRot = _rotation.z;

        zRot = Random.Range(zRot - _randomRange, zRot + _randomRange);

        return new Vector3(_rotation.x, _rotation.y, zRot);
    }
}