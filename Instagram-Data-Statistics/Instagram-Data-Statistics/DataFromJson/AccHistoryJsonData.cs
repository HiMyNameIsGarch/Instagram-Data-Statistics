using System;
using System.Collections.Generic;
using System.Text;

namespace Instagram_Data_Statistics.DataFromJson
{
    public class AccHistoryJsonData
    {
        public IEnumerable<LoggerModel> login_history { get; set; }
        public IEnumerable<LoggerModel> logout_history { get; set; }
        public RegistrationInfo registration_info { get; set; }
    }
    public class LoggerModel
    {
        public string timestamp { get; set; }
        public string user_agent { get; set; }
    }
    public class RegistrationInfo 
    {
        public string registration_username { get; set; }
        public string registration_time { get; set; }
        public string registration_email { get; set; }
        public string registration_phone_number { get; set; }
        public string device_name { get; set; }
    }
}
