..
    SPDX-FileCopyrightText: 2011-2025 EasyCoding Team

    SPDX-License-Identifier: GPL-3.0-or-later

.. _development:

**********************************
Development
**********************************

.. index:: building from sources
.. _building-from-sources:

Building from sources
==========================================

There are two supported ways to build application from sources:

  * fully automatic build;
  * manual build.

Preparing to build
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

  1. Clone this repository or download the source tarball from the `releases page <https://github.com/xvitaly/srcrepair/releases>`_.
  2. Install pre-requirements.

Installing pre-requirements
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

First you will need to install C# complier, Microsoft .NET Framework SDK and other required tools (all steps are mandatory):

  1. download the `Microsoft Visual Studio 2022 Community <https://visualstudio.microsoft.com/vs/community/>`_ installer and run it;
  2. select the **Microsoft Visual Studio 2022 Community**, enable the **Classic .NET application development** component, then switch to the **Additional components** tab and enable the **NuGet package manager**;
  3. install the Microsoft Visual Studio 2022 Community;
  4. download the latest version of `NuGet CLI <https://www.nuget.org/downloads>`_ and copy its executable to any directory, located in the ``%PATH%`` environment variable;
  5. download and install HTML Help Workshop from any trusted source;
  6. download and install `Gpg4Win <https://www.gpg4win.org/>`_ to default directory;
  7. download and install `Python 3 for Windows <https://www.python.org/downloads/windows/>`_;
  8. open terminal and install Sphinx-doc using PIP: ``pip3 install sphinx``;
  9. download and install `7-Zip for Windows <https://www.7-zip.org/download.html>`_ to default directory;
  10. download and install `InnoSetup <https://jrsoftware.org/isdl.php>`_ to default directory.

Automatic build
^^^^^^^^^^^^^^^^^^^^^^^^^

If you want to use automatic build, follow these steps:

  1. install pre-requirements;
  2. double click on the ``packaging/build_win.cmd`` script.

You will find results in the ``packaging/results`` directory.

Manual build
^^^^^^^^^^^^^^^^^^^^^^^^^

If you don't want to use automatic method, you can build this project manually.

Building main project
++++++++++++++++++++++++++++++++

  1. Start the Microsoft Visual Studio 2022 Community.
  2. **File** -- **Open** -- **Project or solution**, select ``srcrepair.sln``, then press **Open** button.
  3. **Tools** -- **NuGet Package Manager** -- **Manage NuGet packages for Solution**, press **Restore** button.
  4. **Build** -- **Configuration manager** -- **Active solution configuration** -- **Release**, then press **Close** button.
  5. **Build** -- **Build solution**.

You will find results in the ``src/srcrepair/bin/Release`` directory.

Building installer
+++++++++++++++++++++++++++++++

  1. Run InnoSetup Compiler.
  2. Open the ``packaging/inno/srcrepair.iss`` file.
  3. **Build** -- **Compile**.

If the InnoSetup will complain about missing ``*.sig`` files, you will need to manually sign (use detached signatures) compiled binaries with GnuPG by using Gpg4Win, or remove these rows from the InnoSetup script.

You will find results in the ``packaging/results`` directory.

.. index:: cleanup database
.. _cleanup-database:

Cleanup database documentation
==========================================

The original source file is located in ``assets/cleanup.xml``.

XML database example
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8"?>
    <Targets>
        <Target>
            <ID>0</ID>
            <Name>Example entry</Name>
            <Directories>
                <Directory>
                    <Path>$GamePath$/foo</Path>
                    <Extension>*.*</Extension>
                    <Recursive>1</Recursive>
                    <CleanEmpty>1</CleanEmpty>
                    <Safe>1</Safe>
                </Directory>
                <Directory>
                    <Path>$FullGamePath$/bar</Path>
                    <Extension>*.*</Extension>
                    <Recursive>0</Recursive>
                    <CleanEmpty>0</CleanEmpty>
                    <Safe>0</Safe>
                </Directory>
            </Directories>
        </Target>
    </Targets>

Properties
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Level 0:

  * ``Targets`` -- XML root element.

Level 1:

  * ``Target`` -- database entry base element.

Level 2:

  * ``ID`` -- unique identifier (integer, starting from ``0``);
  * ``Name`` -- user-friendly name;
  * ``Directories`` -- the list of directories for cleanup.

Level 3:

  * ``Directory`` -- a single cleanup entry with additional parameters.

Level 4:

  * ``Path`` -- path to a single directory (with templates support);
  * ``Extension`` -- file mask or a file name;
  * ``Recursive`` -- allow or disallow the recursive cleanup;
  * ``CleanEmpty`` -- allow or disallow the empty directories deletion after cleanup;
  * ``Safe`` -- safety class:

    * ``1`` -- this directory can be safely cleaned;
    * ``0`` -- cleaning up this directory may be dangerous (can be allowed in settings).

Directories
^^^^^^^^^^^^^^^^^^^^^^^^^^

Each path must not contain a file mask or a trailing directory separator character. All matched files will be marked to deletion.

Use ``*.*`` to mark all files in a specified directory.

Templates
^^^^^^^^^^^^^^^^^^^^^^^^^^

Available templates (can be used within the ``Directory`` property):

  * ``$GamePath$`` -- will be replaced by the ``SelectedGame.GamePath``;
  * ``$FullGamePath$`` -- will be replaced by the ``SelectedGame.FullGamePath``;
  * ``$AppWorkshopDir$`` -- will be replaced by the ``SelectedGame.AppWorkshopDir``;
  * ``$CloudScreenshotsPath$`` -- will be replaced by the ``SelectedGame.CloudScreenshotsPath``;
  * ``/`` -- will be replaced by the correct trailing path directory separator character, depending on running platform.

Multiple templates are supported in a single entry.

.. index:: FPS-configs database
.. _configs-database:

FPS-configs database documentation
================================================

The original source file is located in ``assets/configs.xml``.

XML database example
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8"?>
    <Configs>
        <Config>
            <Name>foo-bar</Name>
            <URI>https://example.org/foo-bar.zip</URI>
            <Mirror>https://example.com/foo-bar.zip</Mirror>
            <SupportedGames>240;440</SupportedGames>
            <ru>
            <![CDATA[Description in Russian.]]>
            </ru>
            <en>
            <![CDATA[Description in English.]]>
            </en>
            <ArchiveDir>foo-bar</ArchiveDir>
            <InstallDir>foo-bar</InstallDir>
            <Hash2>SHA-512</Hash2>
        </Config>
    </Configs>

Properties
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Level 0:

  * ``Configs`` -- XML root element.

Level 1:

  * ``Config`` -- database entry base element.

Level 2:

  * ``Name`` -- FPS-config user-friendly name;
  * ``URI`` -- fully-qualified Zip archive download URL (safe redirects (3XX HTTP codes) are allowed);
  * ``Mirror`` -- fully-qualified Zip archive download mirror URL (safe redirects (3XX HTTP codes) are allowed);
  * ``SupportedGames`` -- the list of supported game IDs, separated by commas;
  * ``ru`` -- user-friendly description in Russian (CDATA escaping is required);
  * ``en`` -- user-friendly description in English (CDATA escaping is required);
  * ``ArchiveDir`` -- directory name in the archive (subdirectories are supported (use ``/`` symbol));
  * ``InstallDir`` -- installation directory name;
  * ``Hash2`` -- download file SHA2 (SHA-512) hash, speficied in the ``URI``.

.. index:: games database
.. _games-database:

Games database documentation
================================================

The original source file is located in ``assets/games.xml``.

XML database example
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8"?>
    <Games>
        <Game Name="Team Fortress 2">
            <Enabled>1</Enabled>
            <HasVF>0</HasVF>
            <DirName>Team Fortress 2</DirName>
            <SmallName>tf</SmallName>
            <VFDir>tf</VFDir>
            <UserDir>1</UserDir>
            <HUDsAvail>1</HUDsAvail>
            <SID>440</SID>
            <SVer>1</SVer>
            <Executable>hl2.exe</Executable>
        </Game>
    </Games>

Properties
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Level 0:

  * ``Games`` -- XML root element.

Level 1:

  * ``Game`` -- database entry base element:

    * ``Name`` -- user-friendly game name.

Level 2:

  * ``Enabled`` -- ``1`` if the current game is enabled or ``0`` -- if don't;
  * ``DirName`` -- game installation directory in ``SteamApps/common``;
  * ``SmallName`` -- game subdirectory name in ``SteamApps/common/$DirName``;
  * ``VFDir`` -- directory (or registry key) name of video settings storage;
  * ``UserDir`` -- ``1`` if the current game support custom user stuff, located in ``custom`` directory or ``0`` -- if don't;
  * ``HUDsAvail`` -- ``1`` if the current game support custom HUDs or ``0`` -- if don't;
  * ``SID`` -- Steam database internal ID for the current game;
  * ``SVer`` -- Source Engine version:

    * ``1`` -- Source Engine 1, Type 1 (use Windows registry to store video settings);
    * ``2`` -- Source Engine 1, Type 2 (use ``video.txt`` file to store video settings);
    * ``3`` -- Source Engine 2, generic (not yet implemented; reserved for future use);
    * ``4`` -- Source Engine 1, Type 4 (same as Type 1, but store video settings in ``videoconfig.cfg`` file);

  * ``Executable`` -- game executable (only for Windows). A single file name or a list for each supported architecture: ``x86`` and ``x64``.

.. index:: huds database
.. _huds-database:

HUDs database documentation
================================================

The original source file is located in ``assets/huds.xml``.

XML database example
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8"?>
    <HUDs>
        <HUD>
            <Name>7HUD</Name>
            <Game>tf</Game>
            <IsUpdated>1</IsUpdated>
            <URI>https://sourceforge.net/projects/srcrepair/files/huds/7hud/7hud_28903d1b.zip</URI>
            <Mirror>https://www.team-fortress.su/downloads/huds/7hud/7hud_28903d1b.zip</Mirror>
            <UpURI>https://github.com/Sevin7/7HUD/archive/master.zip</UpURI>
            <Preview>https://www.easycoding.org/files/srcrepair/huds/7hud.jpg</Preview>
            <RepoPath>https://github.com/Sevin7/7HUD</RepoPath>
            <Hash2>2c35b35d3e58dc75f3bc40134c4d137353d994d6dcc879e3edc35b837cbe2ae91cda0b2f698741fda17111a4543b7a002534b609de720e5125655d5b23e65217</Hash2>
            <LastUpdate>1572411245</LastUpdate>
            <Site>https://huds.tf/forum/showthread.php?tid=261</Site>
            <ArchiveDir>7HUD-master</ArchiveDir>
            <InstallDir>7hud</InstallDir>
        </HUD>
    </HUDs>

Properties
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Level 0:

  * ``HUDs`` -- XML root element.

Level 1:

  * ``HUD`` -- database entry base element.

Level 2:

  * ``Name`` -- HUD user-friendly name;
  * ``Game`` -- short name of the supported by this HUD game (``SmallName`` from :ref:`games database <games-database>`);
  * ``IsUpdated`` -- ``1`` if HUD supports the latest version of the game, ``0`` -- if don't;
  * ``URI`` -- fully-qualified Zip archive download URL (safe redirects (3XX HTTP codes) are allowed);
  * ``Mirror`` -- fully-qualified Zip archive download mirror URL (safe redirects (3XX HTTP codes) are allowed);
  * ``UpURI`` -- upstream download archive URL (safe redirects (3XX HTTP codes) are allowed);
  * ``Preview`` -- screenshot of the game with this HUD or any custom image (JPEG and PNG formats are supported);
  * ``RepoPath`` -- GitHub repository URL or ``null``;
  * ``Hash2`` -- download file SHA2 (SHA-512) hash, speficied in the ``URI``;
  * ``LastUpdate`` -- HUD last update time in Unix timestamp format;
  * ``Site`` -- website or homepage URL;
  * ``ArchiveDir`` -- directory name in the archive (subdirectories are supported (use ``/`` symbol));
  * ``InstallDir`` -- installation directory name.

.. index:: updates database
.. _updates-database:

Updates database documentation
================================================

The original source file is located in ``assets/updates.xml``.

Properties
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Level 0:

  * ``Updates`` -- XML root element.

Level 1:

  * ``Application`` -- sub-element with application update metadata.

Level 2:

  * ``Version`` -- application or database version;
  * ``URL`` -- direct download URL (no redirects are allowed);
  * ``Info`` -- changelog URL;
  * ``Hash2`` -- download file SHA2 (SHA-512) hash.

.. index:: plugins database
.. _plugins-database:

Plugins database documentation
================================================

The original source file is located in ``assets/plugins.xml``.

XML database example
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8"?>
    <Plugins>
        <Plugin>
            <Name>KB Helper</Name>
            <IntName>kbhelper</IntName>
            <ExeName>kbhelper.exe</ExeName>
            <ElevationRequired>1</ElevationRequired>
        </Plugin>
    </Plugins>

Properties
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Level 0:

  * ``Plugins`` -- XML root element.

Level 1:

  * ``Plugin`` -- database entry base element.

Level 2:

  * ``Name`` -- plugin user-friendly name;
  * ``IntName`` -- internal name for different actions;
  * ``ExeName`` -- executable file name;
  * ``ElevationRequired`` -- ``1`` if the plugin need a local administrator rights in order to run, ``0`` -- if don't.
