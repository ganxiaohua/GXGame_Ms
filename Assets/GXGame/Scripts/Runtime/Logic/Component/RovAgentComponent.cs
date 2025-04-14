using GameFrame;
using RVO;

namespace GXGame.Scripts.Runtime
{
    public class RovAgent : ECSComponent
    {
        public int Value;

        public override void Dispose()
        {
            //TODO
            Simulator.Instance.setAgentRadius(Value, 0);
        }
    }
}