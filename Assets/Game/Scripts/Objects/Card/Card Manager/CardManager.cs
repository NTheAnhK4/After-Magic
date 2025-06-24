
using System;
using System.Collections.Generic;
using AudioSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;

using Unity.Mathematics;
using UnityEngine;



public class CardManager : Singleton<CardManager>
{
    [Header("Sound Data")] 
    public SoundData DistributeCardSound;

    public SoundData CollectingCardSound;
  
    
    #region Card In A Dungeon
 
    public Card CardPrefab;
    [HideInInspector] public List<PlayerCardData> MainDesk = new List<PlayerCardData>();
    [HideInInspector] public List<PlayerCardData> DrawPile = new List<PlayerCardData>();
    [HideInInspector] public List<PlayerCardData> DisCardPile = new List<PlayerCardData>();
    [HideInInspector] public List<Card> CardInHands = new List<Card>();
    [HideInInspector] public List<PlayerCardData> DepleteCards;
    #endregion
    
    
    public Card CurrentUsingCard;

    #region Card In Hand Param

    private float angleRange = 35f;
    private Vector2 centerPoint = new Vector2(0, -7);
    private float radius = 30;
    private float maxAngleStep = 5f;

    #endregion
   

    #region Spawn And Despawn

    [HideInInspector] public Vector3 despawnPos = new Vector3(14, -7.25f,0);
    [HideInInspector] public Vector3 despawnScale = new Vector3(.4f, .4f, 1);
    [HideInInspector] public Vector3 despawnRotation = new Vector3(0, 0, 255);
    
    [HideInInspector] public Vector3 spawnPos = new Vector3(-14, -7.25f,0);
    [HideInInspector] public Vector3 spawnScale = new Vector3(.8f, .8f, 1);
    [HideInInspector] public Vector3 spawnRotation = new Vector3(0, 0, 45);

    #endregion

    #region State

    private ICardState currentState;
    private DistributeCardState distributeCardState;
    private CollectingCardState collectingCardState;
    private UsingCardState usingCardState;

    #endregion

    #region Action

    private Action<object> onDistributeCard;
    private Action<object> onCollectingCard;
    private Action<object> onPlayerTurn;
    private Action<object> onUsingCard;

    #endregion

    public Canvas Canvas;
    protected override void Awake()
    {
        base.Awake();
        distributeCardState = new DistributeCardState();
        collectingCardState = new CollectingCardState();
        usingCardState = new UsingCardState(() => InGameManager.Instance.IsTurn(GameStateType.PlayerTurn));
        onDistributeCard = _ => ChangeState(distributeCardState);
        onCollectingCard = _ => ChangeState(collectingCardState);
        onPlayerTurn = _ => ChangeState(usingCardState);
        onUsingCard = _ => ChangeState(usingCardState);
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (Canvas == null) Canvas = transform.parent.parent.parent.GetComponent<Canvas>();
        despawnPos = new Vector3(17, -8.75f,0);
        despawnScale = new Vector3(.4f, .4f, 1);
        despawnRotation = new Vector3(0, 0, 255);
    
        spawnPos = new Vector3(-16.75f, -8.75f,0);
        spawnScale = new Vector3(.8f, .8f, 1);
        spawnRotation = new Vector3(0, 0, 45);
    }

    private void OnEnable()
    {
        ObserverManager<GameStateType>.Attach(GameStateType.DistributeCard, onDistributeCard );
        ObserverManager<GameStateType>.Attach(GameStateType.CollectingCard, onCollectingCard );
        ObserverManager<GameStateType>.Attach(GameStateType.PlayerTurn, onPlayerTurn);
        ObserverManager<GameStateType>.Attach(GameStateType.UsingCard, onUsingCard);
        ObserverManager<GameEventType>.Attach(GameEventType.Win, ClearDesks);
        ObserverManager<GameEventType>.Attach(GameEventType.Lose, ClearDesks);
    }

    private void OnDisable()
    {
        ObserverManager<GameStateType>.Detach(GameStateType.DistributeCard, onDistributeCard );
        ObserverManager<GameStateType>.Detach(GameStateType.CollectingCard, onCollectingCard );
        ObserverManager<GameStateType>.Detach(GameStateType.PlayerTurn, onPlayerTurn);
        ObserverManager<GameStateType>.Detach(GameStateType.UsingCard, onUsingCard);
        ObserverManager<GameEventType>.Detach(GameEventType.Win, ClearDesks);
        ObserverManager<GameEventType>.Detach(GameEventType.Lose, ClearDesks);
    }
    
    public void Init()
    {
        CardInHands = new List<Card>();
        DrawPile = new List<PlayerCardData>(MainDesk);
        DepleteCards = new List<PlayerCardData>();
        DisCardPile = new List<PlayerCardData>();
        
        ObserverManager<CardEventType>.Notify(CardEventType.DrawPileCountChange, DrawPile.Count);
        ObserverManager<CardEventType>.Notify(CardEventType.DiscardPileCountChange, DisCardPile.Count);
        ObserverManager<CardEventType>.Notify(CardEventType.DepleteCardsCountChange, DepleteCards.Count);
    }

   

    
    public void ClearDesks(object param = null)
    {
        foreach (Card card in CardInHands)
        {
            PoolingManager.Despawn(card.gameObject);
        }
        CardInHands.Clear();
    }
  

    public void ArrangeHand(int cardsCount, out List<Vector3> cardPositions, out List<Vector3> cardRotations)
    {
        cardPositions = new List<Vector3>();
        cardRotations = new List<Vector3>();
        
        float startAngle = -1.0f *Mathf.Min(1.0f * (cardsCount - 1)/2 * maxAngleStep,angleRange / 2f);
        float angleStep = Mathf.Min(maxAngleStep, cardsCount == 1 ? 0 :  angleRange / (cardsCount - 1)) ;

        for (int i = 0; i < cardsCount; i++) {
            float angleDeg = startAngle + i * angleStep;
            float angleRad = angleDeg * Mathf.Deg2Rad;

           
            float xPos = centerPoint.x + -1f *radius * Mathf.Sin(angleRad);
            float yPos = centerPoint.y +  -1f * radius * (1 - Mathf.Cos(angleRad));
            
            cardPositions.Add(new Vector3(xPos, yPos, i + 50));
            cardRotations.Add(new Vector3(0,0,angleDeg));
            
        }
    }
    
    private async void ChangeState(ICardState newState)
    {
        if (currentState != null) await currentState.OnExit();
        currentState = newState;
        if (currentState != null) await currentState.OnEnter();
    }

    private void Update() => currentState?.Update();

    public async UniTask CollectingCard(Card card, bool isCollectingAllCard)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(card.transform.DOScale(despawnScale, .4f))
                    .Join(card.transform.DOMove(despawnPos, .65f))
                    .Join(card.transform.DORotate(despawnRotation,.6f));
        
        if (!isCollectingAllCard)
        {
            CardInHands.Remove(card);
            
            DisCardPile.Add(card.CardDataCtrl.PlayerCardData);
            ObserverManager<CardEventType>.Notify(CardEventType.DiscardPileCountChange, DisCardPile.Count);
            SetPositionAndQuaternion();
        }
        await sequence.AsyncWaitForCompletion();
       
        PoolingManager.Despawn(card.gameObject);
        
    }

    private async void SetPositionAndQuaternion()
    {
        if (CardInHands.Count == 0) return;
        List<Vector3> positions;
        List<Vector3> rotations;
        List<UniTask> uniTasks = new List<UniTask>();
        ArrangeHand(CardInHands.Count,out positions, out rotations);
        for (int i = 0; i < CardInHands.Count; ++i)
        {
            if(CardInHands[i] == null) continue;
            CardInHands[i].CardAnimation.SetSiblingIndex(CardInHands.Count - 1 - i);
            uniTasks.Add(SetCardPositionAndQuaternionAnim(positions[i], rotations[i], CardInHands[i]));
        }
        await UniTask.WhenAll(uniTasks);
       
    }

    private async UniTask SetCardPositionAndQuaternionAnim(Vector3 position, Vector3 rotation, Card card)
    {
        if (!(card.transform.position == position && card.transform.rotation == Quaternion.Euler(rotation)))
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(card.transform.DOMove(position, .05f))
                .Join(card.transform.DORotate(rotation, .05f));
            await sequence.AsyncWaitForCompletion();
            card.CardAnimation.SaveInitialTransform();
        }
    }
    public void DisableOtherCardsRayCast()
    {
       
        foreach (Card card in CardInHands)
        {
            if(card == null || card == CurrentUsingCard) continue;
            
            card.SetUsable(false, false);
        }
    }

    public void EnableAllCardsRayCast()
    {
       
        foreach (Card card in CardInHands)
        {
            if (card == null)
            {
                Debug.LogWarning("Card in hand is null");
                continue;
            }
            card.SetUsable(true,true);
        }
    }
    
    
}
