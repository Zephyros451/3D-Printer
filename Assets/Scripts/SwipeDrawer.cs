using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDrawer : MonoBehaviour
{
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private LayerController layerController;
    [SerializeField] private GameObject[] layers;

    private Vector3 currentCursorPosition;
    private Vector3 lastCursorPosition;
    private Vector3 lastPointPosition = Vector3.zero;

    private Vector3 CursorDirection
    {
        get
        {
            Vector3 currentDirection = currentCursorPosition - lastPointPosition;
            currentDirection.y = 0f;
            return currentDirection;
        }
    }

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
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100))
                {
                    if (hit.collider.CompareTag("CurrentPrintLayer"))
                    {
                        currentCursorPosition = hit.point;
                        Quaternion newRotation = Quaternion.LookRotation(Input.GetTouch(0).deltaPosition, Vector3.zero);
                        cursor.transform.position = currentCursorPosition;
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
                if (hit.collider.CompareTag("CurrentPrintLayer"))
                {
                        currentCursorPosition = hit.point;
                        Quaternion newRotation = Quaternion.LookRotation(CursorDirection, Vector3.zero);
                        cursor.transform.position = currentCursorPosition;
                        cursor.transform.rotation = newRotation;
                }
            }
        }
    }

    private void Print()
    {
        if (lastCursorPosition != Vector3.zero)
        {
            float distanceBetweenPoints = CursorDirection.magnitude;

            if (distanceBetweenPoints > 0.05f) 
            {
                Quaternion newRotation = Quaternion.LookRotation(CursorDirection, Vector3.zero);
                Instantiate(cubePrefab, currentCursorPosition, newRotation, layers[layerController.CurrentLayerIndex].transform);
                lastPointPosition = currentCursorPosition;
            }
        }
    }
}
