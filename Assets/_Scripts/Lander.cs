using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    public static Lander Instance { get; private set; }

    private const float GRAVITY = 0.7f;

    public event EventHandler OnLeftKeyPressed;
    public event EventHandler OnRightKeyPressed;
    public event EventHandler OnUpKeyPressed;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler OnFuelPickup;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotProduct;
        public float speed;
        public int scoreMultiplier;
    }

    public enum LandingType
    {
        Success,
        NotLandingPad,
        TooFast,
        TooSteepAngle
    }

    public enum State
    {
        WaitingStart,
        Normal,
        GameOver,
    }

    
    [SerializeField] private float thrust = 1f;
    [SerializeField] private float torque = 1f;
    private float fuel = 10f;
    private float fuelMax = 10f;
    private int coin = 0;
    private State state;

    private Rigidbody2D rb2D;

    private void Awake()
    {
        Instance = this;
        
        rb2D = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = -1;

        rb2D.gravityScale = 0f;
        state = State.WaitingStart;
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (state)
        {
            case State.WaitingStart:
                if (InputManager.Instance.IsUpPressed() || InputManager.Instance.IsLeftPressed() 
                    || InputManager.Instance.IsRightPressed() || InputManager.Instance.GetMovementVector() != Vector2.zero)
                {
                    rb2D.gravityScale = GRAVITY;
                    SetState(State.Normal);
                }
                break;
            case State.Normal:
                if (fuel <= 0f)
                {

                    return;
                }

                if (InputManager.Instance.IsUpPressed() || InputManager.Instance.IsLeftPressed()
                    || InputManager.Instance.IsRightPressed() || InputManager.Instance.GetMovementVector() != Vector2.zero)
                {
                    fuel -= Time.deltaTime;
                }

                float gamepadDeadzone = 0.2f;
                if (InputManager.Instance.IsUpPressed() || InputManager.Instance.GetMovementVector().y > gamepadDeadzone)
                {
                    rb2D.AddForce(thrust * Time.deltaTime * transform.up);
                    OnUpKeyPressed?.Invoke(this, EventArgs.Empty);
                }

                if (InputManager.Instance.IsLeftPressed() || InputManager.Instance.GetMovementVector().x < -gamepadDeadzone)
                {
                    rb2D.AddTorque(torque * Time.deltaTime);
                    OnLeftKeyPressed?.Invoke(this, EventArgs.Empty);
                }

                if (InputManager.Instance.IsRightPressed() || InputManager.Instance.GetMovementVector().x > gamepadDeadzone)
                {
                    rb2D.AddTorque(-torque * Time.deltaTime);
                    OnRightKeyPressed?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }



    }

    private void SetState(State newState)
    {
        state = newState;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = newState });
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if(collision2D.transform.TryGetComponent(out LandingPad landingPad) == false)
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.NotLandingPad,
                score = 0,
                dotProduct = 0f,
                speed = 0,
                scoreMultiplier = 0
            });
            SetState(State.GameOver);

            return;
        }

        float softLandVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandVelocityMagnitude)
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFast,
                score = 0,
                dotProduct = 0f,
                speed = relativeVelocityMagnitude,
                scoreMultiplier = 0
            });
            SetState(State.GameOver);

            return;
        }

        float minDotProduct = 0.9f;
        float dotProduct = Vector2.Dot(Vector2.up, transform.up);
        if (dotProduct < minDotProduct)
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                score = 0,
                dotProduct = dotProduct,
                speed = relativeVelocityMagnitude,
                scoreMultiplier = 0
            });
            SetState(State.GameOver);

            return; 
        }

        float maxScoreLandingAngle = 100;
        float scoreDotProductMult = 10f;
        float landingAngleScore = maxScoreLandingAngle - Mathf.Abs(dotProduct - 1f) * scoreDotProductMult * maxScoreLandingAngle;

        float maxScoreLandingSpeed = 100;
        float landingSpeedScore = (softLandVelocityMagnitude - relativeVelocityMagnitude) * maxScoreLandingSpeed;

        int score = Mathf.RoundToInt(landingAngleScore + landingSpeedScore) * landingPad.GetScoreMult();

        OnLanded?.Invoke(this, new OnLandedEventArgs 
        { 
            landingType = LandingType.Success,
            score = score,
            dotProduct = dotProduct,
            speed = relativeVelocityMagnitude,
            scoreMultiplier = landingPad.GetScoreMult()
        });
        SetState(State.GameOver);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            fuel += fuelPickup.GetFuelAmount();
            if(fuel > fuelMax) fuel = fuelMax;
            fuelPickup.DestroySelf();
            OnFuelPickup?.Invoke(this, EventArgs.Empty);
        }

        if (collision.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            coin += 1;
            coinPickup.DestroySelf();
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
        }
    }

    public float GetX_Speed() => rb2D.linearVelocityX;
    public float GetY_Speed() => rb2D.linearVelocityY;
    public float GetNormalizedFuel() => fuel / fuelMax;

}
 