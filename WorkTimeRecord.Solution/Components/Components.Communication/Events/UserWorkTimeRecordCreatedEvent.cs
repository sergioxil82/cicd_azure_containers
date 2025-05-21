namespace Components.Communication.Events
{
    public record UserWorkTimeRecordCreatedEvent : BaseEvent
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastRecord { get; set; }
        public string Mode { get; set; }
    }
}
