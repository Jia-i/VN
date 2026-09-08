using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using System.Collections;

public class GameController : MonoBehaviour
{
    private static Image characterImage;
    private static Image backgroundImage;
    private static Image fadeOverlay;
    private static MonoBehaviour instance;

    public Image characterImageRef;
    public Image backgroundImageRef;
    public Image fadeOverlayRef;

    void Awake()
    {
        characterImage = characterImageRef;
        backgroundImage = backgroundImageRef;
        fadeOverlay = fadeOverlayRef;
        instance = this;
    }

    [YarnCommand("show_sprite")]
    public static void ShowSprite(string spriteName)
    {
        Sprite sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
        if (sprite != null)
        {
            characterImage.sprite = sprite;
            characterImage.enabled = true;
            Color c = characterImage.color;
            characterImage.color = new Color(c.r, c.g, c.b, 1f);
        }
    }

    [YarnCommand("show_background")]
    public static Coroutine ShowBackgroundFade(string bgName)
    {
        return instance.StartCoroutine(FadeBackgroundRoutine(bgName));
    }

    private static IEnumerator FadeBackgroundRoutine(string bgName)
    {
        // 先淡出（变黑）
        yield return Fade(0f, 1f, 0.5f);

        // 换图
        Sprite bg = Resources.Load<Sprite>("Backgrounds/" + bgName);
        if (bg != null)
        {
            backgroundImage.sprite = bg;
        }

        // 再淡入（变亮）
        yield return Fade(1f, 0f, 0.5f);
    }

    private static IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0;
        Color c = fadeOverlay.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, t / duration);
            fadeOverlay.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        fadeOverlay.color = new Color(c.r, c.g, c.b, to);
    }
}