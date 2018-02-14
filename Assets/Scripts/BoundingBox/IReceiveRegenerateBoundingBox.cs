using UniRx;
using UnityEngine;

namespace ObjectController.BoundingBox
{
    public interface IReceiveRegenerateBoundingBox
    {
        IObservable<GameObject> OnRegenerateBoundingBox { get; }
    }
}
