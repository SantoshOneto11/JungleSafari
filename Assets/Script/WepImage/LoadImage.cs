using SantLog;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace LoadWebP
{
    public class LoadImage : MonoBehaviour
    {
        public Image wepImage;
        string url = "https://static.vecteezy.com/system/resources/previews/021/774/593/non_2x/beautifull-pink-flowers-with-green-leaves-free-png.png";
        // Start is called before the first frame update
        void Start()
        {
            Assert.IsNotNull(url);
            StartCoroutine(DownloadImage(url));
            MyLogs.Log("This is a short log message");
            MyLogs.LogWarning("This is a warning without extra details");
            MyLogs.LogError("This is an error log");
        }

        IEnumerator DownloadImage(string url)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            DownloadHandler handler = webRequest.downloadHandler;

            yield return webRequest.SendWebRequest();

            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.Log("Error While Receving " + webRequest.error);
            }
            else
            {
                Texture2D texture = new Texture2D(8, 8);
                Sprite sprite = null;
                if (texture.LoadImage(handler.data))
                {
                    sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    Debug.Log("Setting Image");
                }

                if (sprite != null)
                {
                    wepImage.sprite = sprite;
                    Debug.Log("Image Updated!");
                }
            }
        }
    }
}
