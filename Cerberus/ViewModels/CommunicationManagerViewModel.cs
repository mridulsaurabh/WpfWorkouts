using System;
using System.IO.Ports;
using System.Collections.ObjectModel;
using System.Threading;

namespace Cerberus
{
    public enum TransmissionType { Text, Hex }

    public class CommunicationManagerViewModel
    {
        #region fields and constructors
        private TransmissionType m_CurrentTxType;
        private ObservableCollection<SerialPort> m_AvailablePorts;
        private SerialPort m_CurrentPort;       

        public CommunicationManagerViewModel()
        {
            ScanAndDetectReaderCommunicationPorts();
            InitializeAttachedReaders();
        }

        #endregion

        #region properties and delegates

        public event EventHandler<SerialDataEventArgs> NewSerialDataRecieved;

        public ObservableCollection<SerialPort> AvailablePorts
        {
            get
            {
                return m_AvailablePorts;
            }
            set
            {
                if (m_AvailablePorts != value)
                {
                    m_AvailablePorts = value;
                }
            }
        }

        public SerialPort CurrentPort
        {
            get
            {
                return m_CurrentPort;
            }
            set
            {
                if (m_CurrentPort != value)
                {
                    m_CurrentPort = value;
                }
            }
        }
     
        #endregion

        #region events and methods
                
        private void ScanAndDetectReaderCommunicationPorts()
        {
            var availablePortNames = new ObservableCollection<string>(SerialPort.GetPortNames());
            if (availablePortNames.Count > 0)
            {
                // CHECK FOR ALL THE COMMUNICATION PORT AND IDENTIFY ALL THE PORTS WHICH ARE CONNECTED TO THE READERS.
                foreach (string portName in availablePortNames)
                {
                    ComPortSettings cPort = new ComPortSettings(portName);
                    m_CurrentPort = new SerialPort(cPort.PortName, cPort.BaudRate, cPort.Parity, cPort.DataBits, cPort.StopBits);
                    OpenAndAuthenticatePort(m_CurrentPort);
                }                
            }
        }

        private void InitializeAttachedReaders()
        {
            if (m_AvailablePorts != null && m_AvailablePorts.Count > 0)
            {
                foreach (SerialPort sPort in m_AvailablePorts)
                {
                    sPort.DataReceived += new SerialDataReceivedEventHandler(OnSerialPortDataReceived);
                    sPort.Open();
                }
            }
        }             

        public void OpenAndAuthenticatePort(SerialPort currentPort)
        {
            if (currentPort != null)
            {
                currentPort.DataReceived += new SerialDataReceivedEventHandler(OnReaderResponseReceived);
                currentPort.Open();
                WriteData(currentPort, "IsReader");
                Thread.Sleep(5000);
            }
        }       

        public void ClosePort(SerialPort currentPort)
        {
            if (currentPort != null)
            {
                currentPort.Close();
            }
        }

        public void WriteData(SerialPort sPort, string message)
        {
            int bytesToWrite = sPort.BytesToWrite;
            sPort.Write(message);
        }

        private void OnReaderResponseReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string response = m_CurrentPort.ReadExisting();
            if (!string.IsNullOrEmpty(response) && response == "Y")
            {
                m_AvailablePorts.Add(m_CurrentPort);
                ClosePort(m_CurrentPort);
            }
        }

        private void OnSerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string serialData = m_CurrentPort.ReadExisting();
            // PARSE THE DATA AS PER DDFINED PROTOCOL

        }
        #endregion
    }
}
