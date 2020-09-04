using System.Threading.Tasks;
using System.Windows;

namespace Tool.Bot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //Entry Point
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Task.Run(() =>
            {
                Schema.Protobuf.Api.StartUp();
                Engine.Network.Api.Binder = Schema.Protobuf.Api.Bind;

                Engine.Framework.Api.StartUp();
                Engine.Network.Api.StartUp();

                Api.StartUp();
            }).Wait();
            
        }
    }
}
