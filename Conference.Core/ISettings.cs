namespace Conference.Core
{
    public interface ISettings
    {
        string ConferenceName { get; }

        string CurrentNumber { get; }

        string NextNumber { get; }

        string Password { get; }
    }
}