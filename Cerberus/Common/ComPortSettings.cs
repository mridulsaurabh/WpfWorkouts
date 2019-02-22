using System.IO.Ports;

namespace Cerberus
{   
    public class ComPortSettings
    {
        #region fields and constructors
        private Parity m_Parity;
        private int m_DataBits;
        private StopBits m_StopBits;
        private int m_BaudRate;
        private string m_PortName = string.Empty;

        public ComPortSettings(string pName)
        {
            m_PortName = pName;
            m_BaudRate = 115200;
            m_Parity = Parity.None;
            m_StopBits = StopBits.None;
        }

        #endregion

        #region properties and deletates

        public string PortName
        {
            get
            {
                return m_PortName;
            }
            set
            {
                if (!m_PortName.Equals(value))
                {
                    m_PortName = value;                  
                }
            }
        }

        public int BaudRate
        {
            get
            {
                return m_BaudRate;
            }
            set
            {
                if (m_BaudRate != value)
                {
                    m_BaudRate = value;                 
                }
            }
        }    

        public Parity Parity
        {
            get
            {
                return m_Parity;
            }
            set
            {
                if (m_Parity != value)
                {
                    m_Parity = value;                  
                }
            }
        }      

        public int DataBits
        {
            get
            {
                return m_DataBits;
            }
            set
            {
                if (m_DataBits != value)
                {
                    m_DataBits = value;                  
                }
            }
        }      

        public StopBits StopBits
        {
            get
            {
                return m_StopBits;
            }
            set
            {
                if (m_StopBits != value)
                {
                    m_StopBits = value;                
                }
            }
        }

        #endregion
    }    
}
