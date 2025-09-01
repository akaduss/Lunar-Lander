using UnityEngine;

public class LandingPad : MonoBehaviour
{
    [SerializeField] private int scoreMult = 1;

    public int GetScoreMult() => scoreMult;
}
