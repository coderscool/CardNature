using UnityEngine;

namespace CardGame
{
    public enum Environment
    {
        Neutral,
        Forest,
        Volcano,
        Ocean,
        Frozen,
        Pateau,
        Desert
    }

    /// <summary>
    /// Thẻ môi trường. Bạn muốn tối giản (chỉ id + loại) thì bỏ 'effect' đi.
    /// Nhưng nên giữ effect để môi trường thực sự buff/debuff, nếu không nó không có tác dụng gameplay.
    /// </summary>
    [CreateAssetMenu(fileName = "Field", menuName = "Cards/Field")]
    public class FieldCard : CardData
    {
        [Header("Field")]
        public Environment environment = Environment.Neutral;

        /// <summary>Hiệu ứng nền áp lên cả sân (vd: tăng ATK quái cùng hệ). Có thể null.</summary>
        public CardEffect effect;

        /// <summary>
        /// Sprite hiện trong Ô FIELD trên bàn khi lá này đang được kích hoạt
        /// (khác với 'artwork' - hình lá bài trên tay/collection). Xem FieldSlotView.
        /// </summary>
        public Sprite fieldSprite;

        private void OnEnable()
        {
            type = CardType.Field;
        }
    }
}
