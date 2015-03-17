using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        }
    }
}
