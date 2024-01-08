using UnityEngine;

namespace DungeonBattle
{
    public class InputManager : MonoBehaviour
    {
        private Vector2 touchStart;
        private Vector2 touchEnd;

        void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    touchStart = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    touchEnd = touch.position;
                    Vector2 direction = touchEnd - touchStart;
                    DetermineDirection(direction);
                }
            }
        }

        void DetermineDirection(Vector2 direction)
        {
            if (Vector2.Distance(touchStart, touchEnd) < 50) // 최소 드래그 거리 설정
            {
                Debug.Log("Too short drag, not considered as a direction");
                return;
            }

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;

            if (angle < 22.5 || angle > 337.5)
                Debug.Log("Right");
            else if (angle < 67.5)
                Debug.Log("Up-Right");
            else if (angle < 112.5)
                Debug.Log("Up");
            else if (angle < 157.5)
                Debug.Log("Up-Left");
            else if (angle < 202.5)
                Debug.Log("Left");
            else if (angle < 247.5)
                Debug.Log("Down-Left");
            else if (angle < 292.5)
                Debug.Log("Down");
            else
                Debug.Log("Down-Right");
        }
    }
}