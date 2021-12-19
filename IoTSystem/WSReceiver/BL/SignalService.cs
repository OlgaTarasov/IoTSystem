using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSReceiver.DAL;

namespace WSReceiver.BL
{
    internal class SignalService
    {
        public string Post(Signal signal)
        {
            string SPName = "InsertSignal";
            string json = String.Empty;
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("SourceCode", signal.SignalType),
                new SqlParameter("Value", signal.Value),
                new SqlParameter("TimeStamp", signal.TimeStamp),
                new SqlParameter("IsAnomaly", signal.IsAnomalySignal ? 1 : 0)
            };

            DAService.ExecuteSP(SPName, sqlParameters, out json);

            return json;
        }

        
    }
}
