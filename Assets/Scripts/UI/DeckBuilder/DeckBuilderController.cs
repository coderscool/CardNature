using UnityEngine;
using UnityEngine.Events;

namespace CardGame.UI
{
    /// <summary>
    /// Bộ điều phối màn deck-builder. Nối:
    ///  - CardCollectionView (scroll thẻ hiện có, phải-trên)
    ///  - DeckSectionView side deck (Monster/Trap/Spell, phải-dưới)
    ///  - DeckSectionView nature deck (Field, phải-dưới)
    ///  - CardPreviewPanel (trái: thẻ đang chọn + Export/Import/Back)
    ///
    /// Luồng:
    ///  - Click thẻ trong collection  -> preview + thêm vào deck đúng loại.
    ///  - Click thẻ trong ngăn deck   -> preview.
    ///  - Nút xóa trên thẻ trong deck  -> gỡ khỏi deck.
    /// </summary>
    public class DeckBuilderController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private CardDatabase database;

        [Header("Views")]
        [SerializeField] private CardCollectionView collectionView;
        [SerializeField] private DeckSectionView sideDeckSection;   // Monster/Trap/Spell
        [SerializeField] private DeckSectionView natureDeckSection; // Field
        [SerializeField] private CardPreviewPanel previewPanel;

        /// <summary>Bắn khi nhấn Back (nối tới điều hướng scene/menu của bạn).</summary>
        public UnityEvent onBack;

        private readonly DeckModel _deck = new DeckModel();

        private void Awake()
        {
            // Collection
            if (collectionView != null)
                collectionView.CardSelected += HandleCollectionCardSelected;

            // Side deck
            if (sideDeckSection != null)
            {
                sideDeckSection.CardSelected += previewPanel.Show;
                sideDeckSection.CardRemoveRequested += HandleRemove;
            }

            // Nature deck
            if (natureDeckSection != null)
            {
                natureDeckSection.CardSelected += previewPanel.Show;
                natureDeckSection.CardRemoveRequested += HandleRemove;
            }

            // Preview buttons
            if (previewPanel != null)
            {
                previewPanel.ExportClicked += HandleExport;
                previewPanel.ImportClicked += HandleImport;
                previewPanel.BackClicked += HandleBack;
            }

            _deck.Changed += RefreshDeckViews;
        }

        private void Start()
        {
            if (database != null && collectionView != null)
                collectionView.Populate(database.All);

            RefreshDeckViews();
        }

        // ---- Collection ----

        private void HandleCollectionCardSelected(CardData card)
        {
            previewPanel.Show(card);
            _deck.AddCard(card); // tự phân loại Field -> nature, còn lại -> side
        }

        // ---- Deck sections ----

        private void HandleRemove(CardData card) => _deck.RemoveCard(card);

        private void RefreshDeckViews()
        {
            if (sideDeckSection != null)
                sideDeckSection.Refresh(_deck.MainCards, _deck.maxMainCards);
            if (natureDeckSection != null)
                natureDeckSection.Refresh(_deck.FieldCards, _deck.maxFieldCards);
        }

        // ---- Buttons ----

        private void HandleExport()
        {
            var data = _deck.ToData();
            string path = DeckSerializer.SaveToFile(data);
            Debug.Log($"[DeckBuilder] Exported deck to: {path}");
        }

        private void HandleImport()
        {
            string path = DeckSerializer.GetDefaultPath(_deck.deckName);
            var data = DeckSerializer.LoadFromFile(path);
            if (data == null)
            {
                Debug.LogWarning($"[DeckBuilder] No deck file at: {path}");
                return;
            }
            _deck.LoadFrom(data, database);
            Debug.Log($"[DeckBuilder] Imported deck from: {path}");
        }

        private void HandleBack()
        {
            onBack?.Invoke();
        }

        private void OnDestroy()
        {
            if (collectionView != null)
                collectionView.CardSelected -= HandleCollectionCardSelected;
            if (sideDeckSection != null)
            {
                sideDeckSection.CardSelected -= previewPanel.Show;
                sideDeckSection.CardRemoveRequested -= HandleRemove;
            }
            if (natureDeckSection != null)
            {
                natureDeckSection.CardSelected -= previewPanel.Show;
                natureDeckSection.CardRemoveRequested -= HandleRemove;
            }
            if (previewPanel != null)
            {
                previewPanel.ExportClicked -= HandleExport;
                previewPanel.ImportClicked -= HandleImport;
                previewPanel.BackClicked -= HandleBack;
            }
            _deck.Changed -= RefreshDeckViews;
        }
    }
}
