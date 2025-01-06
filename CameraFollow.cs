//using System.Numerics;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // The player to follow
    public float smoothSpeed = 5f;  // How smoothly the camera follows
    public Vector3 offset = new Vector3(0, 5, -10); // Camera offset from player
    public Vector3 viewOffset = new Vector3(0, 2, 0); 

    private void LateUpdate()
    {
        if (target == null)
            return;
        
        // Calculate desired position
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Optional: Make camera look at player
        //Transform tform = gameObject.AddComponent<Transform>();
        target.position = target.position + viewOffset;
        transform.LookAt(target);
        target.position = target.position - viewOffset;
        
    }
}
