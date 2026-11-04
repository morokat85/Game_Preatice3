
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Framing")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 12f, -8f);
    [SerializeField] private float followSharpness = 5f;

    [Header("Zoom")]
    [SerializeField] private float minZoom = 6f;
    [SerializeField] private float maxZoom = 18f;
    [SerializeField] private float zoomSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // TODO 8: Mouse Scroll Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        float distance = Mathf.Clamp(
            offset.magnitude - scroll * zoomSpeed,
            minZoom,
            maxZoom
        );

        offset = offset.normalized * distance;

        // Follow Player
        Vector3 desired = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desired,
            followSharpness * Time.deltaTime
        );

        // Look at Player
        Quaternion targetRotation = Quaternion.LookRotation(
            target.position - transform.position
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            followSharpness * Time.deltaTime
        );
    }
}
