using Core;

namespace Map
{
    public class ATMTerminalMap : EntityMap<ATMTerminal>
    {
        public ATMTerminalMap()
        {
            Map(x => x.Name);
            Map(x => x.TerminalID);
            Map(x => x.Location);
            References(x => x.ATMTill).Not.LazyLoad();
        }
    }
}
