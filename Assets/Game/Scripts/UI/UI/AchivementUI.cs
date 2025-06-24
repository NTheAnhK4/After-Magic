
using System;
using System.Collections.Generic;
using AudioSystem;
using Game.UI;
using TMPro;
using UnityEngine;



public class AchivementUI : UIView
{
   
    [SerializeField] private Transform rewardHolder;
    private List<ItemBase> items = new List<ItemBase>();
    [SerializeField] private ButtonAnimBase redBtn;
    [SerializeField] private ButtonAnimBase greenBtn;
    [SerializeField] private ButtonAnimBase blueBtn;

    [Header("Achivement Infor")] 
    [SerializeField] private TextMeshProUGUI deepeastTxt;
    [SerializeField] private TextMeshProUGUI bossesDefeatedTxt;
    [SerializeField] private TextMeshProUGUI monstersDefeatedTxt;
    [SerializeField] private TextMeshProUGUI elitesDefeatedTxt;
    [SerializeField] private TextMeshProUGUI roomsExplored;
    [SerializeField] private TextMeshProUGUI timePlayed;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (rewardHolder == null) rewardHolder = transform.Find("Reward/View/Content");
        redBtn = transform.Find("Buttons/Red").GetComponent<ButtonAnimBase>();
        greenBtn = transform.Find("Buttons/Green").GetComponent<ButtonAnimBase>();
        blueBtn = transform.Find("Buttons/Blue").GetComponent<ButtonAnimBase>();


        Transform achivementHolder = transform.Find("Achivement");
        if (deepeastTxt == null) deepeastTxt = achivementHolder.Find("Deepeast").GetComponent<TextMeshProUGUI>();
        if (bossesDefeatedTxt == null) bossesDefeatedTxt = achivementHolder.Find("Bosses").GetComponent<TextMeshProUGUI>();
        if (monstersDefeatedTxt == null) monstersDefeatedTxt = achivementHolder.Find("Monsters").GetComponent<TextMeshProUGUI>();
        if (roomsExplored == null) roomsExplored = achivementHolder.Find("Rooms").GetComponent<TextMeshProUGUI>();
        if (elitesDefeatedTxt == null) elitesDefeatedTxt = achivementHolder.Find("Elite Monsters").GetComponent<TextMeshProUGUI>();
        if (timePlayed == null) timePlayed = achivementHolder.Find("Time").GetComponent<TextMeshProUGUI>();
    }

    public override void Show()
    {
        ObserverManager<SoundActionType>.Notify(SoundActionType.PauseAll);
        ShowAchivement();
        base.Show();
    }

    private void ShowAchivement()
    {
        if (InGameManager.Instance == null) return;
        deepeastTxt.text = "<color=#6A5ACD>Deepest level</color> reached: <color=#FFFFFF>" + InGameManager.Instance.CurrentDepth.ToString() + "</color>";
        bossesDefeatedTxt.text = "<color=#B22222>Bosses</color> defeated: <color=#FFFFFF>" + InGameManager.Instance.BossesDefeated.ToString() + "</color>";
        monstersDefeatedTxt.text = "<color=#228B22>Monsters</color> defeated: <color=#FFFFFF>" + InGameManager.Instance.MonstersDefeated.ToString() + "</color>";
        roomsExplored.text = "<color=#4682B4>Rooms</color> explored: <color=#FFFFFF>" + InGameManager.Instance.RoomsExplored.ToString() + "</color>";
        elitesDefeatedTxt.text = "<color=#8A2BE2>Elite monsters</color> defeated: <color=#FFFFFF>" + InGameManager.Instance.EliteMonstersDefeated.ToString() + "</color>";
       
        float time = InGameManager.Instance.TimePlayed;
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        string formattedTime = timeSpan.Hours > 0
            ? string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds)
            : string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);


        timePlayed.text = "<color=#FF8C00>Time</color> played: <color=#FFFFFF>" + formattedTime + "</color>";


    }
    public override void Hide()
    {
        blueBtn.onClick = null;
        redBtn.onClick = null;
        greenBtn.onClick = null;
        
        blueBtn.gameObject.SetActive(false);
        redBtn.gameObject.SetActive(false);
        greenBtn.gameObject.SetActive(false);
        
       
        foreach (ItemBase item in items)
        {
            PoolingManager.Despawn(item.gameObject);
        }
        
        base.Hide();
        ObserverManager<SoundActionType>.Notify(SoundActionType.UnPauseAll);
    }

    public override void OnFinishedShow()
    {
        
        base.OnFinishedShow();
        ShowReward();
    }

    private async void ShowReward()
    {
        items.Clear();
        items = await InventoryManager.Instance.ShowItemInLoot(rewardHolder);
    }



    private void SetButtonInfor(ButtonAnimBase btn, string textInfor, Action action = null, int siblingIndex = 0)
    {
        btn.gameObject.SetActive(true);
        
        var textComp = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (textComp != null) textComp.text = textInfor;

        btn.onClick = null;
        btn.onClick += () => action?.Invoke();
        btn.transform.SetSiblingIndex(siblingIndex);
    }

    public void SetBlueBtn(string textInfor, Action action = null, int siblingIndex = 0) => SetButtonInfor(blueBtn, textInfor, action, siblingIndex);
    public void SetRedBtn(string textInfor, Action action = null, int siblingIndex = 0) => SetButtonInfor(redBtn, textInfor, action, siblingIndex);
    public void SetGreenBtn(string textInfor, Action action = null, int siblingIndex = 0) => SetButtonInfor(greenBtn, textInfor, action, siblingIndex);
}
