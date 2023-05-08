using Enums;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace Controllers
{
    public class SwipeManager : MonoBehaviour
    {
        public event OnTapDelegate OnTap;
        public event OnSwipeLeftDelegate OnSwipeLeft;
        public event OnSwipeRightDelegate OnSwipeRight;
        public event OnSwipeUpDelegate OnSwipeUp;
        public event OnSwipeDownDelegate OnSwipeDown;

        public delegate void OnSwipeDownDelegate();

        public delegate void OnSwipeLeftDelegate();

        public delegate void OnSwipeRightDelegate();

        public delegate void OnSwipeUpDelegate();

        public delegate void OnTapDelegate();


        public static SwipeManager Instance;
        private Vector3 delta;
        private Vector3 initPos;
        private bool isDragging;
        private Vector3 lastPos;
        private Vector2 touchStartPosition, touchEndPosition, swipeDelta;

        private void Awake()
        {
            Instance = this;
        }

        private void Reset()
        {
            touchStartPosition = swipeDelta = Vector2.zero;
            isDragging = false;
        }

        private void Update()
        {
            if (GameManager.instance.State != GameState.Playing) return;
            if (Helpers.IsCursorOverUI()) return;
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                touchStartPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                touchEndPosition = Input.mousePosition;
                InputEnded();
            }
        }


        private void InputEnded()
        {
            swipeDelta = touchEndPosition - touchStartPosition;
            if (swipeDelta.magnitude > 100)
            {
                var y = swipeDelta.y;
                var x = swipeDelta.x;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    if (x < 0)
                        OnSwipeLeft?.Invoke();
                    else
                        OnSwipeRight?.Invoke();
                }
                else
                {
                    if (y < 0)
                        OnSwipeDown?.Invoke();
                    else
                        OnSwipeUp?.Invoke();
                }
            }
            else
            {
                OnTap?.Invoke();
            }

            Reset();
        }
    }
}