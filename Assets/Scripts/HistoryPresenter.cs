using UnityEngine;
using Yarn.Unity;

public class HistoryPresenter : DialoguePresenterBase
{
    public HistoryManager historyManager;

    public override YarnTask OnDialogueStartedAsync()
    {
        return YarnTask.CompletedTask;
    }

    public override YarnTask OnDialogueCompleteAsync()
    {
        return YarnTask.CompletedTask;
    }

    public override YarnTask RunLineAsync(
        LocalizedLine line,
        LineCancellationToken token)
    {
        string characterName = line.CharacterName;
        string dialogue = line.TextWithoutCharacterName.Text;
        if (historyManager != null)
        {
            historyManager.AddHistory(characterName, dialogue);
        }

        return YarnTask.CompletedTask;
    }
}