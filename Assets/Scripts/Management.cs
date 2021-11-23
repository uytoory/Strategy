using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectionState{
    UnitsSelected,
    Frame,
    Other
}
public class Management : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] SelectableObject _hovered;
    [SerializeField] List<SelectableObject> _ListOfSelected = new List<SelectableObject>();

    [SerializeField] Image _frameImage;
    private Vector2 _frameStart;
    private Vector2 _frameEnd;

    [SerializeField] SelectionState CurrentSelectionState;

    void Update()
    {


        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 15f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)){
            if (hit.collider.GetComponent<SelectableCollider>()){
                SelectableObject hitSelectable = hit.collider.GetComponent<SelectableCollider>().SelectableObject;
                if (_hovered){
                    if (_hovered != hitSelectable){
                        _hovered.OnUnHover();
                        _hovered = hitSelectable;
                        _hovered.OnHover();
                    } 
                }else{
                    _hovered = hitSelectable;
                    _hovered.OnHover();
                }
            }else
            {
                UnHoverCurrent();
            }
        }else{
            UnHoverCurrent();
        }

        if (Input.GetMouseButtonUp(0)) {
            if (_hovered)
            {
                if (Input.GetKey(KeyCode.LeftControl) == false)
                {
                    UnSelectAll();
                }
                CurrentSelectionState = SelectionState.UnitsSelected;
                Select(_hovered);
            }
        }

        if(CurrentSelectionState == SelectionState.UnitsSelected)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (hit.collider.tag == "Ground")
                {
                    for (int i = 0; i < _ListOfSelected.Count; i++)
                    {
                        _ListOfSelected[i].WhenClickOnGround(hit.point);
                    }
                }
            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            UnSelectAll();
        }

        //Выделение рамкой
        if(Input.GetMouseButtonDown(0))
        {
            _frameStart = Input.mousePosition;
        }
        if(Input.GetMouseButton(0))
        {
            
            _frameEnd = Input.mousePosition;

            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);

            Vector2 size = max - min;

            if(size.magnitude >10)
            {
                _frameImage.enabled = true;
                _frameImage.rectTransform.anchoredPosition = min;
                _frameImage.rectTransform.sizeDelta = size;

                Rect rect = new Rect(min, size);

                UnSelectAll();
                Unit[] ALlUnits = FindObjectsOfType<Unit>();
                for (int i = 0; i < ALlUnits.Length; i++)
                {
                    Vector2 screenPosition = _camera.WorldToScreenPoint(ALlUnits[i].transform.position);
                    if (rect.Contains(screenPosition))
                    {
                        Select(ALlUnits[i]);
                    }
                }

                CurrentSelectionState = SelectionState.Frame;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            _frameImage.enabled = false;
            if(_ListOfSelected.Count > 0)
            {
                CurrentSelectionState = SelectionState.UnitsSelected;
            }
            else
            {
                CurrentSelectionState = SelectionState.Other;
            }
        }

    }

    void Select(SelectableObject selectableObject)
    {
        if (_ListOfSelected.Contains(selectableObject) == false)
        {
            _ListOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }


    void UnSelectAll()
    {
        for (int i = 0; i < _ListOfSelected.Count; i++)
        {
            _ListOfSelected[i].UnSelect();
        }
        _ListOfSelected.Clear();
        CurrentSelectionState = SelectionState.Other;
    }

    void UnHoverCurrent()
    {
        if (_hovered)
        {
            _hovered.OnUnHover();
            _hovered = null;
        }
    }
}
