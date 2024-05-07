using System.Data;

namespace ToDoApp.Data
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
    }
}
