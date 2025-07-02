
using Cysharp.Threading.Tasks;


public abstract class IGameInitializer : ComponentBehaviour
{
   public abstract UniTask Init();
  
}
