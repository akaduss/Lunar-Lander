using UnityEngine;

public class LanderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticle;
    [SerializeField] private ParticleSystem midThrusterParticle;
    [SerializeField] private ParticleSystem rightThrusterParticle;

    private Lander lander;

    private void Awake()
    {
        lander = GetComponent<Lander>();

        lander.OnRightKeyPressed += Lander_OnRightKeyPressed;
        lander.OnLeftKeyPressed += Lander_OnLeftKeyPressed;
        lander.OnUpKeyPressed += Lander_OnUpKeyPressed;
        lander.OnBeforeForce += Lander_OnBeforeForce;
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        SetThruster(midThrusterParticle, false);
        SetThruster(leftThrusterParticle, false);
        SetThruster(rightThrusterParticle, false);
    }

    private void Lander_OnUpKeyPressed(object sender, System.EventArgs e)
    {
        SetThruster(midThrusterParticle, true);
        SetThruster(leftThrusterParticle, true);
        SetThruster(rightThrusterParticle, true);
    }

    private void Lander_OnLeftKeyPressed(object sender, System.EventArgs e)
    {
        SetThruster(midThrusterParticle, true);
        SetThruster(leftThrusterParticle, false);
        SetThruster(rightThrusterParticle, true);
    }

    private void Lander_OnRightKeyPressed(object sender, System.EventArgs e)
    {
        SetThruster(midThrusterParticle, true);
        SetThruster(leftThrusterParticle, true);
        SetThruster(rightThrusterParticle, false);
    }

    private void SetThruster(ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = enabled;
    }
}
