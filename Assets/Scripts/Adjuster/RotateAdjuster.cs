using UniRx;
using UnityEngine;

namespace ObjectController.Adjuster
{
    public class RotateAdjuster : BaseAdjuster
    {
        private Vector3 _orthogonalVect = Vector3.zero;

        private void Start()
        {
            GetInputEventProvider();

            // マニピュレーション開始時に回転軸を決めておく
            _manipulationProvider.IsManipulating
                .Where(isManipulating => isManipulating)
                .Subscribe(isManipulating =>
                {
                    var axisVect = Vector3.ProjectOnPlane(transform.up, Camera.main.transform.forward);
                    _orthogonalVect = Vector3.Cross(Camera.main.transform.forward, axisVect);
                })
                .AddTo(gameObject);
        }

        // 回転量の計算と反映
        // Managerからコールされる
        public override void AdjustTransform(GameObject target, Vector3 velocity)
        {
            var projectionVect = Vector3.Project(velocity, _orthogonalVect);
            var rotateAngle = 
                velocity.magnitude
                * Vector3.Dot(projectionVect.normalized, _orthogonalVect.normalized)
                * 360;

            target.transform.RotateAround(target.transform.position, _bbEdges.transform.up, rotateAngle);
            _bbEdges.transform.RotateAround(_bbEdges.transform.position, _bbEdges.transform.up, rotateAngle);
        }
    }
}

