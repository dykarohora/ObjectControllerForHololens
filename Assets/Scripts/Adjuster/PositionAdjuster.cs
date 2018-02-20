using UnityEngine;

namespace ObjectController.Adjuster
{
    public class PositionAdjuster : BaseAdjuster
    {
        [SerializeField]
        [Range(0.0f, 20.0f)] 
        private float _moveMultiple = 8.0f;

        private void Start()
        {
            GetInputEventProvider();
        }

        // 移動量の計算と反映
        // Managerからコールされる
        public override void AdjustTransform(GameObject target, Vector3 velocity)
        {
            target.transform.position += velocity * _moveMultiple;
            _bbEdges.transform.position += velocity * _moveMultiple;
        }
    }
}
