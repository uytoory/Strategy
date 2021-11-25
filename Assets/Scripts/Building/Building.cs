using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : SelectableObject
{
    public int Price;
    public int _xSize = 3;
    public int _zSize = 3;
    [SerializeField] Renderer _renderer;
    [SerializeField] GameObject _menuObject;


    private Color _startColor;

    public override void Start()
    {
        base.Start();
        _menuObject.SetActive(false);
    }

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


    public override void Select()
    {
        base.Select();
        _menuObject.SetActive(true);

    }

    public override void UnSelect()
    {
        base.UnSelect();
        _menuObject.SetActive(false);
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
