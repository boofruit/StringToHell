using UnityEngine;
using System.IO;
using TMPro; // TextMeshProを使用する場合

namespace StringToHell.InGame.Ui
{
    public class TextReaderSample : MonoBehaviour
    {
        // Unityエディタからテキストファイルを指定するための変数
        public TextAsset textFile;

        // 表示先のUI（TextMeshPro）
        public TextMeshProUGUI displayText;

        void Start()
        {
            if (textFile != null)
            {
                // StringReaderを使ってテキストデータを1行ずつ読み込む
                using (StringReader reader = new StringReader(textFile.text))
                {
                    string line;

                    // ファイルの終端（-1）になるまで繰り返す
                    while ((line = reader.ReadLine()) != null)
                    {
                        // 読み込んだ1行を表示する
                        Debug.Log(line);

                        if (displayText != null)
                        {
                            displayText.text += line + "\n";
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("テキストファイルがセットされていません。");
            }
        }
    }
}
