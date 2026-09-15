using UnityEngine;
using Yarn.Unity;

public class SaveManager : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    public GameObject savePanel;
    public GameObject loadPanel;

    public void OpenSavePanel()
    {
        savePanel.SetActive(true);
    }

    public void CloseSavePanel()
    {
        savePanel.SetActive(false);
    }

    public void OpenLoadPanel()
    {
        loadPanel.SetActive(true);
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

        dialogueRunner.SaveStateToPersistentStorage(slotName);

        PlayerPrefs.Save();

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
}