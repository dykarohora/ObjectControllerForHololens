using HoloToolkit.Unity.InputModule;
using UniRx;
using UnityEngine;

namespace ObjectController
{
    public class ManipulationProgressProvider : MonoBehaviour, IManipulationHandler, IInputEventProvider
    {
        private readonly BoolReactiveProperty _isManipulating = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsManipulating => _isManipulating;

        private Vector3 _lastNavigatePos = Vector3.zero;
        private Vector3 _navigateVelocity = Vector3.zero;
        public Vector3 SmoothVelocity { get; private set; } = Vector3.zero;

        public void OnManipulationStarted(ManipulationEventData eventData)
        {
            _isManipulating.Value = true;   // イベント発火
            InputManager.Instance.PushModalInputHandler(gameObject);
        }

        public void OnManipulationUpdated(ManipulationEventData eventData)
        {
            if (_isManipulating.Value == false) return;

            var eventPos = eventData.CumulativeDelta;

            _navigateVelocity = eventPos - _lastNavigatePos;
            _lastNavigatePos = eventPos;
            SmoothVelocity = Vector3.Lerp(SmoothVelocity, _navigateVelocity, 0.5f);
        }

        public void OnManipulationCompleted(ManipulationEventData eventData)
        {
            _isManipulating.Value = false;  // イベント発火
            ResetVectors();
            InputManager.Instance.PopModalInputHandler();
        }

        public void OnManipulationCanceled(ManipulationEventData eventData)
        {
            _isManipulating.Value = false;  // イベント発火
            ResetVectors();
            InputManager.Instance.PopModalInputHandler();
        }

        private void ResetVectors()
        {
            _lastNavigatePos = Vector3.zero;
            _navigateVelocity = Vector3.zero;
            SmoothVelocity = Vector3.zero;
        }
    }
}

