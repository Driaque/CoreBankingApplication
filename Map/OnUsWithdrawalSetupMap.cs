using Core;

namespace Map
{
    public class OnUsWithdrawalSetupMap : EntityMap<OnUsWithdrawalSetup>
    {
        public OnUsWithdrawalSetupMap()
        {

            Map(x => x.Name);
            Map(x => x.TerminalId);
            Map(x => x.Location);

        }
    }
}
