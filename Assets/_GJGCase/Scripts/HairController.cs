using UnityEngine;
using Random = UnityEngine.Random;

public class HairController : MonoBehaviour
{
    public int mappedVertexIndex;
    public Vector3 offsetFromMappedVertex;
    public Quaternion rotationFromMappedVertex;
    public float vertexWaxLevel;
    private MeshRenderer _meshRenderer;
    private static readonly int Anim = Shader.PropertyToID("_Anim");
    private static readonly int Variation = Shader.PropertyToID("_Variation");

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material.SetFloat(Variation, Random.value);
    }

    private void Update()
    {
        _meshRenderer.material.SetFloat(Anim, Mathf.Clamp01(.3f - vertexWaxLevel));
    }
}
