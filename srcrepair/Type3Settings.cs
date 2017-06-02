/*
 * Служебный класс работы с настройками графики Type 3 игр.
 * 
 * Copyright 2011 - 2017 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2017 EasyCoding Team.
 * 
 * Лицензия: GPL v3 (см. файл GPL.txt).
 * Лицензия контента: Creative Commons 3.0 BY.
 * 
 * Запрещается использовать этот файл при использовании любой
 * лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
 * 
 * Официальный блог EasyCoding Team: https://www.easycoding.org/
 * Официальная страница проекта: https://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/

namespace srcrepair
{
    /// <summary>
    /// Служебный класс работы с настройками графики Type 3 игр.
    /// </summary>
    public class Type3Settings
    {
        /// <summary>
        /// Хранит имя настройки ID производителя видеокарты.
        /// </summary>
        public string VendorID { get; private set; }

        /// <summary>
        /// Хранит имя настройки ID видеокарты.
        /// </summary>
        public string DeviceID { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества обычных эффектов.
        /// </summary>
        public string EffectDetails { get; private set; }

        /// <summary>
        /// Хранит имя настройки выделенного игре пула памяти.
        /// </summary>
        public string MemoryPoolType { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества детализации моделей и текстур.
        /// </summary>
        public string TextureModelQuality { get; private set; }

        /// <summary>
        /// Хранит имя настройки разрешения по горизонтали.
        /// </summary>
        public string ScreenWidth { get; private set; }

        /// <summary>
        /// Хранит имя настройки разрешения по вертикали.
        /// </summary>
        public string ScreenHeight { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества шейдерных эффектов.
        /// </summary>
        public string ShaderEffects { get; private set; }

        /// <summary>
        /// Хранит имя настройки версии конфига настроек графики.
        /// </summary>
        public string SchemaVersion { get; private set; }

        /// <summary>
        /// Хранит имя настройки известности устройства.
        /// </summary>
        public string KnownDevice { get; private set; }

        /// <summary>
        /// Хранит имя настройки параметров полноэкранного режима.
        /// </summary>
        public string DisplayMode { get; private set; }

        /// <summary>
        /// Хранит имя настройки параметров полноэкранного режима в
        /// кооперативном режиме.
        /// </summary>
        public string DisplayModeCoop { get; private set; }

        /// <summary>
        /// Хранит имя настройки других параметров графического режима (без рамки).
        /// </summary>
        public string DisplayBorderless { get; private set; }

        /// <summary>
        /// Хранит имя настройки вертикальной синхронизации.
        /// </summary>
        public string VSync { get; private set; }

        /// <summary>
        /// Хранит имя настройки параметров полноэкранного режима при потере фокуса.
        /// </summary>
        public string DisplayModeFocusLoss { get; private set; }

        /// <summary>
        /// Хранит имя настройки параметров рендеринга водных поверхностей.
        /// </summary>
        public string CheapWaterReflections { get; private set; }

        /// <summary>
        /// Хранит имя настройки высоты и качества эффектов тумана.
        /// </summary>
        public string FogHeight { get; private set; }

        /// <summary>
        /// Хранит имя настройки, позволяющей использовать только простые
        /// источники освещения.
        /// </summary>
        public string SimpleLight { get; private set; }

        /// <summary>
        /// Базовый конструктор класса.
        /// </summary>
        public Type3Settings()
        {
            //
        }
    }
}
