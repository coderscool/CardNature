using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardGame.UI
{
    /// <summary>
    /// Ô UI của thẻ Field trên bàn (giống Field Zone trong Yu-Gi-Oh).
    /// Thả 1 CardItemView đang chứa FieldCard vào đây -> ô hiện
    /// <see cref="FieldCard.fieldSprite"/> của lá đó. Thả nhầm loại thẻ khác
    /// thì bị từ chối, thẻ tự bay về tay (xem CardItemView.OnEndDrag).
    /// </summary>
    public class FieldSlotView : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Refs")]
        [SerializeField] private Image slotImage;

        [Header("Màu viền khi kéo thẻ ngang qua (tùy chọn)")]
        [SerializeField] private Image highlightBorder;
        [SerializeField] private Color validColor = Color.green;
        [SerializeField] private Color invalidColor = Color.red;

        /// <summary>Bắn khi 1 FieldCard được thả thành công vào ô này.</summary>
        public event Action<FieldCard> FieldPlaced;

        public FieldCard CurrentField { get; private set; }

        private void Awake()
        {
            if (slotImage != null)
                slotImage.enabled = false;
            if (highlightBorder != null)
                highlightBorder.gameObject.SetActive(false);
        }

        /// <summary>Gán thẳng field đang active (dùng khi không qua kéo-thả, vd load lại trận).</summary>
        public void SetField(FieldCard card)
        {
            CurrentField = card;
            if (slotImage == null) return;

            slotImage.sprite = card != null ? card.fieldSprite : null;
            slotImage.enabled = slotImage.sprite != null;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (highlightBorder != null)
                highlightBorder.gameObject.SetActive(false);

            CardItemView item = ExtractDraggedCard(eventData);
            if (item == null || !(item.Card is FieldCard fieldCard))
                return; // sai loại thẻ / không có gì -> CardItemView tự bay về tay

            SetField(fieldCard);
            FieldPlaced?.Invoke(fieldCard);

            // "Nhận" thẻ: đổi parent để CardItemView.OnEndDrag biết đã được nhận
            // (không tự bay về tay nữa), sau đó xóa item UI khỏi tay vì đã lên sân.
            item.transform.SetParent(transform, false);
            Destroy(item.gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (highlightBorder == null) return;
            CardItemView item = ExtractDraggedCard(eventData);
            if (item == null) return; // không đang kéo gì cả thì thôi

            bool valid = item.Card is FieldCard;
            highlightBorder.color = valid ? validColor : invalidColor;
            highlightBorder.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (highlightBorder != null)
                highlightBorder.gameObject.SetActive(false);
        }

        private static CardItemView ExtractDraggedCard(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return null;
            return eventData.pointerDrag.GetComponent<CardItemView>();
        }
    }
}
