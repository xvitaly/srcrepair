..
    SPDX-FileCopyrightText: 2011-2024 EasyCoding Team

    SPDX-License-Identifier: GPL-3.0-or-later

.. _settings:

*******************************
Program settings
*******************************

.. index:: settings, main settings, generic settings, common settings
.. _settings-main:

Common settings
==========================================

 * **Confirm exit** -- enable or disable exit confirmation dialog.
 * **Hide unsupported by application games** -- enable or disable display of partially supported games.
 * **Auto-highlight old backup files** -- enable or disable highlighting of files older than 30 days on the :ref:`backups page <backups>`.
 * **Compress files to zip before deletion** -- enable or disable creation of backup copies of all files deleted by the :ref:`cleanup module <cleanup-wizard>`. Not recommended as it will slow down the process and create backup containers with junk files on the disk.
 * **Remove empty directories after safe cleanup** -- enable or disable automatic removal of empty directories after the :ref:`cleanup process <cleanup-wizard>`.
 * **Allow download and install latest (untested) HUDs** -- enable or disable the :ref:`HUD manager <hud-manager>` downloading the latest HUD versions directly from their repositories. Installation of such untested HUDs may be dangerous. Proceed with caution.

.. index:: settings, advanced settings, additional settings
.. _settings-advanced:

Advances settings
==========================================

 * **Allow unsafe cleanup operations** -- enable or disable some unsafe methods for the :ref:`cleanup module <cleanup-wizard>`. It will find and remove more files, but may also delete some game data. You should run a :ref:`verification of game cache <cleanup-advanced>` to find and fix possible issues immediately after performing cleanup with this feature enabled.
 * **Automatically check for updates** -- enable or disable checking for updates on program startup (once a week).
 * **Don't show outdated HUDs** -- enable or disable the :ref:`HUD manager <hud-manager>` to display outdated versions of HUDs. We do not recommend to install them.
 * **Text editor binary** -- choose a text editor to load and edit text files instead of using the default one. Click the **Browse** button and find its executable on disk.
 * **Custom directory name** -- specify the directory name for installing custom stuff (only for games with such support).
