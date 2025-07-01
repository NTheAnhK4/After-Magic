
using Cysharp.Threading.Tasks;


public abstract class IGameInitializer : ComponentBehavior
{
   public abstract UniTask Init();
  
}
