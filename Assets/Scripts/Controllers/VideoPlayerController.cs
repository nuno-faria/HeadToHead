using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour {

    public VideoPlayer player;
    public string filename;


    void Start() {
        player.url = System.IO.Path.Combine(Application.streamingAssetsPath, filename);
        player.Play();
    }
}
