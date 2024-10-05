using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class CameraFollowWithBarriers : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] bool offsetBarrierPositionsToFitCameraSize;
    [SerializeField] float manualMovementSpeed;
    [SerializeField] bool useCoords;
    [SerializeField] Vector2 bottomLeftBarrierCoords;
    [SerializeField] Vector2 topRightBarrierCoords;

    [SerializeField] Transform followObject;

    [SerializeField] bool doBarriers = true;

    [Range(.01f, 1f)]
    [SerializeField] float speed;
    [SerializeField] float zoomSmoothTime;
    [SerializeField] float zoomSpeed;
    [SerializeField] float minZoom;
    [SerializeField] float maxZoom;
    [SerializeField] float focusZoomInAmount;
    float zoomVelocity;
    float targetZoom;

    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Time.deltaTime * manualMovementSpeed;

        if (followObject != null)
        {
            if (movement.magnitude > 0)
                followObject = null;
            else
                movement = new Vector3(followObject.position.x - transform.position.x, followObject.position.y - transform.position.y);
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollInput * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);  // Clamping the zoom level

        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, zoomSmoothTime);

        Vector3 blbc = bottomLeftBarrierCoords;
        Vector3 trbc = topRightBarrierCoords;

        if (doBarriers)
            if (offsetBarrierPositionsToFitCameraSize)
            {
                Vector3 camSize = C.GetCameraSizeInUnits(cam);
                blbc += camSize;
                trbc -= camSize;
            }

        Vector3 newPos = (transform.position + (movement * speed));

        if (doBarriers) { newPos = new Vector3(Mathf.Clamp(newPos.x, blbc.x, trbc.x), Mathf.Clamp(newPos.y, blbc.y, trbc.y), -10); }

        transform.position = newPos;
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    public void SetFollow(Transform follow)
    {
        followObject = follow;
        targetZoom = Mathf.Clamp(targetZoom - focusZoomInAmount, minZoom, maxZoom);  // Clamping the zoom level
    }
}
