using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Yarn.Unity;

[System.Serializable]
public class CharacterExpression
{
    public string expressionName;
    public Sprite sprite;
}

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public List<CharacterExpression> expressions;

    public Sprite GetSprite(string expressionName)
    {
        var match = expressions.Find(e => e.expressionName == expressionName);
        if (match == null)
        {
            Debug.LogWarning($"No expression '{expressionName}' found for {characterName}");
            return null;
        }
        return match.sprite;
    }
}

[System.Serializable]
public class BackgroundData
{
    public string backgroundName; 
    public Sprite sprite;
}

[System.Serializable]
public class DateCutsceneData
{
    public string cutsceneName;   
    public Sprite background;    
    public string dateLabel;     
}

[System.Serializable]
public class BGMData
{
    public string bgmName;   
    public AudioClip clip;
}

[System.Serializable]
public class SFXData
{
    public string sfxName;   
    public AudioClip clip;
}

public class VNController : MonoBehaviour
{
    public static VNController Instance;

    public Image background;
    public Image characterSlot;

    public List<CharacterData> characters;
     public List<BackgroundData> backgrounds;
     public List<DateCutsceneData> dateCutscenes;
    
    public RectTransform dateWipePanel;  
    public CanvasGroup dateTextGroup;     
    public TextMeshProUGUI dateText;

    public List<BGMData> bgmTracks;
    public List<SFXData> sfxClips;

    public AudioSource bgmSource;
    public AudioSource sfxSource; 


    public float screenWidth = 1920f;

     void Awake()
    {
        Instance = this;
    }

    static float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);
    static float EaseInCubic(float t) => t * t * t;
   

    [YarnCommand("show_date_cutscene")]
    public static async Task ShowDateCutscene(string cutsceneName, float wipeDuration = 0.5f, float holdSeconds = 2.1f)
    {
        var data = Instance.dateCutscenes.Find(c => c.cutsceneName == cutsceneName);
        if (data == null)
        {
            Debug.LogWarning($"No date cutscene named '{cutsceneName}' found");
            return;
        }

        var panel = Instance.dateWipePanel;
        var textGroup = Instance.dateTextGroup;

        Instance.dateText.text = data.dateLabel;
        textGroup.alpha = 0f;

       
        for (float t = 0; t < wipeDuration; t += Time.deltaTime)
        {
            panel.sizeDelta = new Vector2(Instance.screenWidth * EaseOutCubic(t / wipeDuration), panel.sizeDelta.y);
            await Task.Yield();
        }
        panel.sizeDelta = new Vector2(Instance.screenWidth, panel.sizeDelta.y);

        
        Instance.background.sprite = data.background;
        Instance.characterSlot.enabled = false; 

        

        for (float t = 0; t < 0.3f; t += Time.deltaTime)
        {
            textGroup.alpha = EaseOutCubic(t / 0.3f);
            await Task.Yield();

           /* panel.sizeDelta = new Vector2(targetWidth * EaseOutCubic(t / wipeDuration), panel.sizeDelta.y);
            await Task.Yield();*/
        }
        

        textGroup.alpha = 1f;
        

        await Task.Delay((int)(holdSeconds * 1000));

        
        for (float t = 0; t < 0.3f; t += Time.deltaTime)
        {
            textGroup.alpha = 1f - EaseInCubic(t / 0.3f);
            await Task.Yield();
        }
        textGroup.alpha = 0f;

       
        for (float t = 0; t < wipeDuration; t += Time.deltaTime)
        {
            panel.sizeDelta = new Vector2(Instance.screenWidth * (1f - EaseInCubic(t / wipeDuration)), panel.sizeDelta.y);
            await Task.Yield();
        }
        panel.sizeDelta = new Vector2(0f, panel.sizeDelta.y);
    }



    [YarnCommand("vn_show_sprite")]
    public static void ShowSprite(string character, string expression)
    {
        var data = Instance.characters.Find(c => c.characterName == character);
        if (data == null)
        {
            Debug.LogWarning($"No character named '{character}' found");
            return;
        }

        Sprite sprite = data.GetSprite(expression);
        if (sprite != null)
            Instance.characterSlot.sprite = sprite;
            Instance.characterSlot.enabled = true;
    }

[YarnCommand("vn_hide_sprite")]
public static void HideSprite()
{
    Instance.characterSlot.enabled = false;
}



[YarnCommand("vn_set_background")]
    public static void SetBackground(string backgroundName)
    {
        var match = Instance.backgrounds.Find(b => b.backgroundName == backgroundName);
        if (match == null)
        {
            Debug.LogWarning($"No background named '{backgroundName}' found");
            return;
        }

        Instance.background.sprite = match.sprite;
    }

    [YarnCommand("play_bgm")]
public static async Task PlayBGM(string bgmName, float fadeSeconds = 1f, bool loop = true)
{
    var data = Instance.bgmTracks.Find(b => b.bgmName == bgmName);
    if (data == null) { Debug.LogWarning($"No BGM named '{bgmName}' found"); return; }

    var source = Instance.bgmSource;

    
    float startVolume = source.volume;
    for (float t = 0; t < fadeSeconds; t += Time.deltaTime)
    {
        source.volume = Mathf.Lerp(startVolume, 0f, t / fadeSeconds);
        await Task.Yield();
    }

    
    source.clip = data.clip;
    source.loop = loop;
    source.Play();

    
    for (float t = 0; t < fadeSeconds; t += Time.deltaTime)
    {
        source.volume = Mathf.Lerp(0f, startVolume, t / fadeSeconds);
        await Task.Yield();
    }
    source.volume = startVolume;
}

    [YarnCommand("stop_bgm")]
    public static void StopBGM()
    {
        Instance.bgmSource.Stop();
    }

    [YarnCommand("play_sfx")]
    public static void PlaySFX(string sfxName)
    {
        var data = Instance.sfxClips.Find(s => s.sfxName == sfxName);
        if (data == null)
        {
            Debug.LogWarning($"No SFX named '{sfxName}' found");
            return;
        }

        Instance.sfxSource.PlayOneShot(data.clip);
    }

}





