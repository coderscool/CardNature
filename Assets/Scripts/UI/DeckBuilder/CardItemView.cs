using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.UI
{
    /// <summary>
    /// Một ô thẻ hiển thị (dùng chung cho scroll collection và các ngăn deck).
    /// Gắn script này lên prefab thẻ.
    /// </summary>
    public class CardItemView : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Image artwork;
        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private TMP_Text countLabel; // "x5" - số lượng thẻ trùng
        [SerializeField] private Button selectButton;
        [SerializeField] private Button removeButton; // ẩn khi không cần

        /// <summary>Click chọn thẻ (để preview / thêm vào deck).</summary>
        public event Action<CardItemView> Selected;

        /// <summary>Click nút xóa (chỉ khi ở trong deck).</summary>
        public event Action<CardItemView> RemoveRequested;

        public CardData Card { get; private set; }

        private void Awake()
        {
            if (selectButton != null)
                selectButton.onClick.AddListener(() => Selected?.Invoke(this));
            if (removeButton != null)
                removeButton.onClick.AddListener(() => RemoveRequested?.Invoke(this));
        }

        /// <summary>
        /// Gán dữ liệu thẻ. removable = true để hiện nút xóa.
        /// count = số lượng thẻ trùng; hiện "x{count}" khi &gt; 1.
        /// </summary>
        public void Bind(CardData card, bool removable = false, int count = 1)
        {
            Card = card;

            if (nameLabel != null)
                nameLabel.text = card != null ? card.cardName : string.Empty;

            if (artwork != null)
            {
                artwork.sprite = card != null ? card.artwork : null;
                artwork.enabled = artwork.sprite != null;
            }

            if (countLabel != null)
            {
                bool show = count > 1;
                countLabel.text = show ? $"x{count}" : string.Empty;
                countLabel.enabled = show;
            }

            if (removeButton != null)
                removeButton.gameObject.SetActive(removable);
        }
    }
}
