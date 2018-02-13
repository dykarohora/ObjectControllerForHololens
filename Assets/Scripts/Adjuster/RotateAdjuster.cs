using UniRx;
using UnityEngine;

namespace ObjectController.Adjuster
{
    public class RotateAdjuster : BaseAdjuster
    {
        private Vector3 _orthogonalVect = Vector3.zero;

        private void Start()
        {
            _manipulationProvider.IsManipulating
                .Where(isManipulating => isManipulating)
                .Subscribe(isManipulating =>
                {
                    var axisVect = Vector3.ProjectOnPlane(transform.up, Camera.main.transform.forward);
                    _orthogonalVect = Vector3.Cross(Camera.main.transform.forward, axisVect);
                })
                .AddTo(gameObject);
        }

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

