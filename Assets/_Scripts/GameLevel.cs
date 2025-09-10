using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    [SerializeField] private Transform landerStartTransform;
    [SerializeField] private Transform cameraStartTargetTransform;
    [SerializeField] private float zoomOutOrthoSize;

    public int LevelIndex => levelIndex;
    public Transform LanderStartTransform => landerStartTransform;
    public Transform CameraStartTargetTransform => cameraStartTargetTransform;
    public float ZoomOutOrthoSize => zoomOutOrthoSize;
}
