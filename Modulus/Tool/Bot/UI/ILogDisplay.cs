using Tool.Bot.Protocol;

namespace Tool.Bot.UI
{
    public interface IBotUI
    {
        void ShowLog(string log);
        void RemoveClient(Client client);
    }
}
