using UnityEngine;

namespace CardGame
{
    public enum TrapSubType
    {
        Normal,
        Continuous,
        Counter
    }

    [CreateAssetMenu(fileName = "Trap", menuName = "Cards/Trap")]
    public class TrapCard : CardData
    {
        [Header("Trap")]
        public TrapSubType subType = TrapSubType.Normal;
        public CardEffect effect;

        private void OnEnable()
        {
            type = CardType.Trap;
        }
    }
}
