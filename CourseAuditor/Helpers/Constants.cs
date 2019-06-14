using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Helpers
{
    public static class Constants
    {
        public static int LessonsCount => 12;
        public static double CoursePrice => 6500;
        public static string GroupText => "Группа";
        public static int DefaultModuleLengthMonth => 3;
        public static int NotRespectfulReason => 1;
        public static int RespectfulReason => 2;
        public static int Attendance => 0;
        public static string DefaultPhoneNumberStart => "+7";
        public static string StudentColumnName => "Студент";
        public static string BalanceColumnName => "Баланс";
    }
}
