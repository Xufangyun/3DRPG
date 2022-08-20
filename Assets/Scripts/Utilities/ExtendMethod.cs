using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendMethod
{
    public static bool IsFacingTarget(this Transform transform,Transform target)
    {
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, vectorToTarget);
        return dot>=0.5f;
    }
}
