using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    /// <summary>
    /// Kho chứa toàn bộ thẻ trong game. Tạo 1 asset duy nhất, kéo mọi thẻ vào.
    /// Dùng để tra thẻ theo id khi import deck.
    /// </summary>
    [CreateAssetMenu(fileName = "CardDatabase", menuName = "Cards/Card Database")]
    public class CardDatabase : ScriptableObject
    {
        [SerializeField] private List<CardData> cards = new List<CardData>();

        private Dictionary<string, CardData> _lookup;

        public IReadOnlyList<CardData> All => cards;

        private void BuildLookup()
        {
            _lookup = new Dictionary<string, CardData>(cards.Count);
            foreach (var c in cards)
            {
                if (c == null || string.IsNullOrEmpty(c.id)) continue;
                _lookup[c.id] = c;
            }
        }

        /// <summary>Tra thẻ theo id. Trả null nếu không có.</summary>
        public CardData GetById(string id)
        {
            if (_lookup == null) BuildLookup();
            return _lookup.TryGetValue(id, out var card) ? card : null;
        }

        /// <summary>Lọc thẻ theo loại.</summary>
        public IEnumerable<CardData> GetByType(CardType type)
        {
            foreach (var c in cards)
                if (c != null && c.type == type)
                    yield return c;
        }

#if UNITY_EDITOR
        /// <summary>Gọi trong Editor để nạp lại lookup sau khi sửa list.</summary>
        public void RefreshLookup() => BuildLookup();
#endif
    }
}
