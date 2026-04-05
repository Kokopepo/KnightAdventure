using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshSurfaceManagement : MonoBehaviour
{
    public static NavMeshSurfaceManagement Instance { get; private set; }

    private NavMeshSurface _navMeshSurface;

    private void Awake()
    {
        Instance = this;
        _navMeshSurface = GetComponent<NavMeshSurface>();
        _navMeshSurface.hideEditorLogs = true; //во время запкания в консоль много текста выводит. - это отключает вывод мусора
    }

    public void RebakeNavMeshSurface()
    {
        _navMeshSurface.BuildNavMesh(); 
    }
}
