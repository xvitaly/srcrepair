.. This file is a part of SRC Repair project. For more information
.. visit official site: https://www.easycoding.org/projects/srcrepair
..
.. Copyright (c) 2011 - 2020 EasyCoding Team (ECTeam).
.. Copyright (c) 2005 - 2020 EasyCoding Team.
..
.. This program is free software: you can redistribute it and/or modify
.. it under the terms of the GNU General Public License as published by
.. the Free Software Foundation, either version 3 of the License, or
.. (at your option) any later version.
..
.. This program is distributed in the hope that it will be useful,
.. but WITHOUT ANY WARRANTY; without even the implied warranty of
.. MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
.. GNU General Public License for more details.
..
.. You should have received a copy of the GNU General Public License
.. along with this program. If not, see <http://www.gnu.org/licenses/>.
.. _modules:

*******************************
Additional modules
*******************************

.. index:: report builder, tech support reports
.. _modules-reporter:

Report builder
==========================================

This module will automatically generate reports for official support service or different community forums.

Only Microsoft Windows platform is supported.

To create a new report:

  1. select **Tools** -- **Report builder** menu entry or just press **F12** on keyboard;
  2. press **Generate report!** button and wait from 1 to to 5 minutes (depends on your system);
  3. you will see generated Zip-file in a default file manager.

Report builder is using only integrated into Microsoft Windows utilities. The following information will be included in the final report:

  * common information about hardware, software and operation system using MSInfo;
  * DirectX information;
  * generic network information: routes, netstat;
  * ping and traceroute to Valve servers;
  * game configs;
  * Steam logs;
  * Steam crash dumps;
  * contents of Hosts file.

Report is a simple Zip-file, so you can load it by any archiver and check its contents before uploading to anywhere.

.. index:: updating program, application updates
.. _modules-updater:

Program updater
==========================================

You can start this module from **Help** -- **Check for updates** menu.

Updater will check updates for the following:

  * main program;
  * game database;
  * HUD database.

SRC Repair will automatically check for updates once a week (can be disabled in :ref:`advanced settings <settings-advanced>`).

Database are being download from repository and copied to the installation directory.

Program updates are being downloaded from CDN as a standalone installers.

If updates we found, you can install them by left clicking on message.

If SRC Repair is installed for all users to a system protected directory like **%PROGRAMFILES%**, UAC dialog will appear.

.. index:: quick add-on installer
.. _modules-installer:

Quick add-on installer
==========================================

This module will allow you to easily install into selected game the following custom content:

  * **sprays** (files **\*.vtf**);
  * **demos** (files **\*.dem**);
  * **game configs** (files **\*.cfg**);
  * **custom maps** (files **\*.bsp**);
  * **game packages** (files **\*.vpk**);
  * **binary plugins** (files **\*.dll**);
  * **hit and kill sounds** (files **killsound.wav** or **hitsound.wav**);
  * **content archives** (files **\*.zip**).

Press **Browse** button and find required file on disk. Don't forget to change **File type** if you cannot find it.

Now press **Install** button to install selected file.

Installation will be performed in the game's custom directory (if supported). Do not install content from untrusted sources.

.. index:: system buttons disabler, windows keys
.. _modules-kbd:

System buttons disabler
==========================================

Most of gamers are getting annoyed by accidentally pressed Windows button. Some of them are even removing it physically from keyboard.

This module will allow you to disable both Windows and Context keys.

Supported actions:

  * **Disable left WIN** -- disable only left Windows button;
  * **Disable both WIN** -- disable both left and right Windows buttons;
  * **Disable right WIN and MENU** -- disable right Windows and Menu (Context) buttons;
  * **Disable both WIN and MENU** -- disable both left and right Windows and Menu (Context) buttons;
  * **Restore default settings** -- restore Windows default settings.

Local administrator rights are required in order to change keyboard settings. Only Microsoft Windows platform is supported.

.. index:: in-game mute manager, muted players manager
.. _modules-mute:

Muted players manager
==========================================

With the help of this module you can easily control the list of muted and ignored players in a selected game.

To add a new row, just start typing text in the last cell.

SteamID32 (legacy) and SteamIDv3 formats are only supported. Legacy entry can be converted to a new one by pressing **Convert SteamID format** button on the main toolbar or from context menu.

To remove currently selected row, press **Remove selected row** button on the main toolbar, or press **Delete** on keybooard. You can select and remove multiple rows at once.

If you want to show selected user profile in Web browser, press **Open Steam profile** button on the main toolbar or from context menu.

Press **Save** button on the main toolbar to save changes.

If safe clean is enabled (green light in status bar), backup file will be created automatically. You can restore or delete it on :ref:`BackUps <backups-about>` tab.

.. index:: cleanup module, safe cleanup window
.. _modules-cleanup:

Cleanup module
==========================================

This module cannot be invoked directly by user. It used by different cleanup methods from :ref:`trobleshooting and cleanup <cleanup-wizard>` tab and by :ref:`extended cleanup module <modules-stmcln>`.

Main window consists of table with candidates for deletion, control buttons and a progress bar.

Depending on cleanup type, files can be marked for deletion automatically or not.

Press **Execute cleanup button** to start cleanup sequence. All marked by checkbox files will be removed.

If you changed your mind, press **Cancel** button. No actions will be performed.

If you want to create backups before running cleanup for all files, enable checkbox **Compress files to zip before deletion** in :ref:`common settings <settings-main>`. It will take a long time (depending on the number of files). You can restore or delete it on :ref:`BackUps <backups-about>` tab.

You will see progress bar with status during operation.

Cleanup module support the following hotkeys:

 * **Ctrl + A** -- mark all files for deletion;
 * **Ctrl + D** -- deselect all files;
 * **Ctrl + R** -- invert selection;
 * **Ctrl + C** -- copy marked file names with full path to clipboard.

.. index:: extended cleanup, steam cleanup module
.. _modules-stmcln:

Steam cleanup module
==========================================

This module will find and remove garbage, accumulated on regular daily use. We recommend to run it at least once a month.

You need to select one or multiple checkboxes and then press **Execute cleanup** button. :ref:`Cleanup module <modules-cleanup>` window will appear.

Available cleanup options:

  * basic caches:

    * **Steam client and overlay HTML cache** -- Steam built-in Chromium Embedded Framework web cache;
    * **Steam client HTTP download cache** -- HTTP client download cache;
    * **Steam client depot cache** -- partially downloaded files and depots cache;
    * **Steam shader cache** -- cache of downloaded compiled shader files;
    * **Steam library cache** -- Steam Library cache;

  * basic garbage:

    * **Steam client logs** -- Steam client logs (files **\*.log**);
    * **Steam old binaries** -- no longer needed old binaries and launchers (files **\*.old**);
    * **Steam error dumps** -- generated by Steam crash reports and dumps (files **\*.dmp** and **\*.mdmp**);
    * **Steam build cache** -- updates temporary directory;

  * extended cleanup:

    * **Steam cached game icons** -- Steam Library cached game icons;
    * **Steam Cloud local storage** -- Steam Cloud local storage;
    * **Steam local game stats** -- Steam achievements database for offline use;
    * **Steam music database** -- Steam Music database files;
    * **Steam custom skins** -- all installed custom skins;

  * troubleshooting:

    * **Steam updater cache** -- Steam updater cache with original downloaded files;
    * **Steam Guard cache** -- Steam authorization files.
