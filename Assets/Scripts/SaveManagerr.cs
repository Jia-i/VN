using UnityEngine;
using Yarn.Unity;

public class SaveManager : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    public void OnSaveButtonClicked()
    {
        dialogueRunner.SaveStateToPersistentStorage("save1");
    }

    public void OnLoadButtonClicked()
    {
    dialogueRunner.LoadStateFromPersistentStorage("save1");

    //string nodeName = dialogueRunner.VariableStorage.GetValue("$current_node").ToString();
    //dialogueRunner.StartDialogue(nodeName);
    }
}