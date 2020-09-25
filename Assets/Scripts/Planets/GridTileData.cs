using System;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    internal class GridTileData
    {
        [Serializable]
        public class TileData
        {
            public byte height;
            public BiomeType type;
        }
        public enum BiomeType : byte
        {
            ocean = 0,
            lake,
            beach,
            grass,
            montain,
        }

        int width;
        int height;
        internal TileData[,] map;

        public GridTileData(int width, int height)
        {
            map = new TileData[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = new TileData();
                }
            }
            this.width = width;
            this.height = height;
        }

        public void MergeGrid(GridTileData grid, int insertX = 0, int insertY = 0)
        {
            for(int x = 0; x < grid.width; x++)
            {
                for(int y = 0; y < grid.height; y++)
                {
                    map[x, y] = grid.map[x, y];
                }
            }
        }

        public byte GetHeight(int coordinateX, int coordinateY)
        {
            if (coordinateX >= width || coordinateX < 0 || coordinateY >= height || coordinateY < 0)
                return 0;
            return map[coordinateX, coordinateY].height;
        }
        /*
        private List<PixelData> GetPixels(int coordinateX, int coordinateY, int radius)
        {
            List<PixelData> resultingPixels = new List<PixelData>();

            for (int x = -radius; x < radius; x++)
            {
                for (int y = -radius; y < radius; y++)
                {
                    Vector2Int resultCoordinates = new Vector2Int(Modulus(coordinateX + x, width), Modulus(coordinateY + y, height));
                    if (Mathf.Abs((new Vector2Int(coordinateX + x, coordinateY + y) - new Vector2Int(coordinateX, coordinateY)).magnitude) <= radius)
                    {
                        resultingPixels.Add(map[resultCoordinates.x, resultCoordinates.y]);
                    }
                }
            }
            return resultingPixels;
        }
        public void SetHeights(byte value, int radius, int coordinateX, int coordinateY)
        {
            var pixelsToChange = GetPixels(coordinateX, coordinateY, radius);
            foreach (var pixel in pixelsToChange)
            {
                pixel.height = value;
            }
        }
        public List<PixelData> MoveHeights(int increment, int radius, int coordinateX, int coordinateY)
        {
            var pixelsToChange = GetPixels(coordinateX, coordinateY, radius);
            foreach (var pixel in pixelsToChange)
            {
                if (increment > 0)
                    pixel.height += (byte)increment;
                else
                    pixel.height -= (byte)increment;
            }
            return pixelsToChange;
        }

        public int Modulus(int value, int top)
        {
            int result = value % top;
            if ((result < 0 && top > 0) || (result > 0 && top < 0))
            {
                result += top;
            }
            return result;
        }*/
    }
}

