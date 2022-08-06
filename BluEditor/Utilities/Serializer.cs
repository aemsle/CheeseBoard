using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace BluEditor.Utilities
{
    public static class Serializer
    {
        public static void WriteToFile<T>(T in_instance, string in_filepath)
        {
            try
            {
                using FileStream fs = new FileStream(in_filepath, FileMode.Create);
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(fs, in_instance);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Utilities.Logger.Log($"Failed to write {nameof(in_instance)}: {in_filepath}", Utilities.MessageType.ERROR);
                throw;
            }
        }

        public static T ReadFromFile<T>(string in_filepath)
        {
            try
            {
                using FileStream fs = new FileStream(in_filepath, FileMode.Open);
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                T instance = (T)serializer.ReadObject(fs);
                return instance;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log($"Failed to read from: {in_filepath}", MessageType.ERROR);
                throw;
            }
        }
    }
}