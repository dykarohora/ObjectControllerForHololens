using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;

namespace ObjectController.BoundingBox
{
    public class BoundingBoxGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _bbEdges;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float _scalePadding = 0.5f;

        private Vector3[] _corners = null;
        private Bounds _localTargetBounds = new Bounds();
        private List<Vector3> _boundsPoints = new List<Vector3>();

        private Vector3 _targetBoundsWorldCenter = Vector3.zero;
        private Vector3 _targetBoundsLocalScale = Vector3.zero;

        private const int _ignoreLayer = 2;

        private void Start()
        {
            // ターゲットが変更されたタイミングでBoundingBoxを生成する
            ObjectControllerManager.Instance.Target
                .Subscribe(target =>
                {
                    if (target != null)
                    {
                        GenerateBoundingBox(target);
                    }
                })
                .AddTo(gameObject);

            // BoundingBoxの再生成イベントをサブスクライブ
            var notifiers = GetComponents<IReceiveRegenerateBoundingBox>();
            foreach (var notifier in notifiers)
            {
                notifier.OnRegenerateBoundingBox
                    .Subscribe(GenerateBoundingBox)
                    .AddTo(gameObject);
            }
        }

        // BoundingBoxの生成メソッド
        private void GenerateBoundingBox(GameObject target)
        {
            _boundsPoints.Clear();

            // MeshFilteから計算
            var meshFilters = target.GetComponentsInChildren<MeshFilter>();
            for (var i = 0; i < meshFilters.Length; i++)
            {
                var meshFilterObj = meshFilters[i];
                if (meshFilterObj.gameObject.layer == _ignoreLayer)
                {
                    continue;
                }

                var meshBounds = meshFilterObj.sharedMesh.bounds;
                meshBounds.GetCornerPositions(meshFilterObj.transform, ref _corners);
                _boundsPoints.AddRange(_corners);
            }

            // Rendererから計算（SkinedMeshRenderer対応）
            var renderers = target.GetComponentsInChildren<Renderer>();
            for (var i = 0; i < renderers.Length; i++)
            {
                var rendererObj = renderers[i];
                if (rendererObj.gameObject.layer == _ignoreLayer)
                {
                    continue;
                }

                rendererObj.bounds.GetCornerPositionsFromRendererBounds(ref _corners);
                _boundsPoints.AddRange(_corners);
            }

            if (_boundsPoints.Count > 0)
            {
                // 各座標はワールド系なので、ターゲットのローカル系に変換する
                for (var i = 0; i < _boundsPoints.Count; i++)
                {
                    _boundsPoints[i] = target.transform.InverseTransformPoint(_boundsPoints[i]);
                }

                _localTargetBounds.center = _boundsPoints[0];
                _localTargetBounds.size = Vector3.zero;
                foreach (var point in _boundsPoints)
                {
                    _localTargetBounds.Encapsulate(point);
                }
            }

            _targetBoundsWorldCenter = target.transform.TransformPoint(_localTargetBounds.center);
            _targetBoundsLocalScale = _localTargetBounds.size;
            _targetBoundsLocalScale.Scale(target.transform.localScale);

            _bbEdges.transform.position = _targetBoundsWorldCenter;

            var largestDimension = Mathf.Max(Mathf.Max(_targetBoundsLocalScale.x, _targetBoundsLocalScale.y),
                _targetBoundsLocalScale.z);
            _targetBoundsLocalScale.x += (largestDimension * _scalePadding);
            _targetBoundsLocalScale.y += (largestDimension * _scalePadding);
            _targetBoundsLocalScale.z += (largestDimension * _scalePadding);

            _bbEdges.transform.localScale = _targetBoundsLocalScale;

            _bbEdges.transform.rotation = target.transform.rotation;
        }
    }

    // Boundsのメソッド拡張
    public static class BoundsExtentions
    {
        // Corners
        public const int LBF = 0;
        public const int LBB = 1;
        public const int LTF = 2;
        public const int LTB = 3;
        public const int RBF = 4;
        public const int RBB = 5;
        public const int RTF = 6;
        public const int RTB = 7;

        public static void GetCornerPositions(this Bounds bounds, Transform transform, ref Vector3[] positions)
        {
            // MeshFilterから取得したBoundsの各プロパティはローカル系
            var center = bounds.center;
            var extents = bounds.extents;
            var leftEdge = center.x - extents.x;
            var rightEdge = center.x + extents.x;
            var bottomEdge = center.y - extents.y;
            var topEdge = center.y + extents.y;
            var frontEdge = center.z - extents.z;
            var backEdge = center.z + extents.z;

            const int numPoints = 8;
            if (positions == null || positions.Length != numPoints)
            {
                positions = new Vector3[numPoints];
            }

            // ワールドに変換
            positions[BoundsExtentions.LBF] = transform.TransformPoint(leftEdge, bottomEdge, frontEdge);
            positions[BoundsExtentions.LBB] = transform.TransformPoint(leftEdge, bottomEdge, backEdge);
            positions[BoundsExtentions.LTF] = transform.TransformPoint(leftEdge, topEdge, frontEdge);
            positions[BoundsExtentions.LTB] = transform.TransformPoint(leftEdge, topEdge, backEdge);
            positions[BoundsExtentions.RBF] = transform.TransformPoint(rightEdge, bottomEdge, frontEdge);
            positions[BoundsExtentions.RBB] = transform.TransformPoint(rightEdge, bottomEdge, backEdge);
            positions[BoundsExtentions.RTF] = transform.TransformPoint(rightEdge, topEdge, frontEdge);
            positions[BoundsExtentions.RTB] = transform.TransformPoint(rightEdge, topEdge, backEdge);
        }

        public static void GetCornerPositionsFromRendererBounds(this Bounds bounds, ref Vector3[] positions)
        {
            var center = bounds.center;
            var extents = bounds.extents;
            var leftEdge = center.x - extents.x;
            var rightEdge = center.x + extents.x;
            var bottomEdge = center.y - extents.y;
            var topEdge = center.y + extents.y;
            var frontEdge = center.z - extents.z;
            var backEdge = center.z + extents.z;

            const int numPoints = 8;
            if (positions == null || positions.Length != numPoints)
            {
                positions = new Vector3[numPoints];
            }

            positions[BoundsExtentions.LBF] = new Vector3(leftEdge, bottomEdge, frontEdge);
            positions[BoundsExtentions.LBB] = new Vector3(leftEdge, bottomEdge, backEdge);
            positions[BoundsExtentions.LTF] = new Vector3(leftEdge, topEdge, frontEdge);
            positions[BoundsExtentions.LTB] = new Vector3(leftEdge, topEdge, backEdge);
            positions[BoundsExtentions.RBF] = new Vector3(rightEdge, bottomEdge, frontEdge);
            positions[BoundsExtentions.RBB] = new Vector3(rightEdge, bottomEdge, backEdge);
            positions[BoundsExtentions.RTF] = new Vector3(rightEdge, topEdge, frontEdge);
            positions[BoundsExtentions.RTB] = new Vector3(rightEdge, topEdge, backEdge);
        }
    }
}

