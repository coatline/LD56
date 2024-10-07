using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class CameraFollowWithBarriers : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] bool offsetBarrierPositionsToFitCameraSize;
    [SerializeField] bool useCoords;
    [SerializeField] Vector2 bottomLeftBarrierCoords;
    [SerializeField] Vector2 topRightBarrierCoords;
    [SerializeField] Transform followObject;
    [SerializeField] bool doBarriers;

    [SerializeField] float speedVsZoomMultiplier;
    [SerializeField] float focusZoomInAmount;
    [SerializeField] float initialSpeed;
    [SerializeField] float zoomLerpSpeed;
    [SerializeField] Vector2 zoomLimit;
    [SerializeField] float zoomSpeed;

    float targetZoom;
    float speed;

    private void Start()
    {
        targetZoom = cam.orthographicSize;
        
    }

    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;

        movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.unscaledDeltaTime;

        if (followObject != null)
        {
            if (movement.magnitude > 0)
                followObject = null;
            else
                movement = new Vector3(followObject.position.x - transform.position.x, followObject.position.y - transform.position.y) * Time.unscaledDeltaTime;
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.unscaledDeltaTime;

        //targetZoom -= scrollInput * zoomSpeed * Time.unscaledDeltaTime;
        targetZoom = Mathf.Clamp(targetZoom - scrollInput, zoomLimit.x, zoomLimit.y);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.unscaledDeltaTime * zoomLerpSpeed);

        Vector3 blbc = bottomLeftBarrierCoords;
        Vector3 trbc = topRightBarrierCoords;

        if (doBarriers)
            if (offsetBarrierPositionsToFitCameraSize)
            {
                Vector3 camSize = C.GetCameraSizeInUnits(cam);
                blbc += camSize;
                trbc -= camSize;
            }

        speed = Mathf.Lerp(initialSpeed, initialSpeed * speedVsZoomMultiplier, cam.orthographicSize / zoomLimit.y);

        if (Input.GetKey(KeyCode.LeftShift))
            speed *= 2;

        Vector3 newPos = (transform.position + (movement * speed));

        if (doBarriers) 
            newPos = new Vector3(Mathf.Clamp(newPos.x, blbc.x, trbc.x), Mathf.Clamp(newPos.y, blbc.y, trbc.y), -10);

        transform.position = newPos;
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    public void SetFollow(Transform follow)
    {
        followObject = follow;
        targetZoom = Mathf.Clamp(targetZoom - focusZoomInAmount, zoomLimit.x, zoomLimit.y);
    }
}
