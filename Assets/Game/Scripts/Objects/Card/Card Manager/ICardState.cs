using Cysharp.Threading.Tasks;

public interface ICardState
{
    UniTask OnEnter();
    UniTask OnExit();
    void Update();
}