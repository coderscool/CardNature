using System;
using UnityEngine;

namespace CardGame.Board
{
    /// <summary>
    /// Sinh bàn chơi dạng lưới (mặc định 8x8) lúc vào scene Duel.
    /// Gắn lên 1 object rỗng trong scene, gán <see cref="cellPrefab"/> rồi chạy.
    /// </summary>
    public class BoardGrid : MonoBehaviour
    {
        [Header("Kích thước bàn")]
        [SerializeField] private int rows = 8;
        [SerializeField] private int columns = 8;

        [Header("Prefab & layout")]
        [SerializeField] private BoardCell cellPrefab;
        [Tooltip("Để trống thì các ô sẽ làm con của chính object này.")]
        [SerializeField] private Transform cellsParent;
        [SerializeField] private Vector2 cellSize = new Vector2(1f, 1f);
        [SerializeField] private Vector2 cellSpacing = new Vector2(0.1f, 0.1f);
        [SerializeField] private bool generateOnStart = true;

        public int Rows => rows;
        public int Columns => columns;

        /// <summary>Bắn khi bất kỳ ô nào trong bàn được click, kèm chính ô đó.</summary>
        public event Action<BoardCell> CellClicked;

        private BoardCell[,] _cells;

        private void Start()
        {
            if (generateOnStart)
                Generate();
        }

        /// <summary>Sinh lại toàn bộ bàn (xóa bàn cũ nếu có). Gọi tay từ code hoặc menu chuột phải.</summary>
        [ContextMenu("Generate")]
        public void Generate()
        {
            if (cellPrefab == null)
            {
                Debug.LogError("[BoardGrid] Chưa gán cellPrefab.");
                return;
            }

            Clear();

            Transform parent = cellsParent != null ? cellsParent : transform;
            _cells = new BoardCell[rows, columns];

            float stepX = cellSize.x + cellSpacing.x;
            float stepY = cellSize.y + cellSpacing.y;

            // Căn giữa bàn quanh gốc tọa độ của parent.
            float originX = -((columns - 1) * stepX) / 2f;
            float originY = -((rows - 1) * stepY) / 2f;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    BoardCell cell = Instantiate(cellPrefab, parent);
                    cell.Init(r, c);
                    cell.transform.localPosition = new Vector3(originX + c * stepX, originY + r * stepY, 0f);
                    cell.Clicked += HandleCellClicked;

                    _cells[r, c] = cell;
                }
            }
        }

        /// <summary>Xóa toàn bộ ô đang có trên bàn.</summary>
        public void Clear()
        {
            if (_cells == null) return;
            foreach (BoardCell cell in _cells)
            {
                if (cell == null) continue;
                cell.Clicked -= HandleCellClicked;
                Destroy(cell.gameObject);
            }
            _cells = null;
        }

        public BoardCell GetCell(int row, int column)
        {
            if (_cells == null || row < 0 || row >= rows || column < 0 || column >= columns)
                return null;
            return _cells[row, column];
        }

        private void HandleCellClicked(BoardCell cell) => CellClicked?.Invoke(cell);

        private void OnDestroy() => Clear();
    }
}
