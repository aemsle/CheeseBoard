using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Runtime.CompilerServices;

namespace BluEditor.Utilities
{
    public enum MessageType
    {
        NONE = 0b0000,
        INFO = 0b0001,
        WARNING = 0b0010,
        ERROR = 0b0100
    }

    public class LogMessage
    {
        public DateTime Time { get; }
        public MessageType MessageType { get; }
        public string Message { get; }
        public string File { get; }
        public string Caller { get; }
        public int Line { get; }
        public string MetaData => $"{File}: {Caller}() (Line: {Line})";

        public LogMessage(MessageType in_type, string in_message, string in_file, string in_caller, int in_line)
        {
            Time = DateTime.Now;
            MessageType = in_type;
            Message = in_message;
            File = Path.GetFileName(in_file);
            Caller = in_caller;
            Line = in_line;
        }
    }

    public class Logger
    {
        private static int m_messageFilter = (int)(MessageType.INFO | MessageType.WARNING | MessageType.ERROR);
        private static readonly ObservableCollection<LogMessage> m_messages = new ObservableCollection<LogMessage>();

        public static ReadOnlyObservableCollection<LogMessage> Messages
        { get; } = new ReadOnlyObservableCollection<LogMessage>(m_messages);

        public static CollectionViewSource FilteredMessages
        { get; } = new CollectionViewSource() { Source = Messages };

        public static async void Log(string in_message, MessageType in_type = MessageType.INFO,
            [CallerFilePath] string in_file = "", [CallerMemberName] string in_caller = "",
            [CallerLineNumber] int in_line = 0)
        {
            // Call log on the UI thread to prevent WPF shitting the bed :)
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                m_messages.Add(new LogMessage(in_type, in_message, in_file, in_caller, in_line));
            }
            ));
        }

        public static async void Clear()
        {
            // Call log on the UI thread to prevent WPF shitting the bed :)
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                m_messages.Clear();
            }
            ));
        }

        public static void SetFilter(int in_mask)
        {
            m_messageFilter = in_mask;
            FilteredMessages.View.Refresh();
        }

        static Logger()
        {
            FilteredMessages.Filter += (s, e) =>
            {
                int type = (int)((LogMessage)e.Item).MessageType;
                e.Accepted = (type & m_messageFilter) != 0;
            };
        }
    }
}