using System;
using HoloToolkit.Unity;
using ObjectController.Menu;
using UniRx;
using UnityEngine;

namespace ObjectController.Adjuster
{
    public class AdjusterManager : Singleton<AdjusterManager>
    {
        [SerializeField]
        private PositionAdjuster _positionAdjuster;

        [SerializeField]
        private RotateAdjuster _rotateAdjuster;

        [SerializeField]
        private ScaleAdjuster _scaleAdjuster;

        public ITransformAdjustable CurrentAdjuster { get; private set; } = null;

        private readonly ReactiveProperty<ControllerButtonType> _currentState = new ReactiveProperty<ControllerButtonType>(ControllerButtonType.Done);
        public IReadOnlyReactiveProperty<ControllerButtonType> CurrentState => _currentState;

        private void Start()
        {
            CurrentAdjuster = _positionAdjuster;
        }

        // UIのPresentetor層からコールされるモデルのロジック
        public void SetCurrentAdjuster(ControllerButtonType type)
        {
            switch (type)
            {
                case ControllerButtonType.Move:
                    CurrentAdjuster = _positionAdjuster;
                    break;
                case ControllerButtonType.Rotate:
                    CurrentAdjuster = _rotateAdjuster;
                    break;
                case ControllerButtonType.Scale:
                    CurrentAdjuster = _scaleAdjuster;
                    break;
                case ControllerButtonType.Done:
                    CurrentAdjuster = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            _currentState.Value = type;
        }
    }
}

