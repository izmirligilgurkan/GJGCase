using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _GJGCase.Scripts
{
    public class HairController : MonoBehaviour
    {
        public int mappedVertexIndex;
        public Vector3 offsetFromMappedVertex;
        public Quaternion rotationFromMappedVertex;
        public float vertexWaxLevel;
        private MeshRenderer _meshRenderer;
        private static readonly int Anim = Shader.PropertyToID("_Anim");
        private static readonly int Variation = Shader.PropertyToID("_Variation");
        private bool _peeledOff;
        private Vector3 _initPos;
        public event Action<Vector3> Peeled;

        private void Awake()
        {
            _initPos = transform.position;
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.material.SetFloat(Variation, Random.value);
        }

        private void Update()
        {
            _meshRenderer.material.SetFloat(Anim, Mathf.Clamp01(.3f - vertexWaxLevel));
            if (!_peeledOff && Vector3.Distance(_initPos, transform.position) > .1f)
            {
                _peeledOff = true;
                Peeled?.Invoke(_initPos);
            }
        }
    }
}
