
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

using UnityEngine;


public class CardManager : MonoBehaviour
{
    public List<Card> CardsAvailable = new List<Card>();
    public List<Card> cards = new List<Card>();

    private float angleRange = 35f;
    private Vector2 centerPoint = new Vector2(0, -7);
    private float radius = 30;
    private float maxAngleStep = 6f;


    private ICardState currentState;
    private DistributeCardState distributeCardState;
    private CollectingCardState collectingCardState;

    private void Awake()
    {
        distributeCardState = new DistributeCardState(this);
        collectingCardState = new CollectingCardState(this);
    }


    private void OnEnable()
    {
        ObserverManager<GameStateType>.Attach(GameStateType.DistributeCard,  _ => ChangeState(distributeCardState));
        ObserverManager<GameStateType>.Attach(GameStateType.CollectingCard,  _ => ChangeState(collectingCardState));
    }

    private void OnDisable()
    {
        ObserverManager<GameStateType>.DetachAll();
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

    
}
