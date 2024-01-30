using UnityEngine;

public class LightSimulator : MonoBehaviour
{
    [SerializeField] private float _rotationDegressPerSecond = 1f;
    private void FixedUpdate()
    {
        RotateAroundY();
    }

    private void RotateAroundY()
    {
        transform.Rotate(Vector3.up * Time.fixedDeltaTime * _rotationDegressPerSecond);
    }
}
