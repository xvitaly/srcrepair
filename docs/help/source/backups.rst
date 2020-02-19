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

Plain text backup files can be viewed or edited by a :ref:`default text editor <settings-advanced>`. Just select such file and press **Edit in notepad** button.

Binary files with **bud** extension can be opened by any Zip-archiver.

.. index:: backups, backup types
.. _backups-types:

BackUp types
==========================================

Typical backup file name ``Container_UNIXTIME.bud`` consists of prefix **Container**, **UNIXTIME** -- current datetime in UnixTime format and extension **bud**.

BUD-files are Zip-archives, which contains removed by program files with keeping the original directory hierarchy.

The names of the registry backup files (only in version for Microsoft Windows) are generated as follows:

 * ``Game_AutoBackUp_UNIXTIME.reg`` - automatic backup of game video settings;
 * ``Game_Options_UNIXTIME.reg`` - manual backup of game video settings;
 * ``Source_Options_UNIXTIME.reg`` - manual backup of all Source Engine games video settings;
 * ``Steam_BackUp_UNIXTIME.reg`` - manual backup of all Steam settings, stored in Windows registry.

.. index:: backups, backup storage
.. _backups-storage:

Backup storage
==========================================

All backup files are stored in ``%APPDATA%\SRC Repair\backups`` directory, in subdirectories for each game.

The number of backups is not limited, so don't forget to remove them manually. No automatic removal performed.

Old backup files (older than 30 days) will be highlighted in list for your convenience (can be disabled in :ref:`settings <settings-main>`).

.. index:: backup restoration
.. _backups-restore:

Backup restoration
==========================================

If you want to restore backup, just select file (or files) from list and press **Restore selected backup** button on main toolbar.

Confirm restoration.

.. index:: backups, backup removal
.. _backups-remove:

Backup removal
==========================================

If you want to remove backup, just select file (or files) from list and press **Remove selected backup** button on main toolbar.

Confirm deletion. Removed files cannot be restored!

.. index:: backups, registry backups
.. _backups-registry:

Creating registry backups
==========================================

If you want to create backup of video settings, video settings of all Source Engine games or Steam settings, stored in Windows registry, press button **Create** on main toolbar and select option from dropdown menu. Backup file will be created and added to list.
