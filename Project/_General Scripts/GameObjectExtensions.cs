using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GameObjectExtensions
{
    
    public static RectTransform FindTag(this GameObject child, string tag)
    {
        var parent = child.FindParentCanvas();
        return parent.GetComponentsInChildren<RectTransform>().ToList().Find(x => x.CompareTag(tag));
    }

    public static Canvas FindParentCanvas(this GameObject child)
    {
        return child.GetComponentsInParent<Canvas>()[0];
    }
}