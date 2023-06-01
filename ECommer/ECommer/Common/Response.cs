namespace ECommer.Common
{
    public class Response
    {
        #region Properties
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Result { get; set; }
        #endregion
    }
}
