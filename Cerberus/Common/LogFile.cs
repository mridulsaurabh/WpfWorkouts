using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Infrastructure.Common;
using System.ComponentModel.DataAnnotations;

namespace Cerberus
{
    [Serializable]
    public class LogFile : ModelBase, ISerializable
    {
        #region fields and constructors
        private string _name;
        private string _author;

        public LogFile()
        {

        }

        public LogFile(SerializationInfo info, StreamingContext context)
        {
            Name = Convert.ToString(info.GetValue("Name", typeof(String)));
            Author = Convert.ToString(info.GetValue("Author", typeof(String)));
        }
        #endregion

        #region properties and delegates

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        [Required(ErrorMessage = "Author Name is mandatory.")]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage="Author name can't have special charecters.")]
        public string Author
        {
            get
            {
                return _author;
            }
            set
            {
                SetProperty(ref this._author, value);
                ValidateProperty(value);
            }
        }

        public int LogRate
        {
            get;
            set;
        }

        public LogRateInterval Interval
        {
            get;
            set;
        }

        public DateTime LogDuration
        {
            get;
            set;
        }

        #endregion

        #region events and methods

        public void serialize(string fileName)
        {
            FileStream fileStream;
            fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, this);
            fileStream.Close();
        }

        public LogFile deSerialize(string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            LogFile logfile = (LogFile)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return logfile;
        }

        #endregion

        #region ISerializable members
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", _name);
            info.AddValue("Author", _author);           
        }
        #endregion
    }
    
}
