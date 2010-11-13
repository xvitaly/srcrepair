﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace srcrepair {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class AppStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AppStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("srcrepair.AppStrings", typeof(AppStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Внимание! Программа запущена нет от учётной записи с правами администратора, поэтому некоторые функции будут отключены. Чтобы получить к ним доступ, перезапустите её с првами администратора!.
        /// </summary>
        internal static string AppLaunchedNotAdmin {
            get {
                return ResourceManager.GetString("AppLaunchedNotAdmin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Произошла ошибка при создании резервной копии. Резервная копия не была создана!.
        /// </summary>
        internal static string BackUpCreationFailed {
            get {
                return ResourceManager.GetString("BackUpCreationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сохранить файл конфигурации?.
        /// </summary>
        internal static string CE_CfgSV {
            get {
                return ResourceManager.GetString("CE_CfgSV", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Внимание! Во время сохранения файла произошла ошибка. Файл не был сохранён!.
        /// </summary>
        internal static string CE_CfgSVVEx {
            get {
                return ResourceManager.GetString("CE_CfgSVVEx", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Создать резервную копию файла.
        /// </summary>
        internal static string CE_CreateBackUp {
            get {
                return ResourceManager.GetString("CE_CreateBackUp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Внимание! При попытке открыть файл произошла ошибка. Возможно, файл открыт с ошибками. Пожалуйста, сообщите о ней в наш багтрекер (Справка - Сообщить об ошибке в программе)..
        /// </summary>
        internal static string CE_ExceptionDetected {
            get {
                return ResourceManager.GetString("CE_ExceptionDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Невозможно считать содержимое конфига, т.к. файл не найден!.
        /// </summary>
        internal static string CE_OpenFailed {
            get {
                return ResourceManager.GetString("CE_OpenFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Внимание! Настоятельно не рекомендуется редактировать этот файл, т.к. это может привести к непредсказуемым последствиям. Редактируйте его на свой страх и риск..
        /// </summary>
        internal static string CE_RestConfigOpenWarn {
            get {
                return ResourceManager.GetString("CE_RestConfigOpenWarn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Произошла критическая ошибка: невозможно установить выбранный конфиг!.
        /// </summary>
        internal static string FP_InstallFailed {
            get {
                return ResourceManager.GetString("FP_InstallFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Внимание! Установка FPS-конфига снизит все графические настройки на абсолютный минимум, что приведёт к значительному ухудшению качества графики игры и увеличению производительности игры. Вы уверены, что хотите установить FPS-конфиг?.
        /// </summary>
        internal static string FP_InstallQuestion {
            get {
                return ResourceManager.GetString("FP_InstallQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Конфиг был успешно установлен!.
        /// </summary>
        internal static string FP_InstallSuccessful {
            get {
                return ResourceManager.GetString("FP_InstallSuccessful", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Внимание! Для выбранной игры у нас нет FPS-конфигов, поэтому данная страница будет недоступна!.
        /// </summary>
        internal static string FP_NoCfgGame {
            get {
                return ResourceManager.GetString("FP_NoCfgGame", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to К сожалению, описание выбранного Вами конфига недоступно. Приносим извинения за доставленные неудобства..
        /// </summary>
        internal static string FP_NoDescr {
            get {
                return ResourceManager.GetString("FP_NoDescr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы не выбрали FPS-конфиг из списка. Выберите конфиг и повторите попытку!.
        /// </summary>
        internal static string FP_NothingSelected {
            get {
                return ResourceManager.GetString("FP_NothingSelected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Произошла ошибка при удалении установленного конфига!.
        /// </summary>
        internal static string FP_RemoveFailed {
            get {
                return ResourceManager.GetString("FP_RemoveFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Нечего удалять: для текущей игры FPS-конфиги не установлены!.
        /// </summary>
        internal static string FP_RemoveNotExists {
            get {
                return ResourceManager.GetString("FP_RemoveNotExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы уверены что хотите удалить конфиг?.
        /// </summary>
        internal static string FP_RemoveQuestion {
            get {
                return ResourceManager.GetString("FP_RemoveQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Установленный FPS-конфиг был успешно удалён!.
        /// </summary>
        internal static string FP_RemoveSuccessful {
            get {
                return ResourceManager.GetString("FP_RemoveSuccessful", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Выберите конфиг из списка выше!.
        /// </summary>
        internal static string FP_SelectFromList {
            get {
                return ResourceManager.GetString("FP_SelectFromList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Закрыть приложение? Все несохранённые изменения будут потеряны!.
        /// </summary>
        internal static string FrmCloseQuery {
            get {
                return ResourceManager.GetString("FrmCloseQuery", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Обнаружено исключение, которое мы не смогли обработать. Пожалуйста, сообщите разработчикам на email: vitaly@easycoding.org..
        /// </summary>
        internal static string GeneralErrorDetected {
            get {
                return ResourceManager.GetString("GeneralErrorDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Включить DirectX 8 режим рендера? Это значительно увеличит производительность игры..
        /// </summary>
        internal static string GT_DxLevelMsg {
            get {
                return ResourceManager.GetString("GT_DxLevelMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Внимание! Обнаружен установленный FPS-конфиг, поэтому настройки с этой страницы кроме режима DirectX будут игнорироваться игрой до удаления FPS-конфига..
        /// </summary>
        internal static string GT_FPSCfgDetected {
            get {
                return ResourceManager.GetString("GT_FPSCfgDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы действительно хотите установить настройки графики TF2 на рекомендуемый максимум? Для этого потребуется мощный компютер!.
        /// </summary>
        internal static string GT_MaxPerfMsg {
            get {
                return ResourceManager.GetString("GT_MaxPerfMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы действительно хотите установить все настройки видео на минимальные? Это увеличит FPS, но ухудшит графику в игре..
        /// </summary>
        internal static string GT_MinPerfMsg {
            get {
                return ResourceManager.GetString("GT_MinPerfMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Готово. Не забудьте подкорректировать и сохранить изменения в настройках!.
        /// </summary>
        internal static string GT_PerfSet {
            get {
                return ResourceManager.GetString("GT_PerfSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сохранить сделанные Вами изменения?.
        /// </summary>
        internal static string GT_SaveMsg {
            get {
                return ResourceManager.GetString("GT_SaveMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Изменения были успешно сохранены!.
        /// </summary>
        internal static string GT_SaveSuccess {
            get {
                return ResourceManager.GetString("GT_SaveSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отмена.
        /// </summary>
        internal static string InputBoxCancelBtnName {
            get {
                return ResourceManager.GetString("InputBoxCancelBtnName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Внимание! В памяти обнаружен активный процесс.
        /// </summary>
        internal static string KillMessage1 {
            get {
                return ResourceManager.GetString("KillMessage1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Настоятельно рекомендуется завершить его работу во избежании серьёзных ошибок. Завершить работу этого процесса сейчас.
        /// </summary>
        internal static string KillMessage2 {
            get {
                return ResourceManager.GetString("KillMessage2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Извините, но данная функция ещё не реализована!.
        /// </summary>
        internal static string NotImplementedYet {
            get {
                return ResourceManager.GetString("NotImplementedYet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Обнаружен параметр командной строки, но не обнаружено его значение, поэтому игнорируем данный параметр!.
        /// </summary>
        internal static string ParamError {
            get {
                return ResourceManager.GetString("ParamError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Выполнить очистку?.
        /// </summary>
        internal static string PS_ExecuteMSG {
            get {
                return ResourceManager.GetString("PS_ExecuteMSG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы неправильно выбрали язык Steam из выпадающего списка, поэтому будет использоваться английский!.
        /// </summary>
        internal static string PS_NoLangSelected {
            get {
                return ResourceManager.GetString("PS_NoLangSelected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы ничего не выбрали! Выберите хотя бы одну из опций очистки и повторите запуск!.
        /// </summary>
        internal static string PS_NothingSelected {
            get {
                return ResourceManager.GetString("PS_NothingSelected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Внимание! В памяти был обнаружен запущенный процесс Steam. Steam был успешно завершён..
        /// </summary>
        internal static string PS_ProcessDetected {
            get {
                return ResourceManager.GetString("PS_ProcessDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Очистка успешно выполнена! Спасибо за использование программы. Теперь Вы можете запустить Steam!.
        /// </summary>
        internal static string PS_SeqCompleted {
            get {
                return ResourceManager.GetString("PS_SeqCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Логин Steam был успешно сменён. Продолжаем работу с новым....
        /// </summary>
        internal static string StatusLoginChanged {
            get {
                return ResourceManager.GetString("StatusLoginChanged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Все системы работают в штатном режиме..
        /// </summary>
        internal static string StatusNormal {
            get {
                return ResourceManager.GetString("StatusNormal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сейчас в редакторе открыт файл:.
        /// </summary>
        internal static string StatusOpenedFile {
            get {
                return ResourceManager.GetString("StatusOpenedFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пожалуйста, выберите приложение из списка!.
        /// </summary>
        internal static string StatusSApp {
            get {
                return ResourceManager.GetString("StatusSApp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пожалуйста, выберите Ваш логин Steam из списка!.
        /// </summary>
        internal static string StatusSLogin {
            get {
                return ResourceManager.GetString("StatusSLogin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы отказались вводить свой логин Steam, поэтому работа программы невозможна. Повторите запуск позднее и введите правильный логин!.
        /// </summary>
        internal static string SteamLoginCancel {
            get {
                return ResourceManager.GetString("SteamLoginCancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Введите Ваш логин Steam:.
        /// </summary>
        internal static string SteamLoginEnterText {
            get {
                return ResourceManager.GetString("SteamLoginEnterText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ввод логина.
        /// </summary>
        internal static string SteamLoginEnterTitle {
            get {
                return ResourceManager.GetString("SteamLoginEnterTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Внимание! Source Repair не смог получить список активных логинов Steam, поэтому Вам придётся ввести его вручную..
        /// </summary>
        internal static string SteamLoginsNotDetected {
            get {
                return ResourceManager.GetString("SteamLoginsNotDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Внимание! В пути к Steam обнаружены недопустимые символы: русские, немецкие и т.д. буквы, либо символы Юникода. Steam и TF2 будут работать с ошибками. Переустановите Steam в папку, путь до которой будет содержать только латинские буквы..
        /// </summary>
        internal static string SteamNonASCIIDetected {
            get {
                return ResourceManager.GetString("SteamNonASCIIDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Внимание! В пути к Steam обнаружены недопустимые символы. Steam и TF2 будут работать с ошибками. Переустановите Steam в папку, путь до которой будет содержать только латинские буквы..
        /// </summary>
        internal static string SteamNonASCIISmall {
            get {
                return ResourceManager.GetString("SteamNonASCIISmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Обнаружены запрещённые символы!.
        /// </summary>
        internal static string SteamNonASCIITitle {
            get {
                return ResourceManager.GetString("SteamNonASCIITitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ключи реестра, относящиеся к Steam не обнаружены. Вам не нужна их чистка. Нажмите OK для выхода из программы!.
        /// </summary>
        internal static string SteamNotDetected {
            get {
                return ResourceManager.GetString("SteamNotDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы отказались вводить путь к Steam, поэтому программа будет завершена!.
        /// </summary>
        internal static string SteamPathCancel {
            get {
                return ResourceManager.GetString("SteamPathCancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы указали неправильный путь! Программа будет завершена..
        /// </summary>
        internal static string SteamPathEnterErr {
            get {
                return ResourceManager.GetString("SteamPathEnterErr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Укажите путь к установленному Steam:.
        /// </summary>
        internal static string SteamPathEnterText {
            get {
                return ResourceManager.GetString("SteamPathEnterText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Укажите путь к Steam.
        /// </summary>
        internal static string SteamPathEnterTitle {
            get {
                return ResourceManager.GetString("SteamPathEnterTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Произошла критическая ошибка: не могу получить путь к Steam из реестра, поэтому Вам придётся вручную указать правильный путь!.
        /// </summary>
        internal static string SteamPathNotDetected {
            get {
                return ResourceManager.GetString("SteamPathNotDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Безымянный.cfg.
        /// </summary>
        internal static string UnnamedFileName {
            get {
                return ResourceManager.GetString("UnnamedFileName", resourceCulture);
            }
        }
    }
}
