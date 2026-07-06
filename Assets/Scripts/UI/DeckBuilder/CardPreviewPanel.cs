using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.UI
{
    /// <summary>
    /// Panel bên trái: hiển thị thẻ đang chọn + 3 nút Export / Import / Back.
    /// </summary>
    public class CardPreviewPanel : MonoBehaviour
    {
        [Header("Card display")]
        [SerializeField] private Image artwork;
        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private TMP_Text typeLabel;
        [SerializeField] private TMP_Text descriptionLabel;
        [SerializeField] private TMP_Text statsLabel;   // ATK/DEF cho quái

        [Header("Buttons")]
        [SerializeField] private Button exportButton;
        [SerializeField] private Button importButton;
        [SerializeField] private Button backButton;

        public event Action ExportClicked;
        public event Action ImportClicked;
        public event Action BackClicked;

        private void Awake()
        {
            if (exportButton != null) exportButton.onClick.AddListener(() => ExportClicked?.Invoke());
            if (importButton != null) importButton.onClick.AddListener(() => ImportClicked?.Invoke());
            if (backButton != null) backButton.onClick.AddListener(() => BackClicked?.Invoke());
            Clear();
        }

        /// <summary>Hiển thị thẻ được chọn.</summary>
        public void Show(CardData card)
        {
            if (card == null) { Clear(); return; }

            if (nameLabel != null) nameLabel.text = card.cardName;
            if (typeLabel != null) typeLabel.text = card.type.ToString();
            if (descriptionLabel != null) descriptionLabel.text = card.description;

            if (artwork != null)
            {
                artwork.sprite = card.artwork;
                artwork.enabled = card.artwork != null;
            }

            if (statsLabel != null)
            {
                if (card is MonsterCard m)
                {
                    statsLabel.enabled = true;
                    statsLabel.text = $"ATK {m.atk}   DEF {m.def}   LV {m.level}";
                }
                else
                {
                    statsLabel.text = string.Empty;
                    statsLabel.enabled = false;
                }
            }
        }

        /// <summary>Xóa hiển thị (không có thẻ nào được chọn).</summary>
        public void Clear()
        {
            if (nameLabel != null) nameLabel.text = string.Empty;
            if (typeLabel != null) typeLabel.text = string.Empty;
            if (descriptionLabel != null) descriptionLabel.text = string.Empty;
            if (statsLabel != null) { statsLabel.text = string.Empty; statsLabel.enabled = false; }
            if (artwork != null) { artwork.sprite = null; artwork.enabled = false; }
        }
    }
}
