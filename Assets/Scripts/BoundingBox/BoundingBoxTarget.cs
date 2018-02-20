using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace ObjectController.BoundingBox
{
    public class BoundingBoxTarget : MonoBehaviour, IInputClickHandler
    {
        // TODO:一度計算したBoundingBoxは保管しておく仕組みが欲しい
        // BoundingBoxの再生成時に形状が変わるのを防ぐため

        public void OnInputClicked(InputClickedEventData eventData)
        {
            ObjectControllerManager.Instance.TrySetTarget(gameObject);
        }
    }
}
