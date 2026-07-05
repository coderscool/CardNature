using UnityEngine;

namespace CardGame
{
    public enum Attribute
    {
        None,
        Fire,
        Water,
        Earth,
        Wind,
        Light,
        Dark
    }

    [CreateAssetMenu(fileName = "Monster", menuName = "Cards/Monster")]
    public class MonsterCard : CardData
    {
        [Header("Monster")]
        public Attribute attribute = Attribute.None;
        [Range(1, 12)] public int level = 1;
        public int atk;
        public int def;

        /// <summary>Hiệu ứng của quái (có thể null nếu là quái thường).</summary>
        public CardEffect effect;

        private void OnEnable()
        {
            type = CardType.Monster;
        }
    }
}
