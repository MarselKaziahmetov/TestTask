using System;
using UnityEngine;

/// <summary>
/// Представляет сущность области
/// </summary>
[Serializable]
public class AreaObjectInfo
{
    public GameObject prefab;
    public bool isLimited;
    public int numberOfCopies;
}
