using UnityEngine;
using Yarn.Unity;
using TMPro;

public class SaveManager : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    public GameObject savePanel;
    public GameObject loadPanel;

    public TMP_Text save1DateText;
    public TMP_Text save2DateText;

    public TMP_Text load1DateText;
    public TMP_Text load2DateText;

    public void OpenSavePanel()
    {
        savePanel.SetActive(true);
        UpdateSaveDates();
    }

    public void CloseSavePanel()
    {
        savePanel.SetActive(false);
    }

    public void OpenLoadPanel()
    {
        loadPanel.SetActive(true);
        UpdateSaveDates();
    }

    public void CloseLoadPanel()
    {
        loadPanel.SetActive(false);
    }

    public void SaveSlot1()
    {
        SaveGame("Save1");
    }

    public void SaveSlot2()
    {
        SaveGame("Save2");
    }

    private void SaveGame(string slotName)
    {
        string currentNode = dialogueRunner.Dialogue.CurrentNode;

        PlayerPrefs.SetString(slotName + "_Node", currentNode);

        string saveDate = System.DateTime.Now.ToString("dd/MM/yyyy\nhh:mm tt");
        PlayerPrefs.SetString(slotName + "_Date", saveDate);

        dialogueRunner.SaveStateToPersistentStorage(slotName);
        PlayerPrefs.Save();

        UpdateSaveDates();

        Debug.Log("Game Saved: " + slotName + " / " + currentNode);
    }

    public void LoadSlot1()
    {
        LoadGame("Save1");
    }

    public void LoadSlot2()
    {
        LoadGame("Save2");
    }

    private void LoadGame(string slotName)
    {
        dialogueRunner.LoadStateFromPersistentStorage(slotName);

        string currentNode = PlayerPrefs.GetString(
            slotName + "_Node",
            "Start"
        );

        dialogueRunner.StartDialogue(currentNode);

        Debug.Log("Game Loaded: " + slotName + " / " + currentNode);
    }

    private void UpdateSaveDates()
    {
        string save1Date = PlayerPrefs.GetString(
            "Save1_Date",
            "EMPTY"
        );

        string save2Date = PlayerPrefs.GetString(
            "Save2_Date",
            "EMPTY"
        );

        if (save1DateText != null)
        {
            save1DateText.text = save1Date;
        }

        if (save2DateText != null)
        {
            save2DateText.text = save2Date;
        }

        if (load1DateText != null)
        {
            load1DateText.text = save1Date;
        }

        if (load2DateText != null)
        {
            load2DateText.text = save2Date;
        }
    }
}
