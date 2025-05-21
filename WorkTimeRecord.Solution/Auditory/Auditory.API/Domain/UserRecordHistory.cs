namespace Auditory.API.Domain
{
    public class UserRecordHistory
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastRecord { get; set; }
        public string Mode { get; set; }
    }
}
