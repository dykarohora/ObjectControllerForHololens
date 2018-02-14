using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace ObjectController.BoundingBox
{
    public class BoundingBoxTarget : MonoBehaviour, IInputClickHandler
    {
        // 一度計算したBoundingBoxは保管しておく仕組みが欲しい
        public void OnInputClicked(InputClickedEventData eventData)
        {
            ObjectControllerManager.Instance.TrySetTarget(gameObject);
        }
    }
}
