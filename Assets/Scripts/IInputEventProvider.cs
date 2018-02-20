using UniRx;
using UnityEngine;

namespace ObjectController
{
    public interface IInputEventProvider
    {
        IReadOnlyReactiveProperty<bool> IsManipulating { get; }
        Vector3 SmoothVelocity { get; }
    }
}
