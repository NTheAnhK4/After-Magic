
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;



public class CardManager : Singleton<CardManager>
{
    
    public List<Card> CardsAvailable = new List<Card>();
    
    [HideInInspector] public List<Card> cards = new List<Card>();
     public CardInteraction CurrentUsingCard;
    private float angleRange = 35f;
    private Vector2 centerPoint = new Vector2(0, -7);
    private float radius = 30;
    private float maxAngleStep = 5f;

    private Vector3 despawnPos = new Vector3(14, -8);
    private Vector3 despawnScale = new Vector3(.4f, .4f, 1);
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
                    .Join(card.transform.DORotate(new Vector3(0,0,225),.6f));
        cards.Remove(card);
        if(!isCollectingAllCard) SetPositionAndQuaternion();
        await sequence.AsyncWaitForCompletion();
       
        PoolingManager.Despawn(card.gameObject);
        
    }

    private async void SetPositionAndQuaternion()
    {
        if (cards.Count == 0) return;
        List<Vector3> positions;
        List<Vector3> rotations;
        List<UniTask> uniTasks = new List<UniTask>();
        ArrangeHand(cards.Count,out positions, out rotations);
        for (int i = 0; i < cards.Count; ++i)
        {
            uniTasks.Add(SetCardPositionAndQuaternionAnim(positions[i], rotations[i], cards[i]));
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
