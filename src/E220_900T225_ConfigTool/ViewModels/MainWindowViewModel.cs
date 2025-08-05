using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace E220_900T225_ConfigTool.ViewModels
{

    public class MainWindowViewModel : BindableBase
    {
        private string _title = "E220_900T225_ConfigTool";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IEventAggregator ea)
        {
        }
    }
}
