using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public SkinnedMeshRenderer sharkMesh;
    public Material matBasic;
    public Material matDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trident"))
        {
            StartCoroutine(ColorDamage());
        }
    }

    private IEnumerator ColorDamage()
    {
        Material[] sharkMaterials = new Material[2];

        sharkMaterials[0] = sharkMesh.materials[0];
        sharkMaterials[1] = matDamage;
        
        sharkMesh.materials = sharkMaterials;
        
        yield return new WaitForSeconds(0.5f);
        
        sharkMaterials[0] = sharkMesh.materials[0];
        sharkMaterials[1] = matBasic;
        
        sharkMesh.materials = sharkMaterials;

    }
}
