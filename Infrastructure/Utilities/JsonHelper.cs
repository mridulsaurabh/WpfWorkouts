using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Infrastructure.Utility
{
  public static class JsonHelper
  {
    public static T Deserialize<T>(string jsonValue) where T : class
    {
      T obj = default (T);
      if (!string.IsNullOrWhiteSpace(jsonValue))
      {
        try
        {
          Encoding.Unicode.GetBytes(jsonValue);
          using (MemoryStream memoryStream = new MemoryStream())
            obj = (T) new DataContractJsonSerializer(typeof (T)).ReadObject((Stream) memoryStream);
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }
      return obj;
    }

    public static string Serialize<T>(this T input) where T : class
    {
      string str = string.Empty;
      try
      {
        if ((object) input != null)
        {
          using (MemoryStream memoryStream = new MemoryStream())
          {
            new DataContractJsonSerializer(typeof (T)).WriteObject((Stream) memoryStream, (object) input);
            str = Encoding.Unicode.GetString(memoryStream.ToArray(), 0, Convert.ToInt32(memoryStream.Length));
          }
        }
      }
      catch (InvalidDataContractException ex)
      {
        throw ex;
      }
      return str;
    }
  }
}
