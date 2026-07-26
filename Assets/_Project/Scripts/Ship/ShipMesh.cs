using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class ShipMesh : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        List<CombineInstance> combineList = new List<CombineInstance>();

        Vector3 oldPosition = transform.position;
        Quaternion oldRotation = transform.rotation;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].gameObject == gameObject) continue;
            if (meshFilters[i].sharedMesh == null) continue;

            CombineInstance combine = new CombineInstance();
            combine.mesh = meshFilters[i].sharedMesh;
            combine.transform = meshFilters[i].transform.localToWorldMatrix;
            combineList.Add(combine);

            meshFilters[i].gameObject.SetActive(false);
        }

        MeshFilter targetMeshFilter = GetComponent<MeshFilter>();
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineList.ToArray(), true, true);
        targetMeshFilter.mesh = combinedMesh;

        MeshCollider targetMeshCollider = GetComponent<MeshCollider>();
        targetMeshCollider.sharedMesh = combinedMesh;
        targetMeshCollider.convex = true;

        transform.position = oldPosition;
        transform.rotation = oldRotation;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
