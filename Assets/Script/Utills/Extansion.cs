using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Ȯ�� �޼���
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
