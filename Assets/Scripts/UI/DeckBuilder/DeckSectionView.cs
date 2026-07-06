using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CardGame.UI
{
    /// <summary>
    /// Một ngăn deck ở phía dưới bên phải. Dùng cho cả:
    ///  - "Side Deck" (Monster/Trap/Spell)
    ///  - "Nature Deck" (Field)
    /// Hiện các thẻ đã thêm, mỗi thẻ có nút xóa.
    /// </summary>
    public class DeckSectionView : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Transform content;
        [SerializeField] private CardItemView itemPrefab;
        [SerializeField] private TMP_Text titleLabel;
        [SerializeField] private TMP_Text countLabel;

        [Header("Config")]
        [SerializeField] private string title = "Deck";

        /// <summary>Click một thẻ trong ngăn (để preview).</summary>
        public event Action<CardData> CardSelected;

        /// <summary>Yêu cầu xóa một thẻ khỏi ngăn.</summary>
        public event Action<CardData> CardRemoveRequested;

        private readonly List<CardItemView> _spawned = new List<CardItemView>();

        private void Awake()
        {
            if (titleLabel != null) titleLabel.text = title;
        }

        /// <summary>
        /// Vẽ lại ngăn theo danh sách thẻ hiện tại. Các thẻ TRÙNG được gộp
        /// thành 1 ô kèm số lượng "x{n}". countLabel hiện tổng số lá / max.
        /// </summary>
        public void Refresh(IReadOnlyList<CardData> cards, int max = -1)
        {
            Clear();

            // Gộp trùng, giữ thứ tự xuất hiện lần đầu.
            var order = new List<CardData>();
            var counts = new Dictionary<CardData, int>();
            foreach (var card in cards)
            {
                if (card == null) continue;
                if (counts.TryGetValue(card, out int n))
                {
                    counts[card] = n + 1;
                }
                else
                {
                    counts[card] = 1;
                    order.Add(card);
                }
            }

            foreach (var card in order)
            {
                var item = Instantiate(itemPrefab, content);
                item.Bind(card, removable: true, count: counts[card]);
                item.Selected += OnItemSelected;
                item.RemoveRequested += OnItemRemove;
                _spawned.Add(item);
            }

            if (countLabel != null)
                countLabel.text = max >= 0 ? $"{cards.Count}/{max}" : cards.Count.ToString();
        }

        private void OnItemSelected(CardItemView item) => CardSelected?.Invoke(item.Card);
        private void OnItemRemove(CardItemView item) => CardRemoveRequested?.Invoke(item.Card);

        private void Clear()
        {
            foreach (var item in _spawned)
            {
                if (item == null) continue;
                item.Selected -= OnItemSelected;
                item.RemoveRequested -= OnItemRemove;
                Destroy(item.gameObject);
            }
            _spawned.Clear();
        }

        private void OnDestroy() => Clear();
    }
}
