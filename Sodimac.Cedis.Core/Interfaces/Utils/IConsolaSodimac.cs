using System;

namespace ola_automatica.worker.core.Interfaces
{
    public interface IConsolaSodimac
    {
        void LogError(string value);
        void LogCritical(string value, Exception ex);
        void LogSuccess(string value);
        void LogInfo(string value);
        void LogWarning(string value);

        void Log(string value);

        void TextError(string value);
        void TextSuccess(string value);
        void TextInfo(string value);
        void TextWarning(string value);
    }
}
