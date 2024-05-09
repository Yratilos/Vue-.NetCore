namespace WebApi.Systems.DataBases
{
    public class DataBaseFactory
    {
        public static IDataBase CreateDataBase(string type, string conn)
        {
            switch (type)
            {
                case "KingBase":
                    return KingBase.GetInstance(conn);
                default:
                    return SqlServer.GetInstance(conn);
            }
        }
    }
}
