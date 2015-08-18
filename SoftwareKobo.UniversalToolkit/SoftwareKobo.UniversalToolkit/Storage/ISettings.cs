namespace SoftwareKobo.UniversalToolkit.Storage
{
    internal interface ISettings
    {
        bool Exist(string key);

        T Read<T>(string key);

        bool Remove(string key);

        void Write<T>(string key, T value);
    }
}