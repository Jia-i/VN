using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text;

public class HistoryManager : MonoBehaviour
{
    public GameObject historyPanel;
    public Transform content;
    public GameObject historyTextPrefab;

    private List<string> historyEntries = new List<string>();

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
        string entry;
        if (string.IsNullOrEmpty(characterName))
        {
            entry = dialogue;
        }
        else
        {
            entry = $"<b>{characterName}</b>\n{dialogue}";        
        }
        historyEntries.Add(entry);
        SpawnEntry(entry);
    }

    public void AddChoice(string choice)
    {
        string entry = $"<b><color=#88CCFF>>> {choice}</color></b>";
        historyEntries.Add(entry);
        SpawnEntry(entry);
    }

    private void SpawnEntry(string entry)
    {
        GameObject newText = Instantiate(historyTextPrefab, content);
        TMP_Text text = newText.GetComponent<TMP_Text>();
        if (text != null)
        {
            text.text = entry;
        }
    }

    public void ClearHistoryUI()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    public void ResetHistory()
    {
        historyEntries.Clear();
        ClearHistoryUI();
    }

    public string GetHistoryAsString()
    {
        return string.Join("###ENTRY###", historyEntries);
    }

    public void LoadHistoryFromString(string data)
    {
        ClearHistoryUI();
        historyEntries.Clear();

        if (!string.IsNullOrEmpty(data))
        {
            string[] entries = data.Split(new string[] { "###ENTRY###" }, System.StringSplitOptions.None);
            foreach (string entry in entries)
            {
                historyEntries.Add(entry);
                SpawnEntry(entry);
            }
        }
    }
}