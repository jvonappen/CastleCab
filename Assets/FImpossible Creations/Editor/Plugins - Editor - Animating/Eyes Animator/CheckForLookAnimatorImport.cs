using FIMSpace.FEditor;

[UnityEditor.InitializeOnLoad]
static class FOptimizers_DefineFimpossiblePackages
{
    static string eyes = "EYES_LOOKANIMATOR_IMPORTED";
    static FOptimizers_DefineFimpossiblePackages()
    {
        if (FDefinesCompilation.GetTypesInNamespace("FIMSpace.FLook", "").Count > 1) FDefinesCompilation.SetDefine(eyes);
        else FDefinesCompilation.RemoveDefine(eyes);
    }
}