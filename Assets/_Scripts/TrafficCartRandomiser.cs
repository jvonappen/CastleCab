using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficCartRandomiser : MonoBehaviour
{
    


    [SerializeField] GameObject _horse;
    [SerializeField] GameObject _cart;
    [SerializeField] GameObject _wheelR;
    [SerializeField] GameObject _wheelL;

    [SerializeField] GameObject[] _cartItems;

    [SerializeField] Material[] _horseColourOptions;
    [SerializeField] Material[] _cartColourOptions;
    [SerializeField] Material[] _wheelColourOptions;

    private void OnEnable()
    {
        RandomizeTrafficSkins();
    }


    void RandomizeTrafficSkins()
    {
        EnableRandomMesh(_cartItems);
        EnableRandomMaterial(_horse, _horseColourOptions);
        EnableRandomMaterial(_cart, _cartColourOptions);
        EnableRandomMaterial(_wheelR, _wheelColourOptions);
        MatchWheelColours(_wheelR, _wheelL);
        
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

    void EnableRandomMaterial(GameObject obj, Material[] materials)
    {
        Renderer objRend = obj.GetComponent<Renderer>(); 

        int randomIndex = Random.Range(0, materials.Length);
        objRend.material = materials[randomIndex];
    }

    void MatchWheelColours(GameObject rW, GameObject lW)
    {
        Renderer wColour = rW.GetComponent<Renderer>();

        Renderer poop = lW.GetComponent<Renderer>();

            poop.material = wColour.material;
    }

}
