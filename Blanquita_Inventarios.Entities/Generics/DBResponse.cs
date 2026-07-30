namespace Blanquita_Inventarios.Entities
{
    public class DBResponse<T>
    {
        public bool ExecutionOK;
        public string Message;
        public T Data;
        public int NumRows;
    }
}
