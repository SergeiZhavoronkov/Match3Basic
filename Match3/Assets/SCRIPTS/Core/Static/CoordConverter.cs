using UnityEngine;

namespace Match3
{
    public static class CoordConverter
    {
        private const float _divider = -2f;
        private const float _offset = 0.5f;

        public static Vector3 ConvertFromIntToVector
            (this (int x, int y) intPosition, int gridSizeX, int gridSizeY)
        {
            GetOffset(gridSizeX, gridSizeY, out var offset);

            return 
                new Vector3(intPosition.x + offset.x, intPosition.y + offset.y);
        }

        public static (int, int) ConvertFromVectorToInt(
            this Vector3 position, int gridSizeX, int gridSizeY)
        {
            GetOffset(gridSizeX, gridSizeY, out var offset);

            return 
                (Mathf.RoundToInt(position.x) - offset.x, 
                Mathf.RoundToInt(position.y) - offset.y);
        }

        public static Vector3 ConvertFromIntToVector
            (this (int x, int y) intPosition, (int x, int y) gridSize)
        {
            GetOffset(gridSize.x, gridSize.y, out var offset);

            return
                new Vector3(intPosition.x + offset.x, intPosition.y + offset.y);
        }

        public static (int, int) ConvertFromVectorToInt(
            this Vector3 position, (int x, int y) gridSize)
        {
            GetOffset(gridSize.x, gridSize.y, out var offset);

            return
                (Mathf.RoundToInt(position.x - offset.x),
                Mathf.RoundToInt(position.y - offset.y));
        }

        private static void GetOffset(int gridSizeX, int gridSizeY, 
            out (int x, int y) offset)
        {
           offset = 
                (Mathf.RoundToInt(gridSizeX / _divider + _offset), 
                Mathf.RoundToInt(gridSizeY / _divider + _offset));
        }
    }
}
