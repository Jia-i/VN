using UnityEngine;
using TMPro;

public class HistoryManager : MonoBehaviour
{
    public GameObject historyPanel;
    public Transform content;
    public GameObject historyTextPrefab;

    public void OpenHistory()
    {
        historyPanel.SetActive(true);
    }

    public void CloseHistory()
    {
        historyPanel.SetActive(false);
    }

    public void AddHistory(string characterName, string dialogue)
    {
        GameObject newText = Instantiate(historyTextPrefab, content);

        TMP_Text text = newText.GetComponent<TMP_Text>();

        if (text != null)
        {
            text.text = characterName + "\n" + dialogue;
        }
    }

    public void TestHistory()
    {
        AddHistory("Test Character", "Hello! This is a test.");
    }
}