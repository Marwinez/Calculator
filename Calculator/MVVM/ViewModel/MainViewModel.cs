using Calculator.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Calculator.MVVM.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _screenVal;
        private List<string> _availableOperations = new List<string> {"+", "-", "*", "/"};
        private DataTable _dataTable = new DataTable();
        private bool _isLastSignAnOperation;
        private bool _isLastSignASemicolon = false;
        private bool _operationUse = false;

        public MainViewModel() 
        {
            ScreenVal = "0";
            AddNumberCommand = new RelayCommand(AddNumber);
            AddOperationCommand = new RelayCommand(AddOperation, CanAddOperation);
            ClearScreenCommand = new RelayCommand(ClearScreen);
            GetResultCommand = new RelayCommand(GetResult, CanGetResult);
        }

        private bool CanGetResult(object obj) => !_isLastSignAnOperation;

        private bool CanAddOperation(object obj) => !_isLastSignAnOperation;

        private void GetResult(object obj)
        {
            var result = Math.Round(Convert.ToDouble(_dataTable.Compute(ScreenVal.Replace(",", "."), "")), 2);

            ScreenVal = result.ToString();
            if (ScreenVal.Contains(","))
            {
                _isLastSignASemicolon = true;
            }
        }

        private void ClearScreen(object obj)
        {
            ScreenVal = "0";

            _isLastSignAnOperation = false;
            _operationUse = false;
        }

        private void AddOperation(object obj)
        {
            var operation = obj as string;

            ScreenVal += operation;

            _isLastSignAnOperation = true;
            _operationUse = false;
        }

        private void AddNumber(object obj)
        {
            var number = obj as string;

            if (ScreenVal == "0" && number != ",")
                ScreenVal = string.Empty;
            else if (number == "," && _availableOperations.Contains(ScreenVal.Substring(ScreenVal.Length - 1)))
            {
                number = "0,";
                _isLastSignASemicolon = true;
            }
            if (number == ",")
            {
                if(!_isLastSignASemicolon && !_operationUse) { 
                    ScreenVal += number;
                    _isLastSignASemicolon = true;
                    _operationUse = true;
                }
            } else
            {
                ScreenVal += number;
                _isLastSignASemicolon = false;
            }

            _isLastSignAnOperation = false;
            



        }

        public ICommand AddNumberCommand { get; set; }
        public ICommand AddOperationCommand { get; set; }
        public ICommand ClearScreenCommand { get; set; }   
        public ICommand GetResultCommand { get; set; } 

        public string ScreenVal
        {
            get { return _screenVal; }
            set { 
                _screenVal = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler
            PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
