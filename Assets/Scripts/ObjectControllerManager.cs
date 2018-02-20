﻿using HoloToolkit.Unity;
using ObjectController.Adjuster;
using ObjectController.Menu;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ObjectController
{
    public class ObjectControllerManager : Singleton<ObjectControllerManager>
    {
        private readonly ReactiveProperty<GameObject> _target = new ReactiveProperty<GameObject>();
        public IReadOnlyReactiveProperty<GameObject> Target => _target;

        [SerializeField]
        private ManipulationProgressProvider _manipulationProvider;

        [SerializeField]
        private GameObject _bbEdges;

        private ITransformAdjustable _currentAdjuster;

        public bool TrySetTarget(GameObject target)
        {
            // TODO:targetが条件を満たすかを確認する
            _target.Value = target;
            return true;
        }

        private void Start()
        {
            this.UpdateAsObservable()
                .Where(_ => _manipulationProvider.IsManipulating.Value)
                .Subscribe(_ =>
                {
                    var velocity = _manipulationProvider.SmoothVelocity;
                    _currentAdjuster?.AdjustTransform(_target.Value, velocity);
                });

            AdjusterManager.Instance.CurrentState
                .Subscribe(_ => _currentAdjuster = AdjusterManager.Instance.CurrentAdjuster)
                .AddTo(gameObject);

            AdjusterManager.Instance.CurrentState
                .Where(state => state == ControllerButtonType.Done)
                .Subscribe(_ => _bbEdges.transform.localScale = Vector3.zero)
                .AddTo(gameObject);
        }
    }

}

