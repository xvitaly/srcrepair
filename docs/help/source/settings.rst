.. _settings:

*******************************
Program settings
*******************************

.. index:: settings, main settings, generic settings, common settings
.. _settings-main:

Common settings
==========================================

 * **Confirm exit** -- enable or disable exit confirmation dialog.
 * **Hide unsupported by application games** -- if enabled, partially supported games will not be shown.
 * **Auto-highlight old backup files** -- enable or disable highlighting of older than 30 days files on :ref:`backups page <backups>`.
 * **Compress files to zip before deletion** -- if enabled, backups for all removed by :ref:`cleanup module <cleanup-wizard>` files will be created. Not recommended, because it will slow down process and create backup containers with garbage files on disk.
 * **Remove empty directories after safe cleanup** -- enable or disable automatic removal of empty directories after running :ref:`cleanup process <cleanup-wizard>`.
 * **Allow download and install latest (untested) HUDs** -- if enabled, :ref:`HUD manager <hud-manager>` will try to download latest versions of HUDs directly from their repositories. Installation of such untested HUDs may be dangerous. Proceed with caution.

.. index:: settings, advanced settings, additional settings
.. _settings-advanced:

Advances settings
==========================================

 * **Allow unsafe cleanup operations** -- allow some unsafe methods for :ref:`cleanup module <cleanup-wizard>`. It will find and remove more files, but can cause game data corruption. You should run :ref:`verification of game cache <cleanup-advanced>` to find and fix possible issues directly after performing cleanup with this feature enabled.
 * **Automatically check for updates** -- enable or disable checking for updates on program startup (once a week).
 * **Don't show outdated HUDs** -- if enabled, :ref:`HUD Manager <hud-manager>` will show outdated versions of HUDs. We do not recommend to install them.
 * **Use mirrors to download FPS-configs** -- enable or disable downloading of :ref:`FPS-configs <fps-configs>` from mirrors instead of using or main content server.
 * **Text editor binary** -- select text editor to load and edit text files instead of using default one. Press **Browse** button and find its executable on disk.
 * **Custom directory name** -- specify directory name for custom stuff installation (only for games with its support).
