// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraAlgorithm;
using DijkstraAlgorithm.File;
using DijkstraCoffeeAndCode.ViewModels.Commands;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public delegate string GetFilePath(bool isOpen, string fileExtensions);
    public class GraphFileManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Notify([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public ICommand SaveGraphCommand { get; set; }
        public ICommand LoadGraphCommand { get; set; }
        public ICommand ImportGraphCommand { get; set; }
        public ICommand NewGraphCommand { get; set; }

        public event MessageEventHandler? MessageEvent;

        public GetFilePath? GetFilePath { get; set; }

        private string _currentFilePath = "";

        public string CurrentFilePath
        {
            get { return _currentFilePath; }
            set
            {
                _currentFilePath = value;
                Notify();
                Notify(nameof(CurrentFileName));
            }
        }

        public string CurrentFileName
        {
            get
            {
                if (String.IsNullOrEmpty(_currentFilePath)) { return "New Graph"; }
                return Path.GetFileNameWithoutExtension(_currentFilePath);
            }
        }

        private BaseGraphViewModel _viewModel;
        public GraphFileManager(BaseGraphViewModel viewModel)
        {
            _viewModel = viewModel;

            SaveGraphCommand = new SaveGraphCommand(this);
            LoadGraphCommand = new LoadGraphCommand(this);
            ImportGraphCommand = new ImportGraphCommand(this);
            NewGraphCommand = new NewGraphCommand(this);

        }

        public void LoadGraph()
        {
            if (GetFilePath == null) { return; }
            try
            {
                string filePath = GetFilePath(true, GraphFile.FILE_FILTER);
                if (String.IsNullOrEmpty(filePath)) { return; }
                _viewModel.SetGraph(Graph.LoadGraph(filePath));
                CurrentFilePath = filePath;
            }
            catch (Exception e)
            {
                MessageEvent?.Invoke("Could Not Load Graph");
                Debug.WriteLine(e.Message);
            }
        }

        public void ImportGraph()
        {
            if (GetFilePath == null) { return; }
            try
            {
                string filePath = GetFilePath(true, GraphFile.FILE_FILTER);
                if (String.IsNullOrEmpty(filePath)) { return; }
                _viewModel.Graph.ImportGraph(filePath);
            }
            catch (Exception e)
            {
                MessageEvent?.Invoke("Could Not Import Graph");
                Debug.WriteLine(e.Message);
            }

        }

        public void NewGraph()
        {
            CurrentFilePath = "";
            _viewModel.SetGraph(new Graph());
        }

        public void SaveGraph(bool isSaveAs = true)
        {
            if (GetFilePath == null) { return; }
            try
            {
                if (isSaveAs || String.IsNullOrEmpty(CurrentFilePath))
                {
                    string filePath = GetFilePath(false, GraphFile.FILE_FILTER);
                    if (String.IsNullOrEmpty(filePath)) { return; }
                    Graph.SaveGraph(_viewModel.Graph, filePath);
                    CurrentFilePath = filePath;
                }
                else
                {
                    Graph.SaveGraph(_viewModel.Graph, CurrentFilePath);
                }

            }
            catch (Exception e)
            {
                MessageEvent?.Invoke("Could Not Save Graph");
                Debug.WriteLine(e.Message);
            }
        }



    }
}
