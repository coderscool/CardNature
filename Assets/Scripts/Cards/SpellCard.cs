using UnityEngine;

namespace CardGame
{
    public enum SpellSubType
    {
        Normal,
        Continuous,
        Quick,
        Equip,
        Ritual
    }

    [CreateAssetMenu(fileName = "Spell", menuName = "Cards/Spell")]
    public class SpellCard : CardData
    {
        [Header("Spell")]
        public SpellSubType subType = SpellSubType.Normal;
        public CardEffect effect;

        private void OnEnable()
        {
            type = CardType.Spell;
        }
    }
}
