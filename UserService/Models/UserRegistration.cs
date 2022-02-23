namespace UserService.Models
{
    public class UserRegistration
    {
        public string UserName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsRegistrationComplete { get; set; }

        private UserRegistration(string userName, DateTime registrationDate, bool isRegistrationComplete)
        {
            UserName = userName;
            RegistrationDate = registrationDate;
            IsRegistrationComplete = isRegistrationComplete;
        }

        public static List<UserRegistration> DummyUsers = new()
        {
            new ("Babak", new DateTime(2020, 2, 10), true),
            new ("Ali", new DateTime(2022, 2, 23), false)
        };
    }

    public class NewUserRegistration
    {
        public enum RegistrationStatus
        {
            Complete=0,
            HalfComplete=1,
            NeedReview=2,
        }

        public string UserName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public RegistrationStatus RegistrationStep { get; set; }

        private NewUserRegistration(string userName, DateTime registrationDate, RegistrationStatus registrationStep)
        {
            UserName = userName;
            RegistrationDate = registrationDate;
            RegistrationStep = registrationStep;
        }
        public static List<NewUserRegistration> DummyUsers = new()
        {
            new("Babak", new DateTime(2020, 2, 10), RegistrationStatus.Complete),
            new("Ali", new DateTime(2022, 2, 23), RegistrationStatus.NeedReview)
        };
    }
}
