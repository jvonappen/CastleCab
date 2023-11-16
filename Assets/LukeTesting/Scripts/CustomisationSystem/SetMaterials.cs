using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SetMaterials : MonoBehaviour
{
    [SerializeField] private MeshRenderer _cartMeshRenderer;
    [SerializeField] private List<MeshRenderer> _wheelMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer _horseSkinnedMeshRenderer;
    [SerializeField] private Transform _hatPos;
    [SerializeField] private MeshFilter _cartMeshFilter;

    public void SetCartMaterials(Material cartMesh)
    {
        //change cart material
        if (_cartMeshRenderer != null) _cartMeshRenderer.material = cartMesh;
    }

    public void SetHorseMaterials(Texture2D horseTexture)
    {
        //change horse colour
        if (_horseSkinnedMeshRenderer != null)
        {
            _horseSkinnedMeshRenderer.material.SetTexture("_1st_ShadeMap", horseTexture);
            _horseSkinnedMeshRenderer.material.SetTexture("_MainTex", horseTexture);
        }
    }

    public void SetWheelsMaterials(Material wheelMaterial)
    {
        //change wheel colour
        if (_wheelMeshRenderer != null)
        {
            foreach (MeshRenderer wheel in _wheelMeshRenderer)
            {
                wheel.material = wheelMaterial;
            }
        }
    }

    public void SetHatObject(GameObject hatObj)
    {
        //spawn hats
        if (_hatPos != null)
        {
            if (_hatPos.childCount == 0 && hatObj != null)
            {
                GameObject hat = Instantiate(hatObj, _hatPos);
                hat.transform.parent = _hatPos.transform;
            }
            else
            {
                if (_hatPos.childCount != 0) Destroy(_hatPos.GetChild(0).gameObject);
                if (hatObj != null)
                {
                    GameObject hat = Instantiate(hatObj, _hatPos);
                    hat.transform.parent = _hatPos.transform;
                }
            }
        }
    }

    public void SetCartMesh(Mesh mesh)
    {
        if (_cartMeshFilter != null) _cartMeshFilter.mesh = mesh;
    }
}
