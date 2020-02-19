.. _cleanup:

*******************************
Troubleshooting and cleanup
*******************************

With tools, presented on this page, you can resolve most of the known Steam and Source Engine games issues. Also you can clean up garbage, accumulated in game on regular daily use.

.. index:: troubleshooting, blob files cleanup, registry entries cleanup
.. _cleanup-troubleshooting:

Troubleshooting and recovery
==========================================

To run Steam client recovery, enable **Clean .blob files from Steam directory** or/and **Clean all registry entries** (require admin rights; not available on non-Windows platforms) checkboxes, then press button **Cleanup Now!**.

Warning! Steam client will be automatically terminated (if running).

Known issues, can be resolved by this tool:

 * Steam logon errors;
 * errors like **Steam servers is not available**, **This game is not available at this time**, **No Steam connection**, **Cannot connect to Steam network**, etc.;
 * endless Steam connection to servers;
 * some issues, related to Friends system;
 * damaged Steam installation (after running this, Steam client will be re-downloaded from official servers);
 * some other issues.

.. index:: troubleshooting, principle of operation, recovery process
.. _cleanup-principle:

Principle of operation
============================================

 * if **Clean .blob files from Steam directory** is checked, SRC Repair will get Steam installation directory path, find and remove files with .blob extension, and force Steam recovery on next launch. Warning! Steam can force you to login again after performing this action.
 * if **Clean all registry entries** is checked, SRC Repair will remove ``HKEY_CURRENT_USER\Software\Valve\Steam`` registry key, modify some values in ``HKEY_LOCAL_MACHINE\Software\Valve\Steam`` (only when running with admin rights), and force Steam recovery on next launch. You will need to specify Steam language from list.

.. index:: troubleshooting, path checker
.. _cleanup-pathcheck:

Installation path checker
============================================

This tool automatically check for restricted (non-ASCII) symbols in Steam installation directory path. Steam path should contain only latin letters and numbers. All other can cause major issues with games, compiled without Unicode support.

If restricted symbols are found, a warning message will be shown. Also you will see red sign in **General information** section.

This tool starts automatically with SRC Repair and cannot be disabled.

.. index:: cleanup, game cleanup, safe cleanup
.. _cleanup-wizard:

Game cleanup wizard
===============================================

This tool allow you to clean up garbage, accumulated in game on regular daily use. It can free-up lots of disk space.

After selecting cleanup action, you will see a separate :ref:`safe clean window <modules-cleanup>`. You can check the list of files, marked to deletion.

All marked files will be removed. If you don't want to remove some files, uncheck checkbox near its name.

When done, press **Execute cleanup** button to run cleanup sequence. If you changed your mind  -- press **Cancel**.

Currently supported by SRC Repair cleanu actions:

 * **custom maps** -- remove downloaded or installed custom maps. This will free lots of disk space and can speed-up game startup;
 * **download cache** -- remove downloaded from game servers data: personal sprays of all players, you ever seen on servers, different partial downloaded or damaged during download files, etc. Contains garbage. We recommend to run this cleanup at least once a week;
 * **custom directory** -- remove installed custom modifications from ``/custom`` directory (only if supported by game);
 * **custom sounds** -- remove sound files downloaded from game servers;
 * **FPS-configs** -- remove installed :ref:`FPS-configs <fps-about>`;
 * **graph and sound cache** -- remove sound and graphic caches, created by game. It can fix some issues. Will be automatically created on next game launch;
 * **secondary cache** -- remove downloaded from game servers cache files. We recommend to run this cleanup at least once a month;
 * **screenshots** -- remove screenshots from ``/screenshots`` directory. Can free-up lots of disk space;
 * **recorded demos** -- remove downloaded or installed demo files (files \*.dem) from game directory;
 * **models and textures** -- remove models and textures downloaded from game servers or installed by different custom modifications;
 * **deep cleanup** -- try to remove all garbage from game at once. You **must** run game :ref:`cache verification <cleanup-advanced>` after running this cleanup;
 * **replays** -- remove files, created by Replays system (only if supported by game).

.. index:: cleanup, game cleanup, deep cleanup
.. _cleanup-advanced:

Deep cleanup
============================================

 * **Clean game settings (+video)** -- reset all in-game video settings. Backup file will be created.
 * **Remove all binaries and launchers** -- remove game binaries and launchers (contents of ``/bin``, ``/{game}/bin`` directories and ``/hl2.exe`` file). Use this if you have issues with starting game, crashes of different origin, etc. You **must** verify game cache after running this cleanup.
 * **Validate game cache files** -- force game cache verification. Will check all game files and re-download corrupted or missing. You must run this process after running deep cleanup or removing game binaries and launchers.
