namespace YogoServer.Requests
{
    public class InboxMailRequest
    {
        public string User { get; set; }

        public int Index { get; set; }

        public bool Optmize { get; set; } = true;

        public bool IgnoreRandomText { get; set; } = true;
    }
}
