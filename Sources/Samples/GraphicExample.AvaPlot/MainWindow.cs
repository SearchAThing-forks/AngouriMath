using System;
using System.Threading.Tasks;
using AngouriMath;
using AngouriMathPlot;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using ScottPlot.Avalonia;

namespace GraphicExample
{

    public class MainWindow : Win
    {

        readonly AMPlotter plotter;
        AvaPlot Chart;
        decimal t = 120;

        public MainWindow() : base(new string[] { })
        {
            Width = 660;
            Height = 660;
            var grRoot = new Grid();

            grRoot.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Auto));
            grRoot.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Auto));
            grRoot.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));

            Chart = new AvaPlot();
            Chart.Width = 573;
            Chart.Height = 565;
            Chart.Margin = new Avalonia.Thickness(5, 3, 5, 3);
            plotter = new AMPlotter(Chart);

            Grid.SetRow(Chart, 0);
            grRoot.Children.Add(Chart);

            var btnJump = new Button()
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Width = 100,
                Margin = new Avalonia.Thickness(10),
                Content = "Jump"
            };
            btnJump.Click += (a, b) => { t += 1.0m; };
            Grid.SetRow(btnJump, 1);
            grRoot.Children.Add(btnJump);

            this.Renderer.DrawFps = true;
            this.Renderer.SceneInvalidated += (a, b) =>
            {
                var B = MathS.Var("B");
                var expr2 = B * MathS.Sin(t + B) * MathS.Pow(MathS.e, MathS.i * B * MathS.Cos(t));
                var niceFunc2 = expr2.Compile(B);
                plotter.Clear();
                Chart.plt.Axis(-125, 125, -125, 125);
                plotter.PlotIterativeComplex(niceFunc2, 0, t);
                plotter.Render();
                t += 0.0005m;
            };

            this.Content = grRoot;
        }

    }

}
