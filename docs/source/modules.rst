..
    SPDX-FileCopyrightText: 2011-2025 EasyCoding Team

    SPDX-License-Identifier: GPL-3.0-or-later

.. _modules:

*******************************
Additional modules
*******************************

.. index:: report builder, tech support reports
.. _modules-reporter:

Report builder
==========================================

This module automatically generates reports for the official support service or various community forums.

Only Microsoft Windows platform is supported.

To create a new report, follow these steps:

  1. select the **Tools** -- **Report builder** menu item or simply press **F12** on the keyboard;
  2. click the **Generate report!** button and wait from 1 to 5 minutes (depending on your system);
  3. you will see the created Zip file in your default file manager.

Report builder uses only built-in utilities. The final report will include the following information:

  * common information about hardware, software and operating system;
  * DirectX information;
  * generic network information: routes, netstat;
  * ping and traceroute to Valve servers;
  * installed game configs;
  * Steam logs;
  * Steam crash dumps;
  * contents of the Hosts file.

The report is a simple Zip archive, so you can load it with any archiver and check its contents before uploading it anywhere.

.. index:: updating program, application updates
.. _modules-updater:

Program updater
==========================================

You can start this module from the **Help** -- **Check for updates** menu item.

The Updater will immediately check for program updates.

SRC Repair will automatically check for updates once a week (can be disabled in :ref:`advanced settings <settings-advanced>`).

The updates are downloaded from the CDN as separate installers.

If the updates have been found, they can be installed by left-clicking on the message.

If SRC Repair is installed for all users in a protected system directory, such as **%PROGRAMFILES%**, a UAC dialog box will appear.

.. index:: quick add-on installer
.. _modules-installer:

Quick add-on installer
==========================================

This module will allow you to easily install the following custom content into the selected game:

  * **sprays** (files **\*.vtf**);
  * **demos** (files **\*.dem**);
  * **game configs** (files **\*.cfg**);
  * **custom maps** (files **\*.bsp**);
  * **game packages** (files **\*.vpk**);
  * **binary plugins** (files **\*.dll**);
  * **hit and kill sounds** (files **killsound.wav** or **hitsound.wav**);
  * **content archives** (files **\*.zip**).

Click the **Browse** button and find the desired file on disk. Don't forget to change the **File type** if you cannot find it.

Now click the **Install** button to install the selected file.

Installation will be performed in the game's custom directory (if supported). Do not install content from untrusted sources.

.. index:: system buttons disabler, windows keys
.. _modules-kbd:

System buttons disabler
==========================================

Most gamers are annoyed by accidentally pressing the Windows button. Some of them even physically remove it from the keyboard.

This module allows you to disable both Windows and Context keys.

Supported actions:

  * **Disable left WIN** -- disable only left Windows button;
  * **Disable both WIN** -- disable both left and right Windows buttons;
  * **Disable right WIN and MENU** -- disable right Windows and Menu (Context) buttons;
  * **Disable both WIN and MENU** -- disable both left and right Windows and Menu (Context) buttons;
  * **Restore default settings** -- restore the default settings.

To change keyboard settings, local administrator rights are required. Only Microsoft Windows platform is supported.

.. index:: in-game mute manager, muted players manager
.. _modules-mute:

Muted players manager
==========================================

With this module you can easily manage the list of muted and ignored players in the selected game.

To add a new row, simply start typing in the last cell.

SteamID32 (legacy) and SteamIDv3 formats are only supported. The legacy entry can be converted to a modern one by clicking the **Convert SteamID format** button on the main toolbar or in the context menu.

You can select and remove one or more rows by using the **Delete selected row** button on the main toolbar, or by pressing **Delete** on your keyboard.

If you want to show the selected user's profile in a web browser, click the **Open Steam profile** button on the main toolbar or in the context menu.

Click the **Save** button on the main toolbar to save changes.

If safe clean is enabled (green light in the status bar), a backup file will be created automatically. You can restore or delete it on :ref:`BackUps <backups-about>` tab.

.. index:: cleanup module, safe cleanup window
.. _modules-cleanup:

Cleanup module
==========================================

This module cannot be called directly by the user. It is used by various cleaning methods from the :ref:`trobleshooting and cleanup <cleanup-wizard>` tab and the :ref:`extended cleanup module <modules-stmcln>`.

The main window consists of a table with candidates for deletion, control buttons and a progress bar.

Depending on the type of cleanup, files may or may not be marked for automatic deletion.

Click the **Execute cleanup button** to start the cleanup sequence. All marked files will be deleted.

If you change your mind, click the **Cancel** button. No actions will be performed.

If you want to create backups before clearing all files, enable the checkbox **Compress files to zip before deletion** in :ref:`common settings <settings-main>`. It will take a long time (depending on the number of files). You can restore or delete it on :ref:`BackUps <backups-about>` tab.

You will see a progress bar with status during operation.

The cleanup module supports the following hotkeys:

 * **Ctrl + A** -- select all files for deletion;
 * **Ctrl + D** -- deselect all files;
 * **Ctrl + R** -- invert selection;
 * **Ctrl + C** -- copy the marked file names with full path to the clipboard.

.. index:: extended cleanup, steam cleanup module
.. _modules-stmcln:

Steam cleanup module
==========================================

This module will find and remove garbage, accumulated on regular daily use. We recommend running it at least once a month.

You need to select one or multiple checkboxes and then click the **Execute cleanup** button. The :ref:`Cleanup module <modules-cleanup>` window will appear.

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
