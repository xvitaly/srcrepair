using System;
using System.Reflection;
using System.IO;
using System.Globalization;

namespace srcrepair
{
    public sealed class CurrentApp
    {
        /// <summary>
        /// Хранит User-Agent, которым представляется удалённым службам...
        /// </summary>
        public string UserAgent;

        /// <summary>
        /// В этой переменной мы будем хранить полный путь к каталогу установленного
        /// клиента Steam.
        /// </summary>
        public string FullSteamPath;

        /// <summary>
        /// В этой переменной будем хранить полный путь к каталогу с утилитой
        /// SRCRepair для служебных целей.
        /// </summary>
        public string FullAppPath;

        /// <summary>
        /// В этой переменной будем хранить путь до каталога пользователя
        /// программы. Используется для служебных целей.
        /// </summary>
        public string AppUserDir;

        /// <summary>
        /// В этой переменной мы будем хранить полную информацию о версии
        /// приложения для служебных целей.
        /// </summary>
        public string AppVersionInfo;

        /// <summary>
        /// Конструктор класса. Получает информацию для рантайма.
        /// </summary>
        public CurrentApp()
        {
            // Получаем путь к каталогу приложения...
            Assembly Assmbl = Assembly.GetEntryAssembly();
            this.FullAppPath = Path.GetDirectoryName(Assmbl.Location);

            // Укажем путь к пользовательским данным и создадим если не существует...
            this.AppUserDir = Properties.Settings.Default.IsPortable ? Path.Combine(this.FullAppPath, "portable") : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Properties.Resources.AppName);

            // Проверим существование каталога пользовательских данных и при необходимости создадим...
            if (!(Directory.Exists(this.AppUserDir)))
            {
                Directory.CreateDirectory(this.AppUserDir);
            }

            // Получаем информацию о версии нашего приложения...
            this.AppVersionInfo = Assmbl.GetName().Version.ToString();

            // Генерируем User-Agent для SRC Repair...
            this.UserAgent = String.Format(Properties.Resources.AppDefUA, Properties.Resources.PlatformFriendlyName, Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor, CultureInfo.CurrentCulture.Name, this.AppVersionInfo, Properties.Resources.AppName, CoreLib.GetSystemArch());
        }
    }
}
