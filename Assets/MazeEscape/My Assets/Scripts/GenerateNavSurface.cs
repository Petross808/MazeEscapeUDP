using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GenerateNavSurface : MonoBehaviour
{
    void Awake()
    {
        NavMeshSurface navMeshSurface = GetComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
    }
}
