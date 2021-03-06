using UnityEngine;

namespace HouraiTeahouse.SmashBrew {

    [RequireComponent(typeof(Ground), typeof(Rigidbody))]
    public sealed class Gravity : MonoBehaviour {

        [SerializeField]
        [Tooltip("The acceleration downward per second applied")]
        float _gravity = 9.86f;

        Ground _ground;
        Rigidbody _rigidbody;

        /// <summary> Gets or sets the magnitude of gravity applied to the Character. </summary>
        public float Force {
            get { return _gravity; }
            set { _gravity = Mathf.Abs(value); }
        }

        public static implicit operator float(Gravity gravity) { return gravity != null ? gravity.Force : 0f; }

        void Awake() {
            _ground = GetComponent<Ground>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate() {
            float grav = Force;
            //Simulates ground friction.
            if (_ground)
                grav *= 2.5f;
            _rigidbody.AddForce(-Vector3.up * grav, ForceMode.Acceleration);
        }

    }

}