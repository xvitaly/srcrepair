..
    SPDX-FileCopyrightText: 2011-2025 EasyCoding Team

    SPDX-License-Identifier: GPL-3.0-or-later

.. _getting_started:

*******************************
Getting started
*******************************

.. index:: starting program, launching application
.. _gs-launch:

Starting program
==========================================

You can launch SRC Repair from the Start menu right after installation: **Start** -- **SRC Repair** -- **SRC Repair**.

.. index:: localization selection, language detection, program language
.. _gs-localization:

Localization selection
======================================

SRC Repair will automatically detect and use the default OS language if supported by the application.

.. index:: administrator rights, permissions, restricted modules, UAC
.. _gs-admin:

Admin permissions
==========================================

SRC Repair can be started with or without administrator rights.

We recommend running this program **without** administrator rights, however some advanced features will not be available:

  * :ref:`System buttons disabler <modules-kbd>`;

When you use them, SRC Repair will display the standard Windows UAC dialog and run only that module as an administrator for maximum security.

You can also run the application as an administrator. Right-click on the SRC Repair shortcut in the Start menu or on desktop, select **Run with administrator** and confirm this action.

After completing the necessary actions, please return to standard user rights. Staying as a super-user is not completely safe.

.. index:: startup actions
.. _gs-startup:

Startup actions
==========================================

The following actions will be performed on SRC Repair's startup:

  1. getting Steam installation directory path;
  2. checking if Steam installation directory exists;
  3. checking if required Steam configuration files exists and can be read;
  4. getting the list of supported games;
  5. checking for installed supported games;
  6. creating user-friendly GUI;
  7. working with updates, if enabled in :ref:`advanced settings <settings-advanced>`.

If the program cannot find Steam installation directory path in the Windows Registry (only on Microsoft Windows platform) or in known common locations (all other supported platforms), the **Find Steam directory** dialog will appear (but only once).

.. index:: working with application, using application
.. _gs-useapp:

Using application
==========================================

First you will need to select a game from the list of supported and installed Source Engine games. Your choise will be saved.

If only one game is found, it will be selected automatically.

If the program cannot find the required game, please launch it at least once from Steam and then restart SRC Repair.

.. index:: data files storage
.. _gs-datafiles:

Data files storage
==========================================

All program settings are stored in the ``%LOCALAPPDATA%\EasyCoding_Team`` directory (each subdirectory for every version).

Created by program :ref:`backups <backups-about>` can be found in the ``%APPDATA%\SRC Repair\backups`` directory.

All other data files -- ``%APPDATA%\SRC Repair``.

Logs -- ``%APPDATA%\SRC Repair\logs``.

.. index:: updating program, application updates
.. _gs-update:

Updating application
==========================================

You can :ref:`check for updates <modules-updater>` from the **Help** -- **Check for updates** menu.

SRC Repair will automatically check for new versions once a week (can be disabled in :ref:`advanced settings <settings-advanced>`).

.. index:: removing program, uninstalling program
.. _gs-uninstall:

Uninstalling program
==========================================

If you want to uninstall SRC Repair from your compuler, use **Start** -- **Settings** -- **Apps** -- **Apps & features** -- **SRC Repair** -- **Uninstall**.

Uninstaller will automatically remove all program files, shortcuts, registry entries, but will save created by user :ref:`data files <gs-datafiles>`. You can remove them manually.
