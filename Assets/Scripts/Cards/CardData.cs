using UnityEngine;

namespace CardGame
{
    /// <summary>
    /// Loại thẻ trong game.
    /// </summary>
    public enum CardType
    {
        Monster,
        Spell,
        Trap,
        Field
    }

    /// <summary>
    /// Base cho mọi thẻ. Đây là DATA GỐC (dùng chung, không đổi khi chơi).
    /// Không sửa trực tiếp field của ScriptableObject lúc trong trận -
    /// dùng CardInstance để giữ trạng thái riêng của từng lá.
    /// </summary>
    public abstract class CardData : ScriptableObject
    {
        [Header("Chung")]
        public string id;
        public string cardName;
        public CardType type;

        [TextArea]
        public string description;

        public Sprite artwork;
    }
}
