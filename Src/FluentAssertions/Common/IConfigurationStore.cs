namespace FluentAssertionsAsync.Common;

public interface IConfigurationStore
{
    string GetSetting(string name);
}
