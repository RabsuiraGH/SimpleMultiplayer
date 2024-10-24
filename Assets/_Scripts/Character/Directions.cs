using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public static class Directions
    {
        public enum MainDirection
        {
            Left,
            Right
        }

        public enum SecondaryDirection
        {
            Up,
            Down
        }

        /// <param name="coordinate">Zero coordinates </param>
        /// <returns>Mouse world position</returns>
        public static Vector2 GetDirectionsViaMouse(Camera camera, Vector2 coordinate, out MainDirection mainDir, out SecondaryDirection secondaryDir)
        {
            Vector2 mousePosition = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());


            if (mousePosition.x > coordinate.x)
            {
                mainDir = MainDirection.Right;
            }
            else
            {
                mainDir = MainDirection.Left;
            }

            if (mousePosition.y > coordinate.y)
            {
                secondaryDir = SecondaryDirection.Up;
            }
            else
            {
                secondaryDir = SecondaryDirection.Down;
            }

            return mousePosition;
        }

    }
}