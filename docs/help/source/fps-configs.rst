.. _fps-configs:

*******************************
FPS-config manager
*******************************

.. index:: FPS-configs
.. _fps-about:

About FPS configs
==========================================

On this page you can install one of available and tested FPS-configs into selected game. FPS-config is a text file with ``autoexec.cfg`` name. It can significantly increase game performance by changing extended video settings.

Settings, obtained from FPS-config, have higher priority than set by :ref:`Graphic tweaker <graphic-tweaker>` or by the game itself. Select config from list to read it's description.

Installed FPS-configs can be edited in :ref:`Config editor <editor-working>`.

All FPS-configs are being downloaded from our cloud and provided by "as is" license.

.. index:: FPS-configs, supported FPS-configs
.. _fps-available:

Supported FPS-configs
==========================================

The following FPS-configs are supported by current version of application:

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

.. index:: FPS-configs, FPS-config installation
.. _fps-install:

FPS-config installation
==========================================

To install FPS-config, select it from the list of available configs, press **Install selected config** button and confirm this operation.

If safe clean is enabled (green light in status bar) and you already has installed ``autoexec.cfg`` file, backup will be created automatically. You can restore or delete in on :ref:`BackUps <backups-about>` tab.

Some configs are not supported by games. You will see warning message if so.

Do not install multiple FPS-configs to game. It will cause unpredictable consequences.

.. index:: FPS-configs, FPS-config uninstallation
.. _fps-uninstall:

FPS-config uninstallation
==========================================

To remove FPS-config, select it from the list of available configs, press **Remove selected config** (or **Remove installed config**) button and confirm this operation.

If safe clean is enabled (green light in status bar), backup will be created automatically. You can restore or delete in on :ref:`BackUps <backups-about>` tab.

.. index:: FPS-configs, FPS-configs known issues, missing voice chat, missing sprays
.. _fps-troubleshooting:

Known issues with FPS-configs
==========================================

After installing FPS-configs voice chat can stop working, sprays and face animation can be disabled. This is conceived. Most of FPS-configs disable this functions in order to significantly increase game performance.

If you want to enable them again, you will need to :ref:`remove all installed FPS-configs <fps-uninstall>` and then change following variables via ``autoexec.cfg`` file or developer console:

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

If you use developer console (can be called by pressing **~** (tilde) button), don't forget to press **Enter** after entering each row.

.. index:: FPS-configs, reverse FPS-configs, maximum quality configs
.. _fps-maxquality:

Reverse FPS-configs
==========================================

If you want to set all video settings to maxium to get best quality, you can install specical :ref:`reverse FPS-configs <fps-available>`.

You will need a high-end computer to use them.
