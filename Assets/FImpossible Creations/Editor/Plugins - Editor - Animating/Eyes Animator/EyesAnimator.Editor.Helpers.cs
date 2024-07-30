using FIMSpace.FEyes;
using System.Collections.Generic;
using UnityEngine;

public partial class FEyesAnimator_Editor : UnityEditor.Editor
{
    public static Texture2D _TexEyesAnimIcon { get { if (__texEyesAnimIcon != null) return __texEyesAnimIcon; __texEyesAnimIcon = Resources.Load<Texture2D>("Eyes Animator/EyesAnimator_IconSmall"); return __texEyesAnimIcon; } }
    private static Texture2D __texEyesAnimIcon = null;
    public static Texture2D _TexEyeIcon { get { if (__texEyeIcon != null) return __texEyeIcon; __texEyeIcon = Resources.Load<Texture2D>("Eyes Animator/EyeIcon"); return __texEyeIcon; } }
    private static Texture2D __texEyeIcon = null;
    public static Texture2D _TexBlinkIcon { get { if (__texBlinkIcon != null) return __texBlinkIcon; __texBlinkIcon = Resources.Load<Texture2D>("Eyes Animator/BlinkingIcon"); return __texBlinkIcon; } }
    private static Texture2D __texBlinkIcon = null;
    public static Texture2D _TexEyeRandomIcon { get { if (__texEyeRandomIcon != null) return __texEyeRandomIcon; __texEyeRandomIcon = Resources.Load<Texture2D>("Eyes Animator/EyeRandom"); return __texEyeRandomIcon; } }
    private static Texture2D __texEyeRandomIcon = null;

    public static Texture2D _TexEyeLidUpIcon { get { if (__texEyeLidUpIcon != null) return __texEyeLidUpIcon; __texEyeLidUpIcon = Resources.Load<Texture2D>("Eyes Animator/EyeLidUp"); return __texEyeLidUpIcon; } }
    private static Texture2D __texEyeLidUpIcon = null;
    public static Texture2D _TexEyeLidDownIcon { get { if (__texEyeLidDownIcon != null) return __texEyeLidDownIcon; __texEyeLidDownIcon = Resources.Load<Texture2D>("Eyes Animator/EyeLidDown"); return __texEyeLidDownIcon; } }
    private static Texture2D __texEyeLidDownIcon = null;

    public static Texture2D _TexEyeFollowIcon { get { if (__texEyeFollowIcon != null) return __texEyeFollowIcon; __texEyeFollowIcon = Resources.Load<Texture2D>("Eyes Animator/EyeFollow"); return __texEyeFollowIcon; } }
    private static Texture2D __texEyeFollowIcon = null;

    private FEyesAnimator Get { get { if (_get == null) _get = target as FEyesAnimator; return _get; } }
    private FEyesAnimator _get;

    private static UnityEngine.Object _manualFile;
    private Color c;

    public List<SkinnedMeshRenderer> skins;
    SkinnedMeshRenderer largestSkin;
    Animator animator;
    Animation animation;

    bool drawRandomSettings = true;
    bool drawLagSettings = true;


    /// <summary>
    /// Trying to deep find skinned mesh renderer
    /// </summary>
    private void FindComponents()
    {
        if (skins == null) skins = new List<SkinnedMeshRenderer>();

        foreach (var t in Get.transform.GetComponentsInChildren<Transform>())
        {
            SkinnedMeshRenderer s = t.GetComponent<SkinnedMeshRenderer>(); if (s) skins.Add(s);
            if (!animator) animator = t.GetComponent<Animator>();
            if (!animator) if (!animation) animation = t.GetComponent<Animation>();
        }

        if ((skins != null && largestSkin != null) && (animator != null || animation != null)) return;

        if (Get.transform != Get.transform)
        {
            foreach (var t in Get.transform.GetComponentsInChildren<Transform>())
            {
                SkinnedMeshRenderer s = t.GetComponent<SkinnedMeshRenderer>(); if (!skins.Contains(s)) if (s) skins.Add(s);
                if (!animator) animator = t.GetComponent<Animator>();
                if (!animator) if (!animation) animation = t.GetComponent<Animation>();
            }
        }

        // Searching in parent
        if (skins.Count == 0)
        {
            Transform lastParent = Get.transform;

            while (lastParent != null)
            {
                if (lastParent.parent == null) break;
                lastParent = lastParent.parent;
            }

            foreach (var t in lastParent.GetComponentsInChildren<Transform>())
            {
                SkinnedMeshRenderer s = t.GetComponent<SkinnedMeshRenderer>(); if (!skins.Contains(s)) if (s) skins.Add(s);
                if (!animator) animator = t.GetComponent<Animator>();
                if (!animator) if (!animation) animation = t.GetComponent<Animation>();
            }
        }

        if (skins.Count > 1)
        {
            largestSkin = skins[0];
            for (int i = 1; i < skins.Count; i++)
                if (skins[i].bones.Length > largestSkin.bones.Length)
                    largestSkin = skins[i];
        }
        else
            if (skins.Count > 0) largestSkin = skins[0];

    }


    /// <summary>
    /// Searching through component's owner to find head or neck bone
    /// </summary>
    private void FindHeadBone(FEyesAnimator eyesAnimator)
    {
        // First let's check if it's humanoid character, then we can get head bone transform from it
        Animator animator = eyesAnimator.GetComponent<Animator>();
        Transform animatorHeadBone = null;
        if (animator)
        {
            if (animator.isHuman)
            {
                animatorHeadBone = animator.GetBoneTransform(HumanBodyBones.Head);
            }
        }


        Transform headBone = null;
        Transform probablyWrongTransform = null;

        foreach (Transform t in eyesAnimator.GetComponentsInChildren<Transform>())
        {
            if (t == eyesAnimator.transform) continue;

            if (t.name.ToLower().Contains("head"))
            {
                if (t.GetComponent<SkinnedMeshRenderer>())
                {
                    if (t.parent == eyesAnimator.transform) continue; // If it's just mesh object from first depths
                    probablyWrongTransform = t;
                    continue;
                }

                headBone = t;
                break;
            }
        }

        if (!headBone)
            foreach (Transform t in eyesAnimator.GetComponentsInChildren<Transform>())
            {
                if (t.name.ToLower().Contains("neck"))
                {
                    headBone = t;
                    break;
                }
            }

        if (headBone == null && animatorHeadBone != null)
            headBone = animatorHeadBone;
        else
        if (headBone != null && animatorHeadBone != null)
        {
            if (animatorHeadBone.name.ToLower().Contains("head")) headBone = animatorHeadBone;
            else
                if (!headBone.name.ToLower().Contains("head")) headBone = animatorHeadBone;
        }

        if (headBone)
        {
            eyesAnimator.HeadReference = headBone;
            FindingEyes(eyesAnimator);
        }
        else
        {
            if (probablyWrongTransform) eyesAnimator.HeadReference = probablyWrongTransform;
            Debug.LogWarning("Found " + probablyWrongTransform + " but it's probably wrong transform");
        }
    }


    protected virtual void FindingEyes(FEyesAnimator eyesAnimator)
    {
        if (eyesAnimator.HeadReference == null) return;

        // Trying to find eye bones inside head bone
        Transform[] children = eyesAnimator.HeadReference.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            string lowerName = children[i].name.ToLower();
            if (lowerName.Contains("eye"))
            {
                if (lowerName.Contains("brow") || lowerName.Contains("lid") || lowerName.Contains("las")) continue;

                if (lowerName.Contains("left")) { if (!eyesAnimator.Eyes.Contains(children[i])) eyesAnimator.Eyes.Add(children[i]); continue; }
                else
                    if (lowerName.Contains("l")) { if (!eyesAnimator.Eyes.Contains(children[i])) eyesAnimator.Eyes.Add(children[i]); continue; }

                if (lowerName.Contains("right")) { if (!eyesAnimator.Eyes.Contains(children[i])) eyesAnimator.Eyes.Add(children[i]); continue; }
                else
                    if (lowerName.Contains("r")) { if (!eyesAnimator.Eyes.Contains(children[i])) eyesAnimator.Eyes.Add(children[i]); continue; }
            }
        }


        if (eyesAnimator.HeadReference)
        {
            if (eyesAnimator.HeadReference == null) return;

            if (Get.EyeLids == null) Get.EyeLids = new List<Transform>();
            if (Get.DownEyelids == null) Get.DownEyelids = new List<Transform>();
            if (Get.UpEyelids == null) Get.UpEyelids = new List<Transform>();

            // Trying to find eyelid bones inside eyes bones
            for (int e = 0; e < eyesAnimator.Eyes.Count; e++)
            {
                children = eyesAnimator.Eyes[e].GetComponentsInChildren<Transform>();

                for (int i = 0; i < children.Length; i++)
                {
                    string lowerName = children[i].name.ToLower();
                    if (lowerName.Contains("lid"))
                    {
                        if (lowerName.Contains("low") || lowerName.Contains("down") || lowerName.Contains("bot"))
                        {
                            if (!Get.DownEyelids.Contains(children[i])) Get.DownEyelids.Add(children[i]);
                        }
                        else
                        if (lowerName.Contains("up") || lowerName.Contains("top"))
                        {
                            if (!Get.UpEyelids.Contains(children[i])) Get.UpEyelids.Add(children[i]);
                        }
                        else
                        {
                            if (!Get.EyeLids.Contains(children[i])) Get.EyeLids.Add(children[i]);
                        }
                    }
                }

                Get.UpdateLists();
            }
        }
    }


}
