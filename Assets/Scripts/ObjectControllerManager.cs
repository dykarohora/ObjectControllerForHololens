using HoloToolkit.Unity;
using ObjectController.Adjuster;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ObjectController
{
    public class ObjectControllerManager : Singleton<ObjectControllerManager>
    {
        private ReactiveProperty<GameObject> _target = new ReactiveProperty<GameObject>();
        public IReadOnlyReactiveProperty<GameObject> Target => _target;

        [SerializeField]
        private ManipulationProgressProvider _manipulationProvider;
        
        public bool TrySetTarget(GameObject target)
        {
            // TODO:argetが条件を満たすかを確認する
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
                    var currentAdjuster = AdjusterManager.Instance.CurrentAdjuster;
                    currentAdjuster.AdjustTransform(_target.Value, velocity);
                });
        }
    }

}

