
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Vector2 _cameraSensitivity;

    public void AimChanged(Vector2 delta)
    {
        Vector3 rotationEuler = _camera.rotation.eulerAngles;
        Vector3 newRotation = Vector3.zero;

        newRotation.x = Mathf.Clamp((-Mathf.DeltaAngle(rotationEuler.x, 0) - (delta.y * _cameraSensitivity.y)), -80, 80);
        newRotation.y = (rotationEuler.y + (delta.x * _cameraSensitivity.x));
        newRotation.z = 0;

        _camera.rotation = Quaternion.Euler(newRotation);
    }
}
