namespace Spacy
{
    public static class WindowSizeExtensions
    {
        // The sizes for calculating necessary screen size is guessed/estimated numbers with some extra buffer to ensure there is some air between tiles and screen border

        private const int TileWidth = 350; // Estimate number, any update to a tile's size will have to be changed here also
        private const int TileHeight = 150; // Estimate number, any update to a tile's size will have to be changed here also
        private const int MarginBuffer = 10; // Estimate number, any update to a tile's size will have to be changed here also

        public static WindowSize ForTilesHorizontally(this WindowSize windowSize, int horizontalTiles)
        {
            windowSize.Width = TileWidth*horizontalTiles + MarginBuffer;
            return windowSize;
        }

        public static WindowSize ForTilesVertically(this WindowSize windowSize, int verticalTiles)
        {
            var overallStatusTileHeight = TileHeight;
            windowSize.Height = TileHeight * verticalTiles + overallStatusTileHeight + MarginBuffer;
            return windowSize;
        }
    }
}