namespace CardGame
{
    /// <summary>
    /// Tên các scene dùng chung, tránh hard-code string rải rác nhiều nơi.
    /// Tên ở đây PHẢI khớp tên file scene đã thêm vào Build Settings
    /// (File → Build Profiles → Scene List), xem README_Home.md.
    /// </summary>
    public static class SceneNames
    {
        public const string Home = "Home";
        public const string DeckBuilder = "DeckBuilder";
        public const string Duel = "Duel";
    }
}
