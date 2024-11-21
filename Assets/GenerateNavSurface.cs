using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GenerateNavSurface : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NavMeshSurface navMeshSurface = GetComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
    }
}
