.. This file is a part of SRC Repair project. For more information
.. visit official site: https://www.easycoding.org/projects/srcrepair
..
.. Copyright (c) 2011 - 2021 EasyCoding Team (ECTeam).
.. Copyright (c) 2005 - 2021 EasyCoding Team.
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
.. _backups:

**********************************
BackUps Manager
**********************************

On this tab you can view, remove or restore backups, created by program.

.. index:: backups, managing backups
.. _backups-about:

Managing backup files
==========================================

When safe cleanup is on (green light in status bar), application will automatically create backup files for the most performed actions.

You can select multiple backups by pressing **Ctrl** or **Shift**.

Plain text backup files can be viewed or edited by a :ref:`default text editor <settings-advanced>`. Just select such file and press **Edit in Notepad** button on the main toolbar.

Binary files with **bud** extension can be opened by any Zip-archiver.

.. index:: backups, backup types
.. _backups-types:

BackUp types
==========================================

Typical backup file name ``Container_UNIXTIME.bud`` consists of prefix **Container**, **UNIXTIME** -- current datetime in UnixTime format and extension **bud**.

BUD-files are standard Zip-archives, which contains removed by program files with keeping the original directory hierarchy.

The names of the registry backup files (only in version for Microsoft Windows) are generated as follows:

 * ``Game_AutoBackUp_UNIXTIME.reg`` -- automatic backup of game video settings;
 * ``Game_Options_UNIXTIME.reg`` -- manual backup of game video settings;
 * ``Source_Options_UNIXTIME.reg`` -- manual backup of all Source Engine games video settings;
 * ``Steam_BackUp_UNIXTIME.reg`` -- manual backup of all Steam settings, stored in Windows registry.

.. index:: backups, backup storage
.. _backups-storage:

Backup storage
==========================================

All backup files are stored in ``%APPDATA%\SRC Repair\backups`` directory, in subdirectories for each game.

The number of backups is not limited, so don't forget to remove them manually. No automatic removal will be performed.

Old backup files (older than 30 days) will be highlighted in list for your convenience (it can be disabled in :ref:`settings <settings-main>`).

.. index:: backup restoration
.. _backups-restore:

Backup restoration
==========================================

If you want to restore backup, just select file (or files) from the list and press **Restore selected backup** button on the main toolbar.

Confirm restoration.

.. index:: backups, backup removal
.. _backups-remove:

Backup removal
==========================================

If you want to remove backup, just select file (or files) from the list and press **Remove selected backup** button on the main toolbar.

Confirm deletion. Removed files cannot be restored!

.. index:: backups, registry backups
.. _backups-registry:

Creating registry backups
==========================================

If you want to create backup of video settings, video settings of all Source Engine games or Steam settings, stored in Windows registry, press button **Create** on the main toolbar and select any option from dropdown menu. Backup file will be created and added to the list.
