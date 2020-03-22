.. This file is a part of SRC Repair project. For more information
.. visit official site: https://www.easycoding.org/projects/srcrepair
..
.. Copyright (c) 2011 - 2020 EasyCoding Team (ECTeam).
.. Copyright (c) 2005 - 2020 EasyCoding Team.
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
.. _development:

**********************************
Development
**********************************

.. index:: development, building from sources
.. _building-from-sources:

Building from sources
==========================================

There are two supported ways to build application from sources:

  * fully automatic build;
  * manual build.

Preparing to build
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

  1. Clone this repository or download a source tarball from `releases page <https://github.com/xvitaly/srcrepair/releases>`_.
  2. Install pre-requirements.

Installing pre-requirements
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

First you will need to install C# complier, Microsoft .NET Framework SDK and other required tools (all steps are mandatory):

  1. download `Microsoft Visual Studio 2019 Community <https://visualstudio.microsoft.com/vs/community/>`_ installer and run it;
  2. select **Microsoft Visual Studio 2019 Community**, enable **Classic .NET application development** component, then switch to **Additional components** tab and enable **NuGet package manager**;
  3. install Microsoft Visual Studio 2019 Community;
  4. download the latest version of `NuGet CLI <https://www.nuget.org/downloads>`_ and copy executable to any directory, located in ``%PATH%`` environment variable;
  5. install `Doxygen for Windows <http://www.doxygen.nl/download.html>`_ to a default directory;
  6. download and install `HTML Help Workshop <https://www.microsoft.com/en-us/download/details.aspx?id=21138>`_;
  7. download and install `Gpg4Win <https://www.gpg4win.org/>`_ to a default directory;
  8. download and install `Python 3.7 for Windows <https://www.python.org/downloads/windows/>`_;
  9. open terminal and install Sphinx-doc using PIP: ``pip3 install sphinx``;
  10. download and install `7-Zip for Windows <https://www.7-zip.org/download.html>`_ to a default directory;
  11. download and install `InnoSetup 6 <http://www.jrsoftware.org/isdl.php>`_ to a default directory.

Automatic build
^^^^^^^^^^^^^^^^^^^^^^^^^

If you want to use automatic build, follow this steps:

  1. install pre-requirements;
  2. double click on ``packaging/build_win.cmd``.

You will find results in a ``packaging/results`` directory.

Manual build
^^^^^^^^^^^^^^^^^^^^^^^^^

If you don't want to use automatic method, you can build this project manually.

Building main project
++++++++++++++++++++++++++++++++

  1. Start Microsoft Visual Studio 2019 Community.
  2. **File** -- **Open** -- **Project or solution**, select ``srcrepair.sln``, then press **Open** button.
  3. **Tools** -- **NuGet Package Manager** -- **Manage NuGet packages for Solution**, press **Restore** button.
  4. **Build** -- **Configuration manager** -- **Active solution configuration** -- **Release**, then press **Close** button.
  5. **Build** -- **Build solution**.

You will find results in a ``srcrepair/bin/Release`` directory.

Building installer
+++++++++++++++++++++++++++++++

  1. Run InnoSetup Compiler.
  2. Open file ``packaging/inno/srcrepair.iss``.
  3. **Build** -- **Compile**.

If InnoSetup will complain about missing ``*.sig`` files, you will need to manually sign (use detached signatures) compiled binaries with GnuPG by using Gpg4Win, or remove this rows from InnoSetup script.

You will find results in a ``packaging/results`` directory.

.. index:: development, cleanup, database, cleanup database
.. _cleanup-database:

Cleanup database documentation
==========================================

Original source file located at ``assets/cleanup.xml`` of current repository.

XML database example
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8"?>
    <Targets>
        <Target>
            <ID>1</ID>
            <Name>Example entry</Name>
            <Directories>
                <Directory Class="Safe">$GamePath$/foo/*.*</Directory>
                <Directory Class="Unsafe">$FullGamePath$/bar/*.*</Directory>
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

  * ``ID`` -- unique identifier (integer, starting from 1);
  * ``Name`` -- user-friendly name;
  * ``Directories`` -- list of directories.

Level 3:

  * ``Directory`` -- path to a single directory with templates support:

    * ``Safe`` -- this directory can be safely cleaned;
    * ``Unsafe`` -- cleaning up this directory may be dangerous (can be disabled in settings).

Directories
^^^^^^^^^^^^^^^^^^^^^^^^^^

Each path must ends with a file mask. All matched files will be marked to deletion.

Use ``*.*`` to mark all files in a specified directory.

Templates
^^^^^^^^^^^^^^^^^^^^^^^^^^

Available templates (can be used within ``Directory`` property):

  * ``$GamePath$`` -- will be replaced by ``SelectedGame.GamePath``;
  * ``$FullGamePath$`` -- will be replaced by ``SelectedGame.FullGamePath``;
  * ``$AppWorkshopDir$`` -- will be replaced by ``SelectedGame.AppWorkshopDir``;
  * ``$CloudScreenshotsPath$`` -- will be replaced by ``SelectedGame.CloudScreenshotsPath``;
  * ``\`` -- will be replaced by correct trailing path directory separator character, depending on running platform.

Multiple templates are supported in a single entry.

.. index:: development, FPS-configs, database, FPS-configs database
.. _configs-database:

FPS-configs database documentation
================================================

Original source file located at ``assets/configs.xml`` of current repository.

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

  * ``Name`` -- user-friendly name of FPS-config;
  * ``URI`` -- fully-qualified Zip archive download URL (safe redirects (3XX HTTP codes) are allowed);
  * ``Mirror`` -- fully-qualified Zip archive download mirror URL (safe redirects (3XX HTTP codes) are allowed);
  * ``SupportedGames`` -- comma-separated list of supported game IDs;
  * ``ru`` -- user-friendly description in Russian (CDATA escaping is required);
  * ``en`` -- user-friendly description in English (CDATA escaping is required);
  * ``ArchiveDir`` -- name of directory in archive (subdirectories are supported (use ``/`` symbol));
  * ``InstallDir`` -- installation directory name;
  * ``Hash2`` -- SHA2 (SHA-512) hash of download file, speficied in ``URI``.

.. index:: development, games, database, games database
.. _games-database:

Games database documentation
================================================

Original source file located at ``assets/games.xml`` of current repository.

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

  * ``Enabled`` -- ``1`` if current game is enabled or ``0`` -- if don't;
  * ``HasVF`` -- ``1`` if current game use file to store video settings or ``0`` if current game use Windows registry to store video settings (deprecated);
  * ``DirName`` -- game installation directory in ``SteamApps/common``;
  * ``SmallName`` -- game subdirectory name in ``SteamApps/common/$DirName``;
  * ``VFDir`` -- directory (or registry key) name of video settings storage;
  * ``UserDir`` -- ``1`` if current game supports custom user stuff, located in ``custom`` directory or ``0`` -- if don't;
  * ``HUDsAvail`` -- ``1`` if current game supports custom HUDs or ``0`` -- if don't;
  * ``SID`` -- Steam database game internal ID;
  * ``SVer`` -- Source Engine version:

    * ``1`` -- Source Engine 1, Type 1 (use Windows registry to store video settings);
    * ``2`` -- Source Engine 1, Type 2 (use ``video.txt`` file to store video settings);
    * ``3`` -- Source Engine 2, generic (not yet implemented; reserved for future use);
    * ``4`` -- Source Engine 1, Type 4 (same as Type 1, but store video settings in ``videoconfig.cfg`` file);

  * ``Executable`` -- game executable file name (only for Windows).
