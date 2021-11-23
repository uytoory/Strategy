using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : SelectableObject
{
    public int Price;
    [SerializeField] Renderer _renderer;
    public int _xSize = 3;
    public int _zSize = 3;

    private Color _startColor;

    private void Awake()
    {
        _startColor = _renderer.material.color;
    }

    private void OnDrawGizmos()
    {
        float cellsize = FindObjectOfType<BuildingPlacer>().CellSize;
        for (int x = 0; x < _xSize; x++)
        {
            for (int z = 0; z < _zSize; z++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, z) * cellsize, new Vector3(1f, 0f, 1f) * cellsize);
            }
        }
    }

    public void DisplayUnacceptablePosition()
    {
        _renderer.material.color = Color.red;
    }

    public void DisplayAcceptablePosition()
    {
        _renderer.material.color = _startColor;
    }
}
