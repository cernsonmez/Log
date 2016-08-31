using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Log.DataTypes
{
    public enum LogType
    {
        Debug,
        Info,
        Fatal,
        Error,
        Trace,
        Warn
    };

    [Serializable]
    public class LogInfo
    {
        public LogInfo(LogType type, int sequenceid, string appname, string message)
        {
            this.LogId = Guid.NewGuid().ToString();
            this.TimeStamp = DateTime.Now;
            this.Type = type;
            this.AppName = appname;
            this.Message = message;
            this.SequenceID = sequenceid;

        }

        [BsonId]
        private string LogId { get; set; }

        [BsonElement("sequence")]
        public int SequenceID { get; set; }

        [BsonElement("appname")]
        public string AppName { get; set; }

        [BsonElement("timestamp")]
        public DateTime TimeStamp { get; set; }

        [BsonElement("type")]
        public LogType Type { get; set; }

        [BsonElement("message")]
        public string Message { get; set; }

    }
}
