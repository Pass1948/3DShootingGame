using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 확장 메서드
public static class Extansion
{
    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeInHierarchy;
    }
    public static bool IsValid(this Component co)
    {
        return co != null && co.gameObject.activeInHierarchy;
    }
}
