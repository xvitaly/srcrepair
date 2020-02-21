.. _getting_started:

*******************************
Getting started
*******************************

.. index:: getting started, starting program, launching application
.. _gs-launch:

Starting program
==========================================

You can directly start SRC Repair from start menu after installation: **Start** - **Programs** - **SRC Repair** - **SRC Repair**.

SRC Repair will automatically detect and use system default language.


.. index:: administrator rights, permissions, restricted modules, UAC
.. _gs-admin:

Admin permissions
==========================================

We recommended to run SRC Repair **without** administrator user rights, but some advanced features will be unavailable:

  * :ref:`System buttons disabler <modules-kbd>`;

If you want to use them, you will need to run program with administrator user rights. Press right mouse button on SRC Repair's shortcut in start menu or on desktop, select **Run with administrator** and confirm this action in Windows UAC dialog.

After performing needed actions, please switch back to regular user. Staying as super-user is not completely safe.

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
  6. creating user-friendly GUI.

If program cannot find Steam installation directory path in registry (only on Microsoft Windows platform) or in known common locations, the **Find Steam directory** dialog will be shown (only once).

.. index:: getting started, working with application, using application
.. _gs-useapp:

Using application
==========================================

First you will need to select game from the list of supported and installed Source Engine games. Your choise will be saved.

If only one game will be found, it will be selected automatically.

If program cannot find required game, please start it at least once from Steam, then restart SRC Repair.

.. index:: data files storage
.. _gs-datafiles:

Data files storage
==========================================

All settings will be stored in ``%LOCALAPPDATA%\EasyCoding_Team`` directory (each subdirectory for every version).

Created by program :ref:`backups <backups-about>` can found in ``%APPDATA%\SRC Repair\backups`` directory.

All other data files -- ``%APPDATA%\SRC Repair``.

.. index:: updating program, application updates
.. _gs-update:

Updating application
==========================================

You can :ref:`check for updates <modules-updater>` from **Help** -- **Check for updates** menu.

SRC Repair will automatically check for new versions once a week (can be disabled in :ref:`advanced settings <settings-advanced>`).

.. index:: removing program, uninstalling program
.. _gs-uninstall:

Uninstalling program
==========================================

If you want to uninstall SRC Repair from your compuler, use **Control panel** -- **Programs and components** -- **SRC Repair** -- **Uninstall**.

Uninstaller will automatically remove all program files, shortcuts, registry entries, but will save created by user :ref:`data files <gs-datafiles>`. You can remove them manually.
