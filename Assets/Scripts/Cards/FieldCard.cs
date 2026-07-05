using UnityEngine;

namespace CardGame
{
    public enum Environment
    {
        Neutral,
        Forest,
        Volcano,
        Ocean,
        Mountain,
        Wasteland
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

        private void OnEnable()
        {
            type = CardType.Field;
        }
    }
}
