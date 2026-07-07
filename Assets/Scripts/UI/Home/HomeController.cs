using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CardGame.UI
{
    /// <summary>
    /// Màn Home (menu chính). Gắn lên object gốc của Canvas Home.
    /// 2 nút:
    ///  - Deck Builder -> load scene DeckBuilder.
    ///  - Play         -> load scene Duel (màn chơi có bàn 8x8, xem BoardGrid).
    /// </summary>
    public class HomeController : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button deckBuilderButton;
        [SerializeField] private Button playButton;

        [Header("Scenes (phải có trong Build Settings, xem README_Home.md)")]
        [SerializeField] private string deckBuilderSceneName = SceneNames.DeckBuilder;
        [SerializeField] private string duelSceneName = SceneNames.Duel;

        private void Awake()
        {
            if (deckBuilderButton != null)
                deckBuilderButton.onClick.AddListener(OpenDeckBuilder);
            if (playButton != null)
                playButton.onClick.AddListener(Play);
        }

        private void OnDestroy()
        {
            if (deckBuilderButton != null)
                deckBuilderButton.onClick.RemoveListener(OpenDeckBuilder);
            if (playButton != null)
                playButton.onClick.RemoveListener(Play);
        }

        /// <summary>Mở canvas Deck Builder (load scene riêng).</summary>
        public void OpenDeckBuilder()
        {
            if (string.IsNullOrEmpty(deckBuilderSceneName))
            {
                Debug.LogWarning("[Home] Chưa gán deckBuilderSceneName.");
                return;
            }
            SceneManager.LoadScene(deckBuilderSceneName);
        }

        /// <summary>Nhấn Play -> vào trận, BoardGrid trong scene Duel tự sinh bàn 8x8 lúc Start.</summary>
        public void Play()
        {
            if (string.IsNullOrEmpty(duelSceneName))
            {
                Debug.LogWarning("[Home] Chưa gán duelSceneName.");
                return;
            }
            SceneManager.LoadScene(duelSceneName);
        }
    }
}
