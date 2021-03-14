using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAssets : MonoBehaviour {

    public Offsets offsets;

    public AnimatorOverrideController[] maleHair;
    public AnimatorOverrideController[] maleHead;
    public AnimatorOverrideController[] maleShirt;
    public AnimatorOverrideController[] maleLegs;
    public AnimatorOverrideController[] maleExtra;
    public AnimatorOverrideController[] femaleHair;
    public AnimatorOverrideController[] femaleHead;
    public AnimatorOverrideController[] femaleShirt;
    public AnimatorOverrideController[] femaleLegs;
    public AnimatorOverrideController[] femaleExtra;
}


[System.Serializable]
public struct Offsets
{
    public Vector2 torsoOffset;
    public Vector2 legsOffset;
}
