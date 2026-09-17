namespace CantineApi
{
    public class ResponseWrapper<T>
    {
        public ResponseWrapper(Exception ex)
        {
            this.IsError = true;
            this.ErroMessage = ex.Message;
        }

        public ResponseWrapper(T resultat)
        {
            this.Data = resultat;
        }
        public bool IsError { get; set; } = false;
        public T? Data { get; set; }

        public string? ErroMessage { get; set; }
    }
}
