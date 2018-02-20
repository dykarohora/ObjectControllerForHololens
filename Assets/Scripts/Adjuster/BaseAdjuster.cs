using ObjectController.BoundingBox;
using UniRx;
using UnityEngine;

namespace ObjectController.Adjuster
{
    public abstract class BaseAdjuster : MonoBehaviour, ITransformAdjustable, IReceiveRegenerateBoundingBox
    {
        [SerializeField]
        protected GameObject _bbEdges;

        // BoundingBoxの再生成を依頼するSubject
        protected readonly Subject<GameObject> _onRegenerateBoundingBox = new Subject<GameObject>();
        public IObservable<GameObject> OnRegenerateBoundingBox => _onRegenerateBoundingBox;

        protected IInputEventProvider _manipulationProvider;

        public abstract void AdjustTransform(GameObject target, Vector3 velocity);

        // 入力ソースの取得（要は依存性の解決）
        protected void GetInputEventProvider()
        {
            _manipulationProvider = GetComponent<IInputEventProvider>();
        }
    }
}

