using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLight : MonoBehaviour
{
    public SkinnedMeshRenderer _SkinnedMeshRenderer;

    public MeshRenderer _MeshRenderer;

    public Material highLightMaterial;

    private Material[] materialsStartSkined;
    private Material[] materialsStartRenderer;
    void Start()
    {
        if (GetComponent<SkinnedMeshRenderer>() != null)
        {
            _SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        }
        if (GetComponent<MeshRenderer>() != null)
        {
            _MeshRenderer = GetComponent<MeshRenderer>();
        }
        
        if (_SkinnedMeshRenderer != null)
        {
            materialsStartSkined = _SkinnedMeshRenderer.materials;
        }

        if (_MeshRenderer != null)
        {
            materialsStartRenderer = _MeshRenderer.materials;
        }
    }
    
    public void SetColorOn()
    {
        if (_SkinnedMeshRenderer != null)
        {
            Material[] materials = _SkinnedMeshRenderer.materials;
            Material[] materials2 = _SkinnedMeshRenderer.materials;
            for (int i = 0; i < _SkinnedMeshRenderer.materials.Length; i++)
            {
                materials2[i] = highLightMaterial;
            }

            _SkinnedMeshRenderer.sharedMaterials = materials2;
        }
        
        if (_MeshRenderer != null)
        {
            Material[] materials = _MeshRenderer.materials;
            for (int i = 0; i < _MeshRenderer.materials.Length; i++)
            {
                materials[i] = highLightMaterial;
            }

            _MeshRenderer.sharedMaterials = materials;
        }
    }
    
    public void SetColorOff()
    {
        if (_SkinnedMeshRenderer != null)
        {
            _SkinnedMeshRenderer.sharedMaterials = materialsStartSkined;
        }
        
        if (_MeshRenderer != null)
        {
            _MeshRenderer.sharedMaterials = materialsStartRenderer;
        }
    }
}
