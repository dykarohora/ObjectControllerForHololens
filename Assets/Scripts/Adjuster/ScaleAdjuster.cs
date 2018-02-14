using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UniRx;
using UnityEngine;

namespace ObjectController.Adjuster
{
    public class ScaleAdjuster : BaseAdjuster
    {
        [SerializeField]
        [Range(0.0f, 20.0f)]
        private float _scaleMultiple = 8.0f;

        private bool _isRight;

        private void Start()
        {
            _manipulationProvider.IsManipulating
                .Where(isManipulationg => isManipulationg)
                .Subscribe(isManipulating =>
                {
                    // 左右の判定
                    var bbEdgesScreenPoint = Camera.main.WorldToScreenPoint(_bbEdges.transform.position);
                    var gazeScreenPoint = Camera.main.WorldToScreenPoint(GazeManager.Instance.HitPosition);
                    _isRight = gazeScreenPoint.x - bbEdgesScreenPoint.x < 0;
                })
                .AddTo(gameObject);
        }

        public override void AdjustTransform(GameObject target, Vector3 velocity)
        {
            var dragAmount = velocity.x;
            if (_isRight)
            {
                dragAmount = -dragAmount;
            }

            target.transform.localScale += Vector3.one * (dragAmount * _scaleMultiple);
            _bbEdges.transform.localScale += Vector3.one * (dragAmount * _scaleMultiple);
            // boundsの再生成？？
            _onRegenerateBoundingBox.OnNext(target);
        }
    }
}