using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Fractal4WithJob : MonoBehaviour
{
    struct FractalPart
    {
        public Vector3 Direction;
        public Quaternion Rotation;
        public Transform Transform;
    }

    private struct UpdateFractalLevelJob : IJobFor
    {
        public float SpinAngleDelta;
        public float Scale;

        [ReadOnly]
        public NativeArray<FractalPart> Parents;
        public NativeArray<FractalPart> Parts;

        [WriteOnly]
        public NativeArray<Matrix4x4> Matrices;

        public void Execute(int index)
        {
            var parent = Parents[index / _childCount];
            var part = Parts[index];
        }
    }

    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    private FractalPart[][] _parts;

    [SerializeField, Range(1, 8)] private int _depth = 4;
    [SerializeField, Range(0, 360)] private int _speedRotation = 80;
    private const float _positionOffset = 1.5f;
    private const float _scaleBias = .5f;
    private const int _childCount = 5;

    private static readonly Vector3[] _directions =
    {
       Vector3.up,
       Vector3.left,
       Vector3.right,
       Vector3.forward,
       Vector3.back
    };

    private static readonly Quaternion[] _rotations =
    {
       Quaternion.identity,
       Quaternion.Euler(.0f, .0f, 90.0f),
       Quaternion.Euler(.0f, .0f, -90.0f),
       Quaternion.Euler(90.0f, .0f, .0f),
       Quaternion.Euler(-90.0f, .0f, .0f)
    };


    private void OnEnable()
    {
        _parts = new FractalPart[_depth][];

        for (int i = 0, length = 1; i < _parts.Length; i++, length *= _childCount)
        {
            _parts[i] = new FractalPart[length];
        }

        var scale = 1f;

        _parts[0][0] = CreatePart(0, 0, scale);

        for (var li = 1; li < _parts.Length; li++)
        {
            var levelParts = _parts[li];
            for (var fpi = 0; fpi < levelParts.Length; fpi += _childCount)
            {
                for (var ci = 0; ci < _childCount; ci++)
                {
                    levelParts[fpi + ci] = CreatePart(li, ci, scale);
                }
            }
        }
    }

    private FractalPart CreatePart(int levelIndex, int childIndex, float scale)
    {
        var go = new GameObject($"Fractal Path L{levelIndex} C {childIndex}");
        go.transform.SetParent(transform, false);
        go.transform.localScale = Vector3.one * scale;
        go.AddComponent<MeshFilter>().mesh = _mesh;
        go.AddComponent<MeshRenderer>().material = _material;
        return new FractalPart()
        {
            Direction = _directions[childIndex],
            Rotation = _rotations[childIndex],
            Transform = go.transform
        };
    }

    private void Update()
    {
        var deltaRotation = Quaternion.Euler(0f, _speedRotation * Time.deltaTime, 0f);
        var rootPart = _parts[0][0];
        rootPart.Rotation *= deltaRotation;
        rootPart.Transform.localRotation = rootPart.Rotation;
        _parts[0][0] = rootPart;
        for (var li = 1; li < _parts.Length; li++)
        {
            var parentParts = _parts[li - 1];
            var levelParts = _parts[li];
            for (var fpi = 0; fpi < levelParts.Length; fpi++)
            {
                var parentTransform = parentParts[fpi / _childCount].Transform;
                var part = levelParts[fpi];
                part.Rotation *= deltaRotation;
                part.Transform.localRotation = parentTransform.localRotation * part.Rotation;
                part.Transform.localPosition = parentTransform.localPosition + parentTransform.localRotation * (_positionOffset * part.Transform.localScale.x * part.Direction);

                levelParts[fpi] = part;
            }

        }
    }
}
