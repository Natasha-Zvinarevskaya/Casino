using System.ComponentModel.DataAnnotations;

namespace Casino.Web.Middleware
{
    public class LogRequest
    {
        public Guid RequestId { get; set; }
        public DateTime DateStart { get; set; }
        [MaxLength(150)]
        public string LoggerName { get; set; }
        [MaxLength(400)]
        public string Url { get; set; }
        public int Type { get; set; }
        public int RequestLenght { get; set; }
        public int ResponseLenght { get; set; }
        public DateTime DateEnd { get; set; }
        public int LevelId { get; set; }
        public Guid SessionToken { get; set; }
        [MaxLength(100)]
        public string UserId { get; set; }
        [MaxLength(200)]
        public string Ip { get; set; }
        [MaxLength(250)]
        public string ShortMessage { get; set; }
        public string UserAgent { get; set; }
        public string Error { get; set; }
        public string RequestBody { get; set; }
        public string Message { get; set; }
    }
}
