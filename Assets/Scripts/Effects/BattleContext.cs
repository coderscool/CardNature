namespace CardGame
{
    /// <summary>
    /// Thông tin ngữ cảnh khi một hiệu ứng chạy: ai dùng thẻ, nhắm vào lá nào.
    /// Bạn mở rộng thêm field tùy game (player hiện tại, đối thủ, sân bài...).
    /// </summary>
    public class BattleContext
    {
        public Player source;   // người kích hoạt
        public Player opponent; // đối thủ
        public CardInstance sourceCard; // lá bài phát ra hiệu ứng
        public CardInstance target;     // lá bài mục tiêu (có thể null)

        public BattleContext(Player source, Player opponent)
        {
            this.source = source;
            this.opponent = opponent;
        }
    }
}
