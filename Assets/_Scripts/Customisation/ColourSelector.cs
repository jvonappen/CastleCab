using UnityEngine;

public struct DyeData
{
    public Color colour;
    public float metal;
    public float roughness;
}

public class ColourSelector : MonoBehaviour
{
    public virtual Material GetMat() { return null; }

    public virtual void ResetDye(string _colourSegment) { }
    public virtual void SetDye(string _colourSegment, SO_Dye _dye)
    {
        if (_dye) SetDye(_colourSegment, _dye.m_colour, _dye.m_metal, _dye.m_roughness);
        else ResetDye(_colourSegment);
    }
    public virtual void SetDye(string _colourSegment, DyeData _dye) => SetDye(_colourSegment, _dye.colour, _dye.metal, _dye.roughness);

    public void SetDye(string _colourSegment, Color _colour, float _metal, float _roughness)
    {
        Material mat = GetMat();
        if (mat)
        {
            mat.SetColor("_" + _colourSegment + "_Colour", _colour);
            mat.SetFloat("_" + _colourSegment + "_Metal", _metal);
            mat.SetFloat("_" + _colourSegment + "_Rough", _roughness);
        }
    }

    public DyeData GetDye(string _colourSegment)
    {
        Material mat = GetMat();
        if (mat)
        {
            DyeData data = new();

            data.colour = mat.GetColor("_" + _colourSegment + "_Colour");
            data.metal = mat.GetFloat("_" + _colourSegment + "_Metal");
            data.roughness = mat.GetFloat("_" + _colourSegment + "_Rough");

            return data;
        }

        return new();
    }

    public void CopyMatToSelector(ColourSelector _selector) => _selector.GetComponent<Renderer>().sharedMaterial = GetMat();
}
