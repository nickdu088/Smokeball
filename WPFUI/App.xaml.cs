using Autofac;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using WPFUI.Command;
using WPFUI.ViewModel;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();

            // Register individual components
            builder.RegisterType<SearchViewModel>().SingleInstance();
            builder.RegisterInstance<HttpClient>(new HttpClient());
            builder.RegisterType<SearchCommand>().As<ICommand>();
            builder.RegisterType<MainWindow>().AsSelf();

            var container = builder.Build();

            var viewModel = container.Resolve<SearchViewModel>();

            var window = container.Resolve<MainWindow>();
            window.DataContext  = viewModel;
            window.Show();
            base.OnStartup(e);
        }
    }
}
