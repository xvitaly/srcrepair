..
    SPDX-FileCopyrightText: 2011-2025 EasyCoding Team

    SPDX-License-Identifier: GPL-3.0-or-later

.. _backups:

**********************************
BackUps Manager
**********************************

.. index:: about backups manager
.. _backups-about:

About backups manager
==========================================

On this page, you can view, remove, or restore backups created by the program.

When safe cleanup is enabled (green light in the status bar), the application automatically creates backup files for the most performed actions.

.. index:: working with backup files
.. _backups-working:

Working with backup files
==========================================

You can select multiple backups by pressing **Ctrl** or **Shift**.

Plain text backup files can be viewed or edited using a :ref:`text editor <settings-advanced>`. Simply select such a file and click the **Edit in Notepad** button on the main toolbar.

Binary files with the **bud** extension can be opened by any Zip-archiver.

.. index:: backup types
.. _backups-types:

Backup types
==========================================

A typical backup file name ``Container_UNIXTIME.bud`` consists of the following parts:

  * **Container** -- prefix;
  * **UNIXTIME** -- current datetime in UnixTime format;
  * **bud** -- file extension.

BUD files are standard Zip-archives containing files removed by the program, preserving the original directory hierarchy.

Registry backup file names (only on Microsoft Windows platform) are generated as follows:

 * ``Game_AutoBackUp_UNIXTIME.reg`` -- automatic backup of the game video settings;
 * ``Game_Options_UNIXTIME.reg`` -- manual backup of the game video settings;
 * ``Source_Options_UNIXTIME.reg`` -- manual backup of all Source Engine games video settings (obsolete; no longer available for creation, but can be restored from existing backup files);
 * ``Steam_BackUp_UNIXTIME.reg`` -- manual backup of all Steam settings, stored in the Windows registry.

.. index:: backup storage
.. _backups-storage:

Backup storage
==========================================

All backup files are stored in ``%APPDATA%\SRC Repair\backups`` directory, in subdirectories for each game.

The number of backups is not limited, so don't forget to remove them manually. No automatic removal will be performed.

Old backup files (older than 30 days) will be highlighted in the list for your convenience (this can be disabled in :ref:`settings <settings-main>`).

.. index:: backup restoration
.. _backups-restore:

Backup restoration
==========================================

If you want to restore a backup, just select the file (or files) from the list and click the **Restore selected backup** button on the main toolbar.

Confirm restoration.

.. index:: backup removal
.. _backups-remove:

Backup removal
==========================================

If you want to remove a backup, just select the file (or files) from the list and click the **Remove selected backup** button on the main toolbar.

Confirm deletion. Removed files cannot be restored!

.. index:: registry backups
.. _backups-registry:

Creating manual backups
==========================================

If you want to create a backup of the game video settings or Steam settings, stored in the Windows registry, click the **Create** button on the main toolbar and select an option from the drop-down menu. The backup file will be created and added to the list.
