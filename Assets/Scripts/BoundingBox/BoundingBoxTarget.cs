using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace ObjectController.BoundingBox
{
    public class BoundingBoxTarget : MonoBehaviour, IInputClickHandler
    {
        public void OnInputClicked(InputClickedEventData eventData)
        {
            ObjectControllerManager.Instance.TrySetTarget(gameObject);
        }
    }
}
