using System.IO;
using UnityEngine;

namespace CardGame
{
    /// <summary>
    /// Chuyển DeckData &lt;-&gt; JSON và đọc/ghi file.
    /// Mặc định lưu vào Application.persistentDataPath.
    /// </summary>
    public static class DeckSerializer
    {
        public static string ToJson(DeckData data) => JsonUtility.ToJson(data, true);

        public static DeckData FromJson(string json) => JsonUtility.FromJson<DeckData>(json);

        /// <summary>Đường dẫn file mặc định cho một tên deck.</summary>
        public static string GetDefaultPath(string deckName)
        {
            string safe = string.IsNullOrEmpty(deckName) ? "deck" : deckName;
            foreach (var c in Path.GetInvalidFileNameChars())
                safe = safe.Replace(c, '_');
            return Path.Combine(Application.persistentDataPath, safe + ".json");
        }

        /// <summary>Ghi deck ra file. Trả đường dẫn đã ghi.</summary>
        public static string SaveToFile(DeckData data, string path = null)
        {
            path ??= GetDefaultPath(data.deckName);
            File.WriteAllText(path, ToJson(data));
            return path;
        }

        /// <summary>Đọc deck từ file. Trả null nếu không tồn tại.</summary>
        public static DeckData LoadFromFile(string path)
        {
            if (!File.Exists(path)) return null;
            return FromJson(File.ReadAllText(path));
        }
    }
}
