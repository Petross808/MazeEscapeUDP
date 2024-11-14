
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Vector2 _cameraSensitivity;

    public void AimChanged(Vector2 delta)
    {
        Vector3 rotationEuler = _camera.rotation.eulerAngles;
        Vector3 newRotation = Vector3.zero;

        //Convert from (90 to 0/360 to 270) range to (90 to 0 to -90) range, add the delta and clamp to (-80, 80)
        newRotation.x = Mathf.Clamp((-Mathf.DeltaAngle(rotationEuler.x, 0) - (delta.y * _cameraSensitivity.y)), -80, 80);
        newRotation.y = (rotationEuler.y + (delta.x * _cameraSensitivity.x));
        newRotation.z = 0;

        _camera.rotation = Quaternion.Euler(newRotation);
    }
}
