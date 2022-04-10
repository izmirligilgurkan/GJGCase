using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _GurkanTemplate.Scripts;
using DG.Tweening;
using Lofelt.NiceVibrations;
using UnityEngine;

public class WaxController : MonoBehaviour
{
    [SerializeField] private GameObject liquidFXPrefab;
    [SerializeField] private GameObject decalFXPrefab;
    
    
    [SerializeField] private BrushController brushController;
    [SerializeField][Range(0.001f, 5f)] private float brushEffectArea = 1f;
    [SerializeField] private float transitionThreshold = 200f;
    [SerializeField] private Color waxDryColor;
    private List<HairController> _hairControllers;
    private MeshRenderer _meshRenderer;
    private bool _readyForTransitionToPulling;
    private bool _peelMode;
    private bool _readyForTransitionToFinish;
    private float _timeSinceLastFeedback;
    private MeshFilter _meshFilter;
    private Vector3[] _cachedVertices;
    private void OnEnable()
    {
        GameManager.TransitionToStarted += Transition;
    }

    private void OnDisable()
    {
        GameManager.TransitionToStarted -= Transition; 
    }

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _hairControllers = FindObjectsOfType<HairController>().ToList();
        MeshDeformInitialization();
        foreach (var hairController in _hairControllers)
        {
            MapHairToVertex(hairController);
        }
    }
    
    private void Update()
    {
        _timeSinceLastFeedback += Time.deltaTime;
        if (brushController.shouldPaint)
        {
            if (!_peelMode)
            {
                PaintWax(brushController.transform.position);
            }
            else
            {
                PeelOff(brushController.transform.position);
            }
        }

        if (_peelMode)
        {
            foreach (var hairController in _hairControllers)
            {
                SetHairTransformFromVertex(hairController);
            }
        }
        
    }

    private void MeshDeformInitialization()
    {
        var originalMesh = _meshFilter.sharedMesh;
        var mesh = new Mesh
        {
            vertices = originalMesh.vertices,
            triangles = originalMesh.triangles,
            normals = originalMesh.normals,
            uv = originalMesh.uv
        };
        
        var vertices = originalMesh.vertices;
        
        _cachedVertices = originalMesh.vertices;
        
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].z += 45.5f;
        }

        mesh.vertices = vertices;
        mesh.Optimize();
        
        _meshFilter.sharedMesh = mesh;
    }

    private void MapHairToVertex(HairController hairController)
    {
        Matrix4x4 localToWorld = transform.localToWorldMatrix;
        int closestIndex = 0;
        float closestDistance = 10000f;

        for (int i = 0; i < _cachedVertices.Length; i++)
        {
            var vertWorldPos = localToWorld.MultiplyPoint3x4(_cachedVertices[i]);
            var dist = Vector3.Distance(vertWorldPos, hairController.transform.position);
            if (dist < closestDistance)
            {
                closestIndex = i;
                closestDistance = dist;
            }
        }
        
        var normals = _meshFilter.mesh.normals;
        var closestVertWorldPos = localToWorld.MultiplyPoint3x4(_cachedVertices[closestIndex]);
        var closestVertWorldNormal = localToWorld.MultiplyVector(normals[closestIndex]);
        hairController.mappedVertexIndex = closestIndex;
        hairController.offsetFromMappedVertex = hairController.transform.position - closestVertWorldPos;
        hairController.rotationFromMappedVertex = Quaternion.FromToRotation(closestVertWorldNormal, hairController.transform.up);
    }

    private void SetHairWaxLevel(HairController hairController)
    {
        hairController.vertexWaxLevel = _meshFilter.mesh.colors[hairController.mappedVertexIndex].a;
    }

    private void SetHairTransformFromVertex(HairController hairController)
    {
        Matrix4x4 localToWorld = transform.localToWorldMatrix;

        var vertices = _meshFilter.mesh.vertices;
        var normals = _meshFilter.mesh.normals;
        var vertWorldPos = localToWorld.MultiplyPoint3x4(vertices[hairController.mappedVertexIndex]);
        var vertWorldNormal = localToWorld.MultiplyVector(normals[hairController.mappedVertexIndex]);

        hairController.transform.position = vertWorldPos + hairController.offsetFromMappedVertex;
        hairController.transform.up = Vector3.Lerp(hairController.transform.up, hairController.rotationFromMappedVertex * vertWorldNormal, .001f);
    }

   

    private void PaintWax(Vector3 worldPos)
    {
        Matrix4x4 localToWorld = transform.localToWorldMatrix;
        var originalMesh = _meshFilter.sharedMesh; 
        var mesh = new Mesh
        {
            vertices = originalMesh.vertices,
            colors = originalMesh.colors,
            triangles = originalMesh.triangles,
            normals = originalMesh.normals,
            uv = originalMesh.uv
        };
        var vertices = mesh.vertices;
        var colors = new Color[vertices.Length];
        
        float totalDiff = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            var vertWorldPos = localToWorld.MultiplyPoint3x4(_cachedVertices[i]);
            var dist = Vector3.Distance(vertWorldPos, worldPos);
            var weight = Mathf.Clamp01((brushEffectArea - dist) / brushEffectArea);
            vertices[i] = Vector3.Lerp(vertices[i],_cachedVertices[i], weight *.5f);
            var finalDist = Vector3.Distance(vertices[i], _cachedVertices[i]);
            colors[i].a = Mathf.Clamp01((brushEffectArea - finalDist) / brushEffectArea);
            totalDiff += finalDist;
            if(finalDist > 2f && weight > .7f) PaintWaxFeedback(vertWorldPos);
        }
        if(totalDiff < transitionThreshold) ReadyForTransitionToPulling();
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.RecalculateNormals();
        _meshFilter.sharedMesh = mesh;
        foreach (var hairController in _hairControllers)
        {
            SetHairWaxLevel(hairController);
        }
    }

    private void PeelOff(Vector3 worldPos)
    {
        Matrix4x4 localToWorld = transform.localToWorldMatrix;
        var originalMesh = _meshFilter.sharedMesh; 
        var mesh = new Mesh
        {
            vertices = originalMesh.vertices,
            colors = originalMesh.colors,
            triangles = originalMesh.triangles,
            normals = originalMesh.normals,
            uv = originalMesh.uv
        };
        var vertices = mesh.vertices;
        var colors = new Color[vertices.Length];
        
        float totalDiff = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            var vertWorldPos = localToWorld.MultiplyPoint3x4(_cachedVertices[i]);
            var dist = Vector3.Distance(vertWorldPos, worldPos);
            var weight = Mathf.Clamp01((brushEffectArea * 2f - dist) / (brushEffectArea * 2f));
            vertices[i].z = Mathf.Lerp(vertices[i].z, _cachedVertices[i].z - 100f, weight * Time.deltaTime * 10f);
            var finalDist = Mathf.Abs(vertices[i].z - (_cachedVertices[i].z - 100f));
            colors[i].a = Mathf.Clamp01((brushEffectArea * 2f - finalDist) / (brushEffectArea * 2f));
            totalDiff += finalDist;
            if(finalDist > 2f && weight > .7f) PeelOffFeedback(vertWorldPos);
        }
        
        if(totalDiff < transitionThreshold) ReadyForTransitionToFinish();

        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.RecalculateNormals();
        _meshFilter.sharedMesh = mesh;
    }

    private void ReadyForTransitionToPulling()
    {
        if(_readyForTransitionToPulling) return;
        _readyForTransitionToPulling = true;
        GameManager.ReadyForTransition();
    }
    private void ReadyForTransitionToFinish()
    {
        if(_readyForTransitionToFinish) return;
        _readyForTransitionToFinish = true;
        GameManager.ReadyForTransition();
    }

    private void Transition()
    {
        if (!_peelMode)
        {
            TransitionToPulling();
        }
        else
        {
            TransitionToFinish();
        }
    }
    
    private void TransitionToPulling()
    {
        
        _meshRenderer.material.DOColor(waxDryColor, 1f).OnComplete(() =>
        {
            GameManager.TransitionComplete();
            _peelMode = true;
            foreach (var hairController in _hairControllers)
            {
                hairController.vertexWaxLevel = 1f;
            }
        });
    }
    
    private void TransitionToFinish()
    {
        foreach (var hairController in _hairControllers)
        {
            hairController.transform.SetParent(transform, true);
        }
        transform.DOMove(Vector3.left * .3f, .3f).SetRelative().OnComplete(() =>GameManager.GameEnd(true)).SetUpdate(UpdateType.Late);
    }

    private void PaintWaxFeedback(Vector3 pos)
    {
        if(_timeSinceLastFeedback < .2f) return;
        _timeSinceLastFeedback = 0f;
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
        var fx = Instantiate(liquidFXPrefab);
        fx.transform.position = pos;
    }
    
    private void PeelOffFeedback(Vector3 pos)
    {
        if(_timeSinceLastFeedback < .2f) return;
        _timeSinceLastFeedback = 0f;
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
        var fx = Instantiate(decalFXPrefab);
        fx.transform.position = pos;
    }


}
