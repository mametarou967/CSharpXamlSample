using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using E220_900T225_ConfigTool.Model.Common;
using E220_900T225_ConfigTool.Model.Logging;
using E220_900T225_ConfigTool.Model.E220_900_T225_ProtocolManager;
using E220_900T225_ConfigTool.Model.SerialPortManager;
using System;
using System.Collections.ObjectModel;
using E220_900T225_ConfigTool.Model.Register;
using System.Linq;

namespace E220_900T225_ConfigTool.ViewModels
{
    public class Item
    {
        public string Display { get; set; }
        public AirDataRate_SF_BW Value { get; set; }
    }

    public class MainWindowViewModel : BindableBase
    {
        private string _title = "E220_900T225_ConfigTool";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        IEventAggregator _ea;
        LogWriter logWriter;

        public MainWindowViewModel(IEventAggregator ea)
        {
            // コマンドの準備
            SerialStartButton = new DelegateCommand(SerialStartButtonExecute);
            SerialStopButton = new DelegateCommand(SerialStopButtonExecute);
            ZenSetteiToiawaseButton = new DelegateCommand(NinshouYoukyuuCommandTestSendButtonExecute);
            NinshouJoutaiYoukyuuCommandTestSendButton = new DelegateCommand(NinshouJoutaiYoukyuuCommandTestSendButtonExecute);
            LogClearButton = new DelegateCommand(LogClearButtonExecute);
            PortListSelectionChanged = new DelegateCommand<object[]>(PortListChangedExecute);
            SendReg02WriteCommandButton = new DelegateCommand(SendReg02WriteCommandButtonExecute);
            SendReg04WriteCommandButton = new DelegateCommand(SendReg04WriteCommandButtonExecute);
            SendDataWriteCommandButton = new DelegateCommand(SendDataWriteCommandButtonExecute);
            DummyButton = new DelegateCommand(DummyButtonExecute);

            // コンボボックスの準備
            SerialPortManager.GetAvailablePortNames().ForEach(serialPort =>
            {
                if (!string.IsNullOrEmpty(serialPort))
                {
                    _serialPortList.Add(new ComboBoxViewModel(Common.ExtractNumber(serialPort),serialPort));
                }
            });

            _ea = ea;
            logWriter = new LogWriter(_ea,(Log log) =>
            {
                _logItems.Add(
                    new LogItem()
                    {
                        Timestamp = log.dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        Content = log.content
                    });
            });

            SelectedCombo1 = Combo1List.FirstOrDefault(item => item.Value == AirDataRate_SF_BW.AirDataRate_62500_SF_5_BW_500kHz);
            InputChannelText = "0";
        }

        /// ボタン関係
        /// シリアル通信関係
        public DelegateCommand SerialStartButton { get; }

        private void SerialStartButtonExecute()
        {
            if (!string.IsNullOrEmpty(selectedSerialComPort))
            {
                serialInterfaceProtocolManager.ComStart(selectedSerialComPort, new LogWriteRequester(_ea));
            }   
        }

        public DelegateCommand SerialStopButton { get; }

        private void SerialStopButtonExecute()
        {
            serialInterfaceProtocolManager.ComStop();
        }

        public DelegateCommand ZenSetteiToiawaseButton { get; }

        // 認証要求関係

        private string _ninshouYoukyuuRiyoushaId = "00043130";

        public string NinshouYoukyuuRiyoushaId
        {
            get { return _ninshouYoukyuuRiyoushaId; }
            set { SetProperty(ref _ninshouYoukyuuRiyoushaId, value); }
        }

        private void NinshouYoukyuuCommandTestSendButtonExecute()
        {
            serialInterfaceProtocolManager.SendDataWrite(new byte[] { 0xC1, 0x00, 0x09 });
        }

        public DelegateCommand NinshouJoutaiYoukyuuCommandTestSendButton { get; }

        private void NinshouJoutaiYoukyuuCommandTestSendButtonExecute()
        {
        }

        public DelegateCommand LogClearButton { get; }

        private void LogClearButtonExecute()
        {
            LogItems = new ObservableCollection<LogItem>();
        }

        /// Combo Box
        private ObservableCollection<ComboBoxViewModel> _serialPortList =
            new ObservableCollection<ComboBoxViewModel>();

        public ObservableCollection<ComboBoxViewModel> SerialPortList
        {
            get { return _serialPortList; }
            set { SetProperty(ref _serialPortList, value); }
        }

        public DelegateCommand<object[]> PortListSelectionChanged { get; }

        private void PortListChangedExecute(object[] selectedItems)
        {
            try
            {
                var selectedItem = selectedItems[0] as ComboBoxViewModel;
                selectedSerialComPort = selectedItem.DisplayValue;
            }
            catch { }
        }

        E220_900_T225_ProtocolManager serialInterfaceProtocolManager = new E220_900_T225_ProtocolManager();
        String selectedSerialComPort = "";

        ObservableCollection<LogItem> _logItems =
            new ObservableCollection<LogItem>();
        public ObservableCollection<LogItem> LogItems
        {
            get { return _logItems; }
            set { SetProperty(ref _logItems, value); }
        }

        public DelegateCommand IncrementYoukyuuOutouJikanMsCommand { get; private set; }
        public DelegateCommand DecrementYoukyuuOutouJikanMsCommand { get; private set; }

        public DelegateCommand IncrementYoukyuuJoutaiOutouJikanMsCommand { get; private set; }
        public DelegateCommand DecrementYoukyuuJoutaiOutouJikanMsCommand { get; private set; }


        public ObservableCollection<Item> Combo1List { get; } = new ObservableCollection<Item>()
        {
            new Item(){ Display = "Air Data Rate:  1953 bps, SF(拡散率): 10, BW(帯域幅): 250 kHz" , Value = AirDataRate_SF_BW.AirDataRate_1953_SF_10_BW_250kHz },
            new Item(){ Display = "Air Data Rate:  2148 bps, SF(拡散率): 11, BW(帯域幅): 500 kHz" , Value = AirDataRate_SF_BW.AirDataRate_2148_SF_11_BW_500kHz },
            new Item(){ Display = "Air Data Rate:  3125 bps, SF(拡散率):  8, BW(帯域幅): 125 kHz" , Value = AirDataRate_SF_BW.AirDataRate_3125_SF_8_BW_125kHz },
            new Item(){ Display = "Air Data Rate:  3516 bps, SF(拡散率):  9, BW(帯域幅): 250 kHz" , Value = AirDataRate_SF_BW.AirDataRate_3516_SF_9_BW_250kHz },
            new Item(){ Display = "Air Data Rate:  3906 bps, SF(拡散率): 10, BW(帯域幅): 500 kHz" , Value = AirDataRate_SF_BW.AirDataRate_3906_SF_10_BW_500kHz },
            new Item(){ Display = "Air Data Rate:  5469 bps, SF(拡散率):  7, BW(帯域幅): 125 kHz" , Value = AirDataRate_SF_BW.AirDataRate_5469_SF_7_BW_125kHz },
            new Item(){ Display = "Air Data Rate:  6250 bps, SF(拡散率):  8, BW(帯域幅): 250 kHz" , Value = AirDataRate_SF_BW.AirDataRate_6250_SF_8_BW_250kHz },
            new Item(){ Display = "Air Data Rate:  9357 bps, SF(拡散率):  6, BW(帯域幅): 125 kHz" , Value = AirDataRate_SF_BW.AirDataRate_9357_SF_6_BW_125kHz },
            new Item(){ Display = "Air Data Rate: 12500 bps, SF(拡散率):  8, BW(帯域幅): 500 kHz" , Value = AirDataRate_SF_BW.AirDataRate_12500_SF_8_BW_500kHz },
            new Item(){ Display = "Air Data Rate: 15625 bps, SF(拡散率):  5, BW(帯域幅): 125 kHz" , Value = AirDataRate_SF_BW.AirDataRate_15625_SF_5_BW_125kHz },
            new Item(){ Display = "Air Data Rate: 18750 bps, SF(拡散率):  6, BW(帯域幅): 250 kHz" , Value = AirDataRate_SF_BW.AirDataRate_18750_SF_6_BW_250kHz },
            new Item(){ Display = "Air Data Rate: 21875 bps, SF(拡散率):  7, BW(帯域幅): 500 kHz" , Value = AirDataRate_SF_BW.AirDataRate_21875_SF_7_BW_500kHz },
            new Item(){ Display = "Air Data Rate: 31250 bps, SF(拡散率):  5, BW(帯域幅): 250 kHz" , Value = AirDataRate_SF_BW.AirDataRate_31250_SF_5_BW_250kHz },
            new Item(){ Display = "Air Data Rate: 37500 bps, SF(拡散率):  6, BW(帯域幅): 500 kHz" , Value = AirDataRate_SF_BW.AirDataRate_37500_SF_6_BW_500kHz },
            new Item(){ Display = "Air Data Rate: 62500 bps, SF(拡散率):  5, BW(帯域幅): 500 kHz <default>" , Value = AirDataRate_SF_BW.AirDataRate_62500_SF_5_BW_500kHz }, //default
        };

        private Item _selectedCombo1;
        public Item SelectedCombo1
        {
            get => _selectedCombo1;
            set
            {
                SetProperty(ref _selectedCombo1, value);
            }
        }

        private string _inputChannelText = "0";
        public string InputChannelText
        {
            get => _inputChannelText;
            set => SetProperty(ref _inputChannelText, value);
        }

        // ボタン押下コマンド
        public DelegateCommand SendReg02WriteCommandButton { get; }

        private void SendReg02WriteCommandButtonExecute()
        {
            serialInterfaceProtocolManager.SendReg02Write(SelectedCombo1.Value);
        }

        public DelegateCommand SendReg04WriteCommandButton { get; }

        private void SendReg04WriteCommandButtonExecute()
        {
            if (byte.TryParse(InputChannelText, out byte channel) && channel <= 38)
            {
                serialInterfaceProtocolManager.SendReg04Write(channel);
            }
            else
            {
                // errmessage
            }
        }

        private string _inputDataText = "0";
        public string InputDataText
        {
            get => _inputDataText;
            set => SetProperty(ref _inputDataText, value);
        }

        public DelegateCommand SendDataWriteCommandButton { get; }

        private void SendDataWriteCommandButtonExecute()
        {
            serialInterfaceProtocolManager.SendDataWrite(System.Text.Encoding.ASCII.GetBytes(InputDataText));
        }


        public DelegateCommand DummyButton { get; }

        private void DummyButtonExecute()
        {
            serialInterfaceProtocolManager.SendDataWrite(new byte[] {0xC0,0x05,0x01,0x03 });
        }
    }
}
