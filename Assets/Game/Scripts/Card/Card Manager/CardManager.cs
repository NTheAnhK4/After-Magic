
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;


public class CardManager : Singleton<CardManager>
{
    
    public List<PlayerCardData> CardsAvailable = new List<PlayerCardData>();
    
    #region Card In A Dungeon
 
    public Card CardPrefab;
    [HideInInspector] public List<Card> MainDesk = new List<Card>();
    public List<Card> DrawPile;
    public List<Card> DisCardPile;
    [HideInInspector] public List<Card> DepleteCards;
    
     public List<Card> CardInHands = new List<Card>();

    #endregion
    
 
     public CardInteraction CurrentUsingCard;
    private float angleRange = 35f;
    private Vector2 centerPoint = new Vector2(0, -7);
    private float radius = 30;
    private float maxAngleStep = 5f;

    [HideInInspector] public Vector3 despawnPos = new Vector3(14, -8);
    [HideInInspector] public Vector3 despawnScale = new Vector3(.4f, .4f, 1);
    [HideInInspector] public Vector3 despawnRotation = new Vector3(0, 0, 255);
    
    [HideInInspector] public Vector3 spawnPos = new Vector3(-10, -9.5f,0);
    [HideInInspector] public Vector3 spawnScale = new Vector3(.8f, .8f, 1);
    [HideInInspector] public Vector3 spawnRotation = new Vector3(0, 0, 45);
    private ICardState currentState;
    private DistributeCardState distributeCardState;
    private CollectingCardState collectingCardState;
    private UsingCardState usingCardState;

    protected override void Awake()
    {
        base.Awake();
        distributeCardState = new DistributeCardState();
        collectingCardState = new CollectingCardState();
        usingCardState = new UsingCardState(() => InGameManager.Instance.IsTurn(GameStateType.PlayerTurn));
    }

    

    private void OnEnable()
    {
        ObserverManager<GameStateType>.Attach(GameStateType.DistributeCard,  _ => ChangeState(distributeCardState));
        ObserverManager<GameStateType>.Attach(GameStateType.CollectingCard,  _ => ChangeState(collectingCardState));
        ObserverManager<GameStateType>.Attach(GameStateType.PlayerTurn, _=>ChangeState(usingCardState));
        ObserverManager<GameStateType>.Attach(GameStateType.UsingCard, _=>ChangeState(usingCardState));
    }

    public void Init()
    {
        DrawPile = new List<Card>();
        MainDesk = new List<Card>();
       
        foreach (PlayerCardData cardData in CardsAvailable)
        {
            Card card = PoolingManager.Spawn(CardPrefab.gameObject, 
                spawnPos, 
                quaternion.Euler(spawnRotation), transform).
                GetComponent<Card>();
            MainDesk.Add(card);
            DrawPile.Add(card);
            
            card.CardDataCtrl.Init(cardData);
            card.transform.localScale = spawnScale;
            card.gameObject.SetActive(false);
           
        }
        DisCardPile = new List<Card>();
        DepleteCards = new List<Card>();
        ObserverManager<CardEventType>.Notify(CardEventType.DrawPileCountChange, DrawPile.Count);
        ObserverManager<CardEventType>.Notify(CardEventType.DiscardPileCountChange, DisCardPile.Count);
        ObserverManager<CardEventType>.Notify(CardEventType.DepleteCardsCountChange, DepleteCards.Count);
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
        CardInHands.Remove(card);
        DisCardPile.Add(card);
        if(!isCollectingAllCard) SetPositionAndQuaternion();
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

    
    
}
