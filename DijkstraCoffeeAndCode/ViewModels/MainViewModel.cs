using DijkstraCoffeeAndCode.Resources.Themes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Notify([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        public DijkstraGraphViewModel Graph { get; private set; }

        public GraphFileManager FileManager { get; private set; }

        private const int VIEW_SIZE = 2048;
        private double _viewWidth = VIEW_SIZE;
        public double ViewWidth
        {
            get { return _viewWidth; }
            set { _viewWidth = value; Notify(); }
        }

        private double _viewHeight = VIEW_SIZE;
        public double ViewHeight
        {
            get { return _viewHeight; }
            set { _viewHeight = value; Notify(); }
        }

        private double _zoomLevel = 0.8;
        public double ZoomLevel
        {
            get => _zoomLevel;
            set
            {
                _zoomLevel = value;
                if (_zoomLevel > 1.5) { _zoomLevel = 1.5; }
                else if (_zoomLevel < 0.5) { _zoomLevel = 0.5; }

                ViewHeight = VIEW_SIZE * _zoomLevel;
                ViewWidth = VIEW_SIZE * _zoomLevel;
            }
        }

        private int _currentStyleIndex;
        private string _nextStyleName;
        public string NextStyleName
        {
            get { return _nextStyleName; }
            set
            {
                _nextStyleName = value;
                Notify();
            }
        }


        public MainViewModel()
        {
            Graph = new DijkstraGraphViewModel();
            FileManager = new GraphFileManager(Graph);
            FileManager.GetFilePath = GetFilePath;
            Graph.MessageEvent += GraphMessageEvent;
            FileManager.MessageEvent += GraphMessageEvent;

            _currentStyleIndex = Themes.GetCurrentThemeIndex(Application.Current.Resources.MergedDictionaries);
            KeyValuePair<string, string> nextStyle;
            Themes.GetNext(_currentStyleIndex, out nextStyle);
            _nextStyleName = nextStyle.Key;
        }


        private void GraphMessageEvent(string message)
        {
            MessageBox.Show(message);
        }

        private string GetFilePath(bool isOpen, string extensions)
        {
            if (isOpen)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = extensions;
                if (openFileDialog.ShowDialog() == true)
                {
                    return openFileDialog.FileName;
                }
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = extensions;
                if (saveFileDialog.ShowDialog() == true)
                {
                    return saveFileDialog.FileName;
                }
            }
            return "";
        }

        public void ScaleZoom(double scale)
        {
            ZoomLevel *= scale;
        }


        private void SetNewColorScheme(string colorFile, string previousColorFile)
        {
            var dictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault(resource => resource.Source.OriginalString.Contains(previousColorFile));
            if (dictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(dictionary);
            }
            ResourceDictionary newColorScheme = new();
            newColorScheme.Source = new Uri(colorFile, UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(newColorScheme);
        }

        public void MoveToNextColorScheme()
        {
            KeyValuePair<string, string> currentStyle = Themes.ColorSchemeList[_currentStyleIndex];
            KeyValuePair<string, string> nextStyle;
            _currentStyleIndex = Themes.GetNext(_currentStyleIndex, out nextStyle);
            SetNewColorScheme(nextStyle.Value, currentStyle.Value);
            Themes.GetNext(_currentStyleIndex, out nextStyle);
            NextStyleName = nextStyle.Key;
        }

    }
}
