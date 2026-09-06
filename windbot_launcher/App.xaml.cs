using System;
using System.Text;
using System.Windows;

namespace WindBotLauncher
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Set global exception handler
            DispatcherUnhandledException += (s, args) =>
            {
                var sb = new StringBuilder();
                var ex = args.Exception;
                int depth = 1;
                while (ex != null)
                {
                    sb.AppendLine($"[Level {depth}] {ex.GetType().Name}: {ex.Message}");
                    sb.AppendLine(ex.StackTrace);
                    sb.AppendLine(new string('-', 50));
                    ex = ex.InnerException;
                    depth++;
                }
                MessageBox.Show($"เกิดข้อผิดพลาด:\n\n{sb}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Handled = true;
            };
        }
    }
}
