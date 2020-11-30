using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.Styling;

namespace GraphicExample
{

    /// <summary>
    /// helper superclass for multiplatform app
    /// </summary>
    public class Win : Window
    {
        public IEnumerable<string> CustomStyleUris;

        public Win(IEnumerable<string> customStyles)
        {
            CustomStyleUris = customStyles;
        }
    }

    /// <summary>
    /// helper to manage multiplatform app
    /// </summary>    
    public class App<W> : Application
        where W : Win, new()
    {
        public App()
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Styles.AddRange(new[]
            {
                new StyleInclude(baseUri:null) { Source = new Uri("resm:Avalonia.Themes.Default.DefaultTheme.xaml?assembly=Avalonia.Themes.Default") },
                new StyleInclude(baseUri:null) { Source = new Uri("resm:Avalonia.Themes.Default.Accents.BaseLight.xaml?assembly=Avalonia.Themes.Default") },
            });
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var desktop = this.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            if (desktop != null)
            {
                var w = new W();
                desktop.MainWindow = w;
                foreach (var s in w.CustomStyleUris)
                {
                    this.Styles.Add(
                        new StyleInclude(baseUri: null) { Source = new Uri(s) }
                    );
                }
            }
            base.OnFrameworkInitializationCompleted();
        }
    }

    public static class GuiToolkit
    {

        /// <summary>
        /// helper to create multiplatform window         
        /// </summary>        
        public static void CreateGui<W>() where W : Win, new()
        {
            var buildAvaloniaApp = AppBuilder.Configure<App<W>>().UsePlatformDetect();

            buildAvaloniaApp.StartWithClassicDesktopLifetime(new string[] { });
        }

    }
}
