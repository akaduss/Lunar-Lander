using TMPro;
using UnityEngine;

public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro multTextMesh;

    private void Awake()
    {
        multTextMesh.text = "x" + GetComponent<LandingPad>().GetScoreMult().ToString();
    }

}
