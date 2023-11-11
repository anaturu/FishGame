using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public SkinnedMeshRenderer sharkMesh;
    public Material matBasic;
    public Material matDamage;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Trident"))
        {
            StartCoroutine(ColorDamage());
        }
    }

    private IEnumerator ColorDamage()
    {
        sharkMesh.materials[0] = matDamage;
        yield return new WaitForSeconds(1f);
        
        sharkMesh.materials[0] = matBasic;

    }
}
