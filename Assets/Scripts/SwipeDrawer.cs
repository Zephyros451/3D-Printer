using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDrawer : MonoBehaviour
{
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject currentLayer;

    private Vector3 currentCursorPosition;
    private Vector3 lastCursorPosition;
    private Vector3 lastPointPosition = Vector3.zero;

    private Camera mainCamera;

    public float CurrentLayerHeight { get; set; }

    private void Awake()
    {
        mainCamera = Camera.main;
        CurrentLayerHeight = 1;
    }

    private void Update()
    {
        MoveCursor();
        Print();
        lastCursorPosition = currentCursorPosition;
    }

    private void MoveCursor()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                if (hit.collider.gameObject.TryGetComponent(out PrintLayer currentLayer))
                {
                    if (currentLayer != null && currentLayer.LayerDone)
                    {
                        Vector3 newPosition = new Vector3(Input.GetTouch(0).position.x, CurrentLayerHeight, Input.GetTouch(0).position.y);
                        Quaternion newRotation = Quaternion.LookRotation(Input.GetTouch(0).deltaPosition, transform.up);
                        cursor.transform.position = newPosition;
                        cursor.transform.rotation = newRotation;
                    }
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                if (hit.collider.gameObject.TryGetComponent(out PrintLayer currentLayer))
                {
                    if (currentLayer != null && currentLayer.LayerDone)
                    {
                        currentCursorPosition = hit.point;
                        Vector3 newPosition = new Vector3(currentCursorPosition.x, CurrentLayerHeight, currentCursorPosition.z);
                        Quaternion newRotation = Quaternion.LookRotation(currentCursorPosition - lastCursorPosition, transform.up);
                        cursor.transform.position = newPosition;
                        cursor.transform.rotation = newRotation;                        
                    }
                }
            }
        }
    }

    private void Print()
    {
        if (lastCursorPosition != Vector3.zero)
        {
            float distanceBetweenPoints = (currentCursorPosition - lastPointPosition).magnitude;

            if (distanceBetweenPoints > 0.08f) 
            {
                Vector3 newPosition = new Vector3(currentCursorPosition.x, CurrentLayerHeight, currentCursorPosition.z);
                Quaternion newRotation = Quaternion.LookRotation(currentCursorPosition - lastPointPosition, transform.up);
                Instantiate(cubePrefab, newPosition, newRotation, currentLayer.transform);
                lastPointPosition = currentCursorPosition;
            }
        }
    }

    public void NextLayer()
    {
        CurrentLayerHeight += 0.1f;
    }
}
