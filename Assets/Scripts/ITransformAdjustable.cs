using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    public interface ITransformAdjustable
    {
        void AdjustTransform(GameObject target, Vector3 velocity);
    }
}
