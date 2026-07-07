using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardGame.UI
{
    /// <summary>
    /// Một ô thẻ hiển thị (dùng chung cho scroll collection, các ngăn deck,
    /// và tay bài trong trận). Gắn script này lên prefab thẻ.
    ///
    /// Kéo-thả: bật <see cref="draggable"/> (mặc định TẮT, không ảnh hưởng
    /// Deck Builder) để cho phép kéo thẻ này thả vào 1 ô nhận (vd FieldSlotView).
    /// Nếu không được ô nào "nhận" (đổi parent), thẻ tự bay về vị trí cũ.
    /// </summary>
    public class CardItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Refs")]
        [SerializeField] private Image artwork;
        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private TMP_Text countLabel; // "x5" - số lượng thẻ trùng
        [SerializeField] private Button selectButton;
        [SerializeField] private Button removeButton; // ẩn khi không cần

        [Header("Kéo-thả (tùy chọn, dùng ở tay bài trong trận)")]
        [SerializeField] private bool draggable = false;

        private RectTransform _rect;
        private CanvasGroup _canvasGroup;
        private Canvas _rootCanvas;
        private Transform _originalParent;
        private int _originalSiblingIndex;
        private Vector2 _originalAnchoredPos;

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

            _rect = transform as RectTransform;
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
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

        // ---- Kéo-thả (chỉ hoạt động khi draggable = true) ----

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!draggable || _rect == null) return;

            _originalParent = transform.parent;
            _originalSiblingIndex = transform.GetSiblingIndex();
            _originalAnchoredPos = _rect.anchoredPosition;

            _rootCanvas = GetComponentInParent<Canvas>();
            if (_rootCanvas != null)
                _rootCanvas = _rootCanvas.rootCanvas;

            // Chuyển lên canvas gốc để thẻ vẽ đè lên mọi UI khác lúc kéo.
            if (_rootCanvas != null)
                transform.SetParent(_rootCanvas.transform, true);
            transform.SetAsLastSibling();

            _canvasGroup.blocksRaycasts = false; // để raycast xuyên qua, tới ô nhận bên dưới
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!draggable || _rect == null) return;
            _rect.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!draggable || _rect == null) return;

            _canvasGroup.blocksRaycasts = true;

            // Nếu chưa ô nào "nhận" thẻ (đổi parent trong OnDrop của ô đó),
            // thẻ vẫn còn nằm ở canvas gốc -> tự bay về vị trí/parent cũ.
            if (_rootCanvas != null && transform.parent == _rootCanvas.transform)
            {
                transform.SetParent(_originalParent, false);
                transform.SetSiblingIndex(_originalSiblingIndex);
                _rect.anchoredPosition = _originalAnchoredPos;
            }
        }
    }
}
