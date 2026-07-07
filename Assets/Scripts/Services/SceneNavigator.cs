using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame
{
    /// <summary>
    /// Helper điều hướng scene dùng chung cho cả game.
    /// Gắn lên 1 object bất kỳ (vd object gốc của mỗi scene) rồi nối các hàm
    /// bên dưới vào UnityEvent trên UI (vd nút Back, hoặc onBack của
    /// DeckBuilderController) - không cần code thêm gì khác.
    /// </summary>
    public class SceneNavigator : MonoBehaviour
    {
        public void GoHome() => Load(SceneNames.Home);

        public void GoDeckBuilder() => Load(SceneNames.DeckBuilder);

        public void GoDuel() => Load(SceneNames.Duel);

        public void ReloadCurrentScene() => Load(SceneManager.GetActiveScene().name);

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private static void Load(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogWarning("[SceneNavigator] Tên scene rỗng, không load được.");
                return;
            }
            SceneManager.LoadScene(sceneName);
        }
    }
}
