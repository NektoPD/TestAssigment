using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private ControlScheme _controlScheme;
        private IEnumerator _clickDetectionCoroutine;

        public Vector2 CurrentPointerPosition { get; private set; }

        public event Action Clicked;

        private void Awake()
        {
            _controlScheme = new ControlScheme();
        }

        private void OnEnable()
        {
            _controlScheme.Player.Enable();
            _controlScheme.Player.Press.performed += OnClickPerformed;
        }

        private void OnDisable()
        {
            _controlScheme.Player.Press.performed -= OnClickPerformed;
        }

        public void EnablePointerDetection()
        {
            _clickDetectionCoroutine = PointerPositionDetectionCoroutine();
            StartCoroutine(_clickDetectionCoroutine);
        }

        public void DisablePointerDetection()
        {
            StopCoroutine(_clickDetectionCoroutine);
            _clickDetectionCoroutine = null;
        }

        private void OnClickPerformed(InputAction.CallbackContext context)
        {
            Clicked?.Invoke();
        }

        private IEnumerator PointerPositionDetectionCoroutine()
        {
            while (true)
            {
                CurrentPointerPosition = _controlScheme.Player.PointerPosition.ReadValue<Vector2>();

                yield return null;
            }
        }
    }
}