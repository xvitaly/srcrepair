..
    SPDX-FileCopyrightText: 2011-2024 EasyCoding Team

    SPDX-License-Identifier: GPL-3.0-or-later

.. _cleanup:

*******************************
Troubleshooting and cleanup
*******************************

.. index:: about cleanup manager
.. _cleanup-about:

About cleanup manager
===============================

On this page you can resolve most known Steam and Source Engine games issues. You can also clean up the garbage that has accumulated in the game due to regular daily use.

.. index:: blob files cleanup, registry entries cleanup
.. _cleanup-troubleshooting:

Troubleshooting and recovery
==========================================

To run Steam client recovery, enable the **Clean .blob files from Steam directory** or/and the **Clean all registry entries** (require admin rights; not available on non-Windows platforms) checkboxes, then click the **Cleanup Now!** button.

Warning! The Steam client will be automatically terminated (if it is running).

Known issues that can be resolved with this tool:

 * login errors;
 * errors like **Steam servers are not available**, **This game is not available at this time**, **No Steam connection**, **Could not connect to Steam network**, etc.;
 * endless connection to servers;
 * some issues, related to Friends system;
 * damaged installation (after running this, Steam client will be re-downloaded from the official servers);
 * some other issues.

.. index:: principle of operation, recovery process
.. _cleanup-principle:

Principle of operation
============================================

 * If the **Clean downloaded Steam package cache** is checked, SRC Repair will get the Steam installation path, find and remove all downloaded package cache files, and force Steam recovery the next time you launch it.
 * If the **Clean all registry entries** is checked, SRC Repair will remove the ``HKEY_CURRENT_USER\Software\Valve\Steam`` registry key, and force Steam recovery the next time you launch it. If safe clean is enabled (green light in the status bar), a backup file will be created automatically.
 * If the **Run Steam service automatic repair** is checked, SRC Repair will run built-in Steam service automatic recovery function.

.. index:: path checker, installation path checker
.. _cleanup-pathcheck:

Installation path checker
============================================

This tool will automatically check for restricted (non-ASCII) symbols in the Steam installation path. The Steam path should contain only latin letters and numbers. Anything else can cause major issues with games compiled without Unicode support.

If any restricted symbols were found, a warning message will be shown. Also you will see a red sign in the **General information** section.

This tool runs automatically along with SRC Repair and cannot be disabled.

.. index:: game cleanup, safe cleanup
.. _cleanup-wizard:

Game cleanup wizard
===============================================

This tool allows you to clean up the garbage that has accumulated in the game through regular daily use. This can free up a lot of disk space.

After selecting the cleanup action, the :ref:`safe clean window <modules-cleanup>` will appear. You can manually check the list of files marked for deletion.

If you don't want to remove a file, uncheck the checkbox next to its name. All marked files will be removed.

When done, click the **Execute cleanup** button to run the cleanup sequence. If you change your mind, click **Cancel**.

Currently supported by SRC Repair cleanup actions:

 * **custom maps** -- remove downloaded or installed custom maps. This will free up a lot of disk space and can speed up the game startup;
 * **download cache** -- remove downloaded from game servers data: personal sprays of all the players you have ever seen on the servers, various partially downloaded or damaged during download files, etc. Contains garbage. We recommend to run this cleanup at least once a week;
 * **custom directory** -- remove installed custom modifications from the ``/custom`` directory (only if supported by the game);
 * **custom sounds** -- remove sound files downloaded from game servers;
 * **FPS-configs** -- remove installed :ref:`FPS-configs <fps-about>`;
 * **graph and sound cache** -- remove sound and graphic caches created by the game. It can fix some issues. They will be automatically created the next time you start the game;
 * **secondary cache** -- remove downloaded from game servers cache files. We recommend to run this cleanup at least once a month;
 * **screenshots** -- remove screenshots from the ``/screenshots`` directory. Can free up a lot of disk space;
 * **recorded demos** -- remove recorded, downloaded or installed demo files (files \*.dem) from the game directory;
 * **models and textures** -- remove models and textures downloaded from game servers or installed by different custom modifications;
 * **deep cleanup** -- try to remove all garbage from the game at once. You **must** run the game :ref:`cache verification <cleanup-advanced>` after running this cleanup;
 * **replays** -- remove files created by the Replay system (only if supported by the game).

.. index:: deep cleanup, advanced cleanup
.. _cleanup-advanced:

Deep cleanup
============================================

 * **Clean game settings (+video)** -- reset all in-game video settings. A backup file will be created.
 * **Remove all binaries and launchers** -- remove the game binaries and launchers (contents of the ``/bin``, ``/{game}/bin`` directories and the ``/hl2.exe`` file). Use this if you have issues with starting the game, crashes of different origin, etc. You **must** validate the game cache after running this cleanup.
 * **Validate game cache files** -- force the game cache verification. This will check all the game data and re-download any corrupted or missing files. You must run this process after running the deep cleanup or removing the game binaries and launchers.
