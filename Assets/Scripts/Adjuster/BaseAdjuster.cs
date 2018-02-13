using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController.Adjuster
{
    public abstract class BaseAdjuster : MonoBehaviour, ITransformAdjustable
    {
        [SerializeField]
        protected GameObject _bbEdges;

        [SerializeField]
        protected ManipulationProgressProvider _manipulationProvider;

        public abstract void AdjustTransform(GameObject target, Vector3 velocity);
    }
}

