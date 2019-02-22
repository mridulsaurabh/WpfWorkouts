using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using Infrastructure.Common;

namespace Cerberus
{
    public class Notification : ModelBase
    {
        #region fields and constructors
        private BitmapImage m_Icon;
        private DateTime m_DateAndTime;
        private string m_Message;
        private Category m_Category;
        private string m_Source;

        public Notification()
        {
            this.Source = String.Empty;
            this.Message = String.Empty;
        }

        public Notification(string message, Category category)
        {
            this.Message = message;
            this.Category = category;
            this.DateAndTime = DateTime.Now;
        }

        public Notification(string source, string message, Category category, DateTime time)
        {
            this.Source = source;
            this.Message = message;
            this.Category = category;
            this.DateAndTime = time;
        }

        #endregion

        #region properties and delegates

        public string Source
        {
            get
            {
                return m_Source;
            }
            private set
            {
                if (m_Source != value)
                {
                    SetProperty(ref this.m_Source, value);
                }
            }
        }

        public string Message
        {
            get
            {
                return m_Message;
            }
            set
            {
                SetProperty(ref this.m_Message, value);
            }
        }

        public Category Category
        {
            get
            {
                return m_Category;
            }
            set
            {
                m_Category = value;
                OnPropertyChanged("Category");
            }
        }

        public DateTime DateAndTime
        {
            get
            {
                return m_DateAndTime;
            }
            set
            {
                SetProperty(ref this.m_DateAndTime, value);
            }
        }

        public BitmapImage Icon
        {
            get
            {
                m_Icon = new BitmapImage();
                switch (m_Category)
                {
                    case Category.Debug:
                        m_Icon = new BitmapImage(new Uri("/Images/question.png", UriKind.RelativeOrAbsolute));
                        break;
                    case Category.Information:
                        m_Icon = new BitmapImage(new Uri("/Images/information.png", UriKind.RelativeOrAbsolute));
                        break;
                    case Category.Warning:
                        m_Icon = new BitmapImage(new Uri("/Images/warning.png", UriKind.RelativeOrAbsolute));
                        break;
                    case Category.Error:
                        m_Icon = new BitmapImage(new Uri("/Images/error.png", UriKind.RelativeOrAbsolute));
                        break;
                    default:
                        break;
                }
                return m_Icon;
            }
        }

        #endregion
    }

    public enum Category
    {
        Debug = 0,
        Information = 1,
        Warning = 2,
        Error = 3
    }

    public enum Priority
    {
        None = 0,
        High = 1,
        Medium = 2,
        Low = 3,
    }   
}
