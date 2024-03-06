using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSkins : MonoBehaviour
{
    [SerializeField] GameObject[] _bottomMesh;
    [SerializeField] GameObject[] _faceMesh;
    [SerializeField] GameObject[] _headMesh;
    [SerializeField] GameObject[] _topMesh;
    // Start is called before the first frame update

    private void OnEnable()
    {
        RandomizeSkins();
    }

    void RandomizeSkins()
    {
        EnableRandomMesh(_bottomMesh);
        EnableRandomMesh(_faceMesh);
        EnableRandomMesh(_headMesh);
        EnableRandomMesh(_topMesh);
    }

    void EnableRandomMesh(GameObject[] meshes)
    {
        // Disable all meshes
        foreach (var mesh in meshes)
        {
            mesh.SetActive(false);
        }

        // Enable a random mesh
        int randomIndex = Random.Range(0, meshes.Length);
        meshes[randomIndex].SetActive(true);
    }
}
