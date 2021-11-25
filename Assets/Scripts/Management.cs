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
    public SelectableObject Hovered;
    public List<SelectableObject> ListOfSelected = new List<SelectableObject>();

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
                if (Hovered){
                    if (Hovered != hitSelectable){
                        Hovered.OnUnHover();
                        Hovered = hitSelectable;
                        Hovered.OnHover();
                    } 
                }else{
                    Hovered = hitSelectable;
                    Hovered.OnHover();
                }
            }else
            {
                UnHoverCurrent();
            }
        }else{
            UnHoverCurrent();
        }

        if (Input.GetMouseButtonUp(0)) {
            if (Hovered)
            {
                if (Input.GetKey(KeyCode.LeftControl) == false)
                {
                    UnSelectAll();
                }
                CurrentSelectionState = SelectionState.UnitsSelected;
                Select(Hovered);
            }
        }

        if(CurrentSelectionState == SelectionState.UnitsSelected)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (hit.collider.tag == "Ground")
                {
                    int rowNumber = Mathf.CeilToInt(Mathf.Sqrt(ListOfSelected.Count));

                    for (int i = 0; i < ListOfSelected.Count; i++)
                    {
                        int row = i / rowNumber;
                        int column = i % rowNumber;

                        Vector3 point = hit.point + new Vector3(row, 0f, column);
                        ListOfSelected[i].WhenClickOnGround(point);
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
            if(ListOfSelected.Count > 0)
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
        if (ListOfSelected.Contains(selectableObject) == false)
        {
            ListOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }

    public void Unselect(SelectableObject selectableObject)
    {
        if(ListOfSelected.Contains(selectableObject))
        {
            ListOfSelected.Remove(selectableObject);
        }
    }


    void UnSelectAll()
    {
        for (int i = 0; i < ListOfSelected.Count; i++)
        {
            ListOfSelected[i].UnSelect();
        }
        ListOfSelected.Clear();
        CurrentSelectionState = SelectionState.Other;
    }

    void UnHoverCurrent()
    {
        if (Hovered)
        {
            Hovered.OnUnHover();
            Hovered = null;
        }
    }
}
