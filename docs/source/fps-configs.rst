..
    SPDX-FileCopyrightText: 2011-2024 EasyCoding Team

    SPDX-License-Identifier: GPL-3.0-or-later

.. _fps-configs:

*******************************
FPS-config manager
*******************************

.. index:: about FPS-configs
.. _fps-about:

About FPS configs
==========================================

On this page you can install, remove or update one of the available and tested FPS-configs into the selected game.

FPS-config is a text file named ``autoexec.cfg``. It can significantly improve game performance by changing hidden advanced video settings.

Settings obtained from FPS-config have higher priority than those set by the :ref:`Graphic tweaker <graphic-tweaker>` or by the game itself.

Select a config from the list to read its description.

Installed FPS-configs can be edited using the :ref:`Config editor <editor-working>`.

All FPS-configs are being downloaded from the cloud and licensed "as is".

.. index:: supported FPS-configs
.. _fps-available:

Supported FPS-configs
==========================================

The following FPS-configs are supported by current version of the application:

  * v1tsk_high_fps;
  * m0refps;
  * m0rehighfps;
  * skullhighfps;
  * bonhighfps;
  * chrishighfps;
  * chrismaxfps;
  * chrismaxquality (reverse);
  * chrisdx9frames;
  * comangliastability;
  * comangliamaxfps;
  * comangliacinema (reverse);
  * dt_legal;
  * mastercomfig-maxperf;
  * mastercomfig-quality (reverse);
  * rhaprody-perf.

.. index:: FPS-config installation
.. _fps-install:

FPS-config installation
==========================================

To install a FPS-config, select it from the list of available configs, press the **Install selected config** button and confirm this operation.

All required files will be downloaded and installed automatically.

If safe clean is enabled (green light in the status bar) and you already have ``autoexec.cfg`` file installed, the backup will be created automatically. You can restore or delete it on :ref:`BackUps <backups-about>` tab.

Some configs are not supported by games. In this case, you will see a warning message.

Do not install more than one FPS-config, as this may lead to unpredictable consequences.

.. index:: FPS-config uninstallation
.. _fps-uninstall:

FPS-config uninstallation
==========================================

To remove a FPS-config, select it from the list of available configs, press the **Remove selected config** (or the **Remove installed config**) button and confirm this operation.

If safe clean is enabled (green light in the status bar), the backup will be created automatically. You can restore or delete it on :ref:`BackUps <backups-about>` tab.

.. index:: known issues with FPS-configs, missing voice chat, missing sprays
.. _fps-troubleshooting:

Known issues with FPS-configs
==========================================

After installing FPS-configs voice chat can stop working, sprays and face animation can be disabled. This is conceived. Most of FPS-configs disable this functions in order to significantly increase game performance.

If you want to enable them again, you will need to :ref:`remove all installed FPS-configs <fps-uninstall>` and then change the following variables via the ``autoexec.cfg`` file or by using the developer console:

.. code-block:: text

    cl_allowdownload "1"
    cl_allowupload "1"
    cl_playerspraydisable "0"
    r_decals "200"
    r_spray_lifetime "2"
    sv_voiceenable "1"
    voice_modenable "1"
    voice_scale "1"
    voice_enable "1"
    r_eyes "1"

If you are using the developer console (can be accessed by pressing the **~** (tilde) button), remember to press **Enter** after entering each line.

.. index:: reverse FPS-configs, maximum quality configs
.. _fps-maxquality:

Reverse FPS-configs
==========================================

If you want to set all video settings to maximum to get the best quality, you can install specical :ref:`reverse FPS-configs <fps-available>`.

You will need a high-end computer in order to use them.
