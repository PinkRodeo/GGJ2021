using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupVariation : MonoBehaviour
{
    public Mesh[] meshes = new Mesh[0];
    public MeshFilter target;

    private void Awake()
    {
        target.mesh = meshes[Random.Range(0, meshes.Length)];
    }
}
