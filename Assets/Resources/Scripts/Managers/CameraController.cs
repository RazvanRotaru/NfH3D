using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    PlayerController player;
    Transform focusedObject;

    float yaw, pitch;
    float distanceToTarget;

    public Vector3 cameraOffset = new Vector3(0f, 2f, 3f);
    public float cameraSpeed = 10f;

    public float maxZoom = 3f;
    float headTilt = 0f;
    public float threshold = 0.2f;
    int sign = 1;

    public Vector2 pitchLimits = new Vector2(-30f, 30f);
    public Vector2 yawLimits = new Vector2(-45f, 45f);
    float zoomAmount = 0f;
    float targetZoomAmount = 0f;
    Vector3 zoomDir;
    Vector2 sensitivity = new Vector2(5, 5);
    public Vector2 Sensitivity { get => sensitivity; set => sensitivity = value; }

    void Start()
    {
        player = PlayerController.instance;

        distanceToTarget = cameraOffset.z;
        cameraOffset.z = 0f;

        sensitivity.y = sensitivity.x * Screen.height / Screen.width;

        Interactible.OnHover += SetFocusedObject;
        Interactible.OnExit += RemoveFocusedObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UserRotateCamera();

        Quaternion additionalRotation = Quaternion.Euler(0, yaw, 0);
        Quaternion toFocusedObjectRotation = GetRotationToFocusedObject();
        Quaternion cameraRotation = additionalRotation *
                                         player.GetRotation();

        transform.rotation = Quaternion.Slerp(transform.rotation,
                                                cameraRotation, Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                                toFocusedObjectRotation * transform.rotation, Time.deltaTime * 0.2f);

        headTilt = yaw;
        headTilt = Mathf.Max(headTilt, 5);

        player.RotateHead(additionalRotation, headTilt);

        if (cameraOffset.x * transform.forward.z < 0)
            cameraOffset.x *= -1;

        Vector3 additionPosition = -transform.forward * distanceToTarget
                                                            + cameraOffset;
        Vector3 toFocusedObjectPosition = GetPositionToFocusedObject();
        Vector3 cameraPosition = player.GetPosition()
                                    + additionPosition
                                       + toFocusedObjectPosition;
        transform.position = Vector3.Lerp(transform.position, cameraPosition
                                                    , Time.deltaTime * cameraSpeed);
    }

    private void UserRotateCamera()
    {
        float inX = Input.GetAxis("Mouse X");
        if (yaw * inX < 0 && inX > threshold)
            sign *= -1;

        yaw += inX * sensitivity.x;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity.y;

        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);
        yaw = Mathf.Clamp(yaw, yawLimits.x, yawLimits.y);
    }

    void ChangeSensitivity(float value)
    {
        sensitivity.x = value;
        sensitivity.y = sensitivity.x * Screen.height / Screen.width;
    }

    Quaternion GetRotationToFocusedObject()
    {
        if (focusedObject == null)
            return Quaternion.identity;

        Vector3 dir = (focusedObject.position - transform.position).normalized;
        
        float theta = Mathf.Acos(Vector3.Dot(transform.forward, dir));
        theta *= Mathf.Rad2Deg;
        //Vector3 axis = Vector3.Cross(transform.forward, dir);

        return Quaternion.AngleAxis(theta, transform.up);
    }

    Vector3 GetPositionToFocusedObject()
    {
        if (focusedObject == null)
            targetZoomAmount = 0f;
        else
        {
            targetZoomAmount = maxZoom;
            zoomDir = (focusedObject.position - transform.position);
        }
        zoomAmount = Mathf.Lerp(zoomAmount, targetZoomAmount, Time.deltaTime * 2f);
        zoomDir = zoomDir.normalized * zoomAmount;

        return zoomDir;
    }

    void SetFocusedObject(Interactible interactible)
    {
        focusedObject = interactible.gameObject.transform;
    }

    void RemoveFocusedObject(Interactible dummy)
    {
        focusedObject = null;
    }
}
