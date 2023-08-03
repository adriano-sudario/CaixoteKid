using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraMode { Normal, Smooth }

    public GameObject target;
    public float smoothness = .125f;
    public Vector3 offset = new Vector3(0, 5, -10);
    public CameraMode mode = CameraMode.Smooth;

    void FixedUpdate()
    {
        var position = target.transform.position + offset;

        if (mode == CameraMode.Smooth)
            position = Vector3.Lerp(transform.position, position, smoothness);

        transform.position = position;
        transform.LookAt(target.transform);
    }
}
