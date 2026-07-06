using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.UI
{
    /// <summary>
    /// Scroll view bên phải phía trên: liệt kê toàn bộ thẻ hiện có.
    /// Gắn lên object 'Content' của ScrollRect (hoặc object cha quản lý content).
    /// </summary>
    public class CardCollectionView : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Transform content;      // container trong ScrollRect
        [SerializeField] private CardItemView itemPrefab;

        /// <summary>Người dùng click một thẻ trong collection.</summary>
        public event Action<CardData> CardSelected;

        private readonly List<CardItemView> _spawned = new List<CardItemView>();

        /// <summary>Dựng lại toàn bộ danh sách thẻ.</summary>
        public void Populate(IEnumerable<CardData> cards)
        {
            Clear();
            foreach (var card in cards)
            {
                var item = Instantiate(itemPrefab, content);
                item.Bind(card, removable: false);
                item.Selected += OnItemSelected;
                _spawned.Add(item);
            }
        }

        private void OnItemSelected(CardItemView item)
        {
            CardSelected?.Invoke(item.Card);
        }

        private void Clear()
        {
            foreach (var item in _spawned)
            {
                if (item == null) continue;
                item.Selected -= OnItemSelected;
                Destroy(item.gameObject);
            }
            _spawned.Clear();
        }

        private void OnDestroy() => Clear();
    }
}
