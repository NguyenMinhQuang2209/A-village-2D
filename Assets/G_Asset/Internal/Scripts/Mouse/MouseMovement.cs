using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private Camera mCamera;
    [SerializeField] private float zoomMin = 1f;
    [SerializeField] private float zoomMax = 1f;
    [SerializeField] private float scrollSensitivity = 1f;
    [SerializeField] private float defaultMouseSize = 2f;
    Vector3 mDragPos;
    [SerializeField] private Vector2 boundXAxis;
    [SerializeField] private Vector2 boundYAxis;
    private void Start()
    {
        mCamera.orthographicSize = defaultMouseSize;
    }
    private void Update()
    {
        CameraChangePosition();
    }
    public void CameraChangePosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mDragPos = mCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 diff = mDragPos - mCamera.ScreenToWorldPoint(Input.mousePosition);
            diff.z = 0.0f;
            diff.x *= mouseSensitivity;
            diff.y *= mouseSensitivity;
            mCamera.transform.position = ClampCameraAngle(mCamera.transform.position + diff);
        }

        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        if (scrollAmount != 0f)
        {
            defaultMouseSize = ClampSize(defaultMouseSize - scrollAmount * scrollSensitivity, zoomMin, zoomMax);
            mCamera.orthographicSize = defaultMouseSize;
            mCamera.transform.position = ClampCameraAngle(mCamera.transform.position);
        }


    }
    public float ClampSize(float size, float min, float max)
    {
        return Mathf.Clamp(size, min, max);
    }
    public Vector3 ClampCameraAngle(Vector3 targetPos)
    {
        Vector2 boundXAxis = GetBoundXAxis();
        Vector2 boundYAxis = GetBoundYAxis();

        boundXAxis.y = MapGenerator.instance.width;
        boundYAxis.y = MapGenerator.instance.height;

        float cHeight = mCamera.orthographicSize;
        float cWidth = mCamera.orthographicSize * mCamera.aspect;

        float newX = Mathf.Clamp(targetPos.x, boundXAxis.x + cWidth, boundXAxis.y - cWidth);
        float newY = Mathf.Clamp(targetPos.y, boundYAxis.x + cHeight, boundYAxis.y - cHeight);
        return new(newX, newY, targetPos.z);
    }
    public Vector2 GetBoundXAxis()
    {
        return boundXAxis;
    }
    public Vector2 GetBoundYAxis()
    {
        return boundYAxis;
    }
}
