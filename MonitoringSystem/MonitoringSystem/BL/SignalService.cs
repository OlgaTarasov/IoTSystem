using MonitoringSystem.DAL;
using System.Data.SqlClient;

namespace MonitoringSystem.BL
{
    public class SignalService: ISignalService
    {
        const string SOURCE_CODE = "SourceCode";
        const string PAGE = "Page";
        const string SIZE = "Size";

        public List<SignalSource> GetSignalSource()
        {
            string SPName = "GetSource";
            SqlParameter[] sqlParameters = new SqlParameter[] { };

            DAService dAService = new DAService();

            List<SignalSource> list = dAService.ExecuteSP<SignalSource>(SPName, sqlParameters);
            return list;
        }

        public List<SignalDTO> GetSineSignals(int page, int size)
        {
            string SPName = "GetSignals";
            SqlParameter[] sqlParameters = new SqlParameter[] {
            new SqlParameter(SOURCE_CODE, (int) SignalType.Sine),
            new SqlParameter(PAGE, page),
            new SqlParameter(SIZE, size)
            };

            DAService dAService = new DAService();
            List<SignalDTO> list = dAService.ExecuteSP<SignalDTO>(SPName, sqlParameters);
            return list;
        }

        public List<SignalDTO> GetStateSignals(int page, int size)
        {
            string SPName = "GetSignals";
            SqlParameter[] sqlParameters = new SqlParameter[] {
            new SqlParameter(SOURCE_CODE, (int) SignalType.State),
            new SqlParameter(PAGE, page),
            new SqlParameter(SIZE, size)
            };
            DAService dAService = new DAService();

            List<SignalDTO> list = dAService.ExecuteSP<SignalDTO>(SPName, sqlParameters);
            return list;
        }

        public List<SignalDTO> GetAnimalySignals(int page, int size)
        {
            string SPName = "GetAnimalySignals";
            SqlParameter[] sqlParameters = new SqlParameter[] {
            new SqlParameter(PAGE, page),
            new SqlParameter(SIZE, size)
            };
            DAService dAService = new DAService();
            List<SignalDTO> list = dAService.ExecuteSP<SignalDTO>(SPName, sqlParameters);
            return list;
        }
    }
}
