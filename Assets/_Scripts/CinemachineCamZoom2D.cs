using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCamZoom2D : MonoBehaviour
{
    private const float NORMAL_ORTHO_SIZE = 10f;
    public static CinemachineCamZoom2D Instance { get; private set; }
    
    [HideInInspector] public float targetOrthoSize = 10f;
    
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float zoomSpeed = 2f;
        cinemachineCamera.Lens.OrthographicSize = Mathf.Abs(targetOrthoSize - cinemachineCamera.Lens.OrthographicSize) < 0.01f ? targetOrthoSize :
            Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, targetOrthoSize, Time.deltaTime * zoomSpeed);
    }

    public void SetNormalOrthoSize()
    {
        targetOrthoSize = NORMAL_ORTHO_SIZE;
    }
}
