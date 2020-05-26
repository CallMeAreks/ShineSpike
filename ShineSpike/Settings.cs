namespace ShineSpike
{
    public static class SettingKeys
    {
        public static class Site
        {
            public static readonly string Name = "site:name";
        }

        public static class User
        {
            public const string Password = "user:password";
            public const string Salt = "user:salt";
            public const string UserName = "user:username";
        }
    }
}
