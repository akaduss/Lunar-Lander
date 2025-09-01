using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    public event EventHandler OnLeftKeyPressed;
    public event EventHandler OnRightKeyPressed;
    public event EventHandler OnUpKeyPressed;
    public event EventHandler OnBeforeForce;


    [SerializeField] private float thrust = 1f;
    [SerializeField] private float torque = 1f;

    private Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = -1;
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        if(Keyboard.current.wKey.isPressed)
        {
            rb2D.AddForce(thrust * Time.deltaTime * transform.up);
            OnUpKeyPressed?.Invoke(this, EventArgs.Empty);
        }

        if(Keyboard.current.aKey.isPressed)
        {
            rb2D.AddTorque(torque * Time.deltaTime);
            OnLeftKeyPressed?.Invoke(this, EventArgs.Empty);
        }

        if(Keyboard.current.dKey.isPressed)
        {
            rb2D.AddTorque(-torque * Time.deltaTime);
            OnRightKeyPressed?.Invoke(this, EventArgs.Empty);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if(collision2D.transform.TryGetComponent(out LandingPad landingPad) == false)
        {
            Debug.Log("Not landed on landing pad");
            return;
        }

        float softLandVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandVelocityMagnitude)
        {
            print("crash too fast");
            return;
        }

        float minDotProduct = 0.9f;
        float dotProduct = Vector2.Dot(Vector2.up, transform.up);
        if (dotProduct < minDotProduct)
        {
            print("crash steep angle");
        }

        Debug.Log("Successful Landing");

        float maxScoreLandingAngle = 100;
        float scoreDotProductMult = 10f;
        float landingAngleScore = maxScoreLandingAngle - Mathf.Abs(dotProduct - 1f) * scoreDotProductMult * maxScoreLandingAngle;

        float maxScoreLandingSpeed = 100;
        float landingSpeedScore = (softLandVelocityMagnitude - relativeVelocityMagnitude) * maxScoreLandingSpeed;

        Debug.Log("Landing Angle Score: " + landingAngleScore);
        Debug.Log("Landing Speed Score: " + landingSpeedScore);

        int score = Mathf.RoundToInt(landingAngleScore + landingSpeedScore) * landingPad.GetScoreMult();

        Debug.Log($"Total Score: {score}");
    }

}
