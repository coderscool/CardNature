using UnityEngine;
using UnityEngine.UI;
using CardGame.Board;

namespace CardGame.UI
{
    /// <summary>
    /// Bộ điều phối màn Duel (màn chơi). BoardGrid tự sinh bàn 8x8 lúc Start
    /// (xem BoardGrid.generateOnStart); script này chỉ nối nút Back và log click ô
    /// làm ví dụ - thay HandleCellClicked bằng logic đặt bài thật của bạn.
    /// </summary>
    public class DuelSceneController : MonoBehaviour
    {
        [Header("Bàn chơi")]
        [SerializeField] private BoardGrid board;

        [Header("UI")]
        [SerializeField] private Button backButton;
        [SerializeField] private SceneNavigator sceneNavigator;

        private void Awake()
        {
            if (board != null)
                board.CellClicked += HandleCellClicked;

            if (backButton != null && sceneNavigator != null)
                backButton.onClick.AddListener(sceneNavigator.GoHome);
        }

        private void OnDestroy()
        {
            if (board != null)
                board.CellClicked -= HandleCellClicked;

            if (backButton != null && sceneNavigator != null)
                backButton.onClick.RemoveListener(sceneNavigator.GoHome);
        }

        private void HandleCellClicked(BoardCell cell)
        {
            Debug.Log($"[Duel] Click ô ({cell.row}, {cell.column}) - trống: {cell.IsEmpty}");
            // TODO: nối vào logic đặt bài / chọn ô của bạn ở đây.
        }
    }
}
