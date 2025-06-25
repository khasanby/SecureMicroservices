namespace Movies.Client.Models;

public sealed class UserInfoViewModel
{
    public Dictionary<string, string> UserInfoDictionary { get; private set; }

    public UserInfoViewModel(Dictionary<string, string> userInfoDictionary)
    {
        UserInfoDictionary = userInfoDictionary ?? new Dictionary<string, string>();
    }
}