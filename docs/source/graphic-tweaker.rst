..
    SPDX-FileCopyrightText: 2011-2025 EasyCoding Team

    SPDX-License-Identifier: GPL-3.0-or-later

.. _graphic-tweaker:

**********************************
Graphic tweaker
**********************************

.. index:: about graphic tweaker
.. _gt-about:

About graphic tweaker
==========================================

On this page you can view or edit graphic settings of all supported games without launching them.

.. index:: source type 1 video settings
.. _gt-type1:

Graphic settings of Type 1 games
==========================================

Available settings:

  * **horisontal resolution** -- an integer ranging from *640* to *2000* pixels;
  * **vertical resolution** -- an integer ranging from *480* to *1900* pixels;
  * **display mode**: *fullscreen* or *windoweed*. In most cases, it is recommended to use fullscreen mode for maximum performance;
  * **model quality**: *low*, *medium* or *high*. At the maximum value, the most powerful GPU will be required;
  * **texture quality**: *low*, *medium* or *high* (the same as models);
  * **shaders quality**: *low* or *high* -- set the quality of shader effects;
  * **water reflections**: *simple* (no water reflections at all), *reflect world* (balanced between quality and performance) or *reflect all* (maximum quality, all reflections will be enabled; requires a high-end CPU and GPU);
  * **shadow quality**: *low* or *high* -- set shadow quality. The shadows can be disabled completely only by installing the :ref:`FPS-config <fps-install>`;
  * **color correction**: *enabled* or *disabled* -- set color and gamma correction;
  * **antialiasing mode**: *disabled*, *2x*, *4x*, *8x* or *16x* -- set the anti-aliasing quality level (FSAA) and its algorithm: hardware (only GPU will be used) or software (both GPU and CPU; maximum quality with huge resource consumption);
  * **filtering mode**: *bilinear*, *trilinear* or *anisotropic* \*x (2x, 4x, 8x, 16x) -- set texture filtering level. The higher the level, the more powerful GPU will be required. The optimal balance of performance and quality can be achieved using anisotropic levels of 4x or 8x. Trilinear filtration is faster then bilinear or anisotropic;
  * **vertical synchronization**: *disabled* or *enabled* -- control vertical synchronization (frame limiter). If enabled, the frame rate will be limited by your monitor's refresh rate and will prevent screen tearing;
  * **motion blur**: *disabled* or *enabled* -- enable or disable motion blur post-effect;
  * **DirectX mode**: *8.0*, *8.1*, *9.0*, *9.0c* -- set DirectX feature level. 8.x will disable some effects and significantly increase performance. :ref:`Some games <faq-gm-dx8>` supports only 9.0c;
  * **High Dynamic Range**: *disabled* or *enabled* -- allow or disallow High Dynamic Range effects.

.. index:: source type 2 video settings
.. _gt-type2:

Graphic settings of Type 2 games
==========================================

Available settings:

  * **horisontal resolution** -- an integer ranging from *640* to *2000* pixels;
  * **vertical resolution** -- an integer ranging from *480* to *1900* pixels;
  * **aspect ratio** -- *normal (4:3)*, *widescreen (16:9)* or *widescreen (16:10)*;
  * **brightness** -- set in-game brightness level;
  * **shadow quality**: *low* or *high* -- set shadow quality. The shadows can be disabled completely only by installing the :ref:`FPS-config <fps-install>`;
  * **motion blur**: *disabled* or *enabled* -- enable or disable motion blur post-effect;
  * **display mode** -- *fullscreen*, *windoweed*, *windoweed (no border)*;
  * **antialiasing mode**: *disabled*, *2x*, *4x*, *8x* or *16x* -- set the anti-aliasing quality level (FSAA) and its algorithm: hardware (only GPU will be used) or software (both GPU and CPU; maximum quality with huge resource consumption);
  * **filtering mode**: *bilinear*, *trilinear* or *anisotropic* \*x (2x, 4x, 8x, 16x) -- set texture filtering level. The higher the level, the more powerful GPU will be required. The optimal balance of performance and quality can be achieved using anisotropic levels of 4x or 8x. Trilinear filtration is faster then bilinear or anisotropic;
  * **wait for vertical sync**: *disabled*, *enabled (double buffered)* or *enabled (triple buffered)* -- control vertical synchronization (frame limiter). If enabled, the frame rate will be limited by your monitor's refresh rate and will prevent screen tearing;
  * **multicore rendeding** -- *disabled* or *enabled*. Allow all physical cores to be used in multi-core processors. May cause crashes, but significantly increases frame rate;
  * **shader detail**: *low*, *medium*, *high* or *very high* -- set the quality of shader effects;
  * **effect detail**: *low*, *medium*, or *high* -- set normal effects quality;
  * **paged memory pool** -- *low*, *medium*, *high* - set the maximum size of the game memory pool;
  * **model and texture detail**: *low*, *medium* or *high*. The higher the level, the more powerful GPU will be required.

.. index:: launch options, game launch options
.. _gt-params:

Game launch options
=================================================

**Game launch options** allow the user to override game launch options. This feature is available for advanced users only. Most users should not use it.

All parameters must be separated by spaces.

Example:

.. code-block:: text

    -novid -full -h 1280 -w 1024

Warning! Do not use the ``-dxlevel`` command line option as this will cause the game video settings to no longer be saved correctly. This is a known common issue of all Source Engine games.

.. index:: changing game launch options
.. _gt-setparams:

Changing game launch options
=================================================

Start the Steam client -- select the game from the Steam Library -- press right mouse button -- select **Properties** -- press the **Set launch options** button -- set new launch options -- press **OK** and **Close** -- launch the game.

.. index:: list of supported launch options
.. _gt-launchopts:

List of supported launch options
=================================================

Available launch options:

  * **-novid** (**-novideo**) -- disable game intro video;
  * **-autoconfig** -- restore default settings. The game will ignore all settings and installed :ref:`FPS-configs <fps-about>`;
  * **-full** (**-fullscreen**) -- start the game in fullscreen mode;
  * **-window** (**-sw** или **-windowed**) -- start the game in windowed mode;
  * **-width** или **-w** -- set horizontal resolution or window size;
  * **-height** или **-h** -- set vertical resolution or window size;
  * **-console** -- enable and automatically launch the developer console;
  * **-textmode** -- start the game in the text mode;
  * **-dxlevel** -- enforce DirectX level (available values are *80*, *81*, *90*, *95*). Do not use it due to :ref:`known major issues <gt-params>`. Use :ref:`graphic tweaker <gt-type1>` instead;
  * **-heapsize X** -- set heap size. Can be calculated using the following formula: (RAM / 2 * 1024). Examples for popular RAM sizes:

    * 512 MB -> **262144**;
    * 1 GB -> **524288**;
    * 2 GB -> **1048576**;
    * 3 GB -> **1572864**;
    * 4 GB -> **2097152**;

  * **-nojoy** -- disable gamepads initialization;
  * **-noipx** -- disable IPX connections support;
  * **-noborder** -- disable window border and controls in windowed mode;
  * **-noforcemspd** -- use mouse speed settings from Windows;
  * **-noforcemparms** -- use mouse buttons settings from Windows;
  * **-noforcemaccel** -- use mouse acceleration settings from Windows;
  * **-freq X** (**-refresh X**) -- set the monitor refresh rate for fullscreen mode;
  * **-nocrashdialog** -- disable built-in debugger. Will disable crash reports;
  * **-32bit** -- start the game in legacy 32-bit mode when running on a 64-bit operating system (x64);
  * **-dev** -- enable developer mode: game debug messages will be displayed directly on the HUD;
  * **-condebug** -- write the contents of the developer console to the ``console.log`` file;
  * **-toconsole** -- force console mode even if the ``+map`` parameter is used;
  * **-lv** -- enable Low Violence mode in Left 4 Dead (2);
  * **-sillygibs** -- enable Low Violence mode in Team Fortress 2.

.. index:: maximum quality profile
.. _gt-maxquality:

Video profiles: maximum quality
=================================================

The **Maximum quality** button will set all graphic settings to recommended maximum:

 * **display mode** -> fullscreen;
 * **model quality** -> high;
 * **texture quality** -> high;
 * **shader quality** -> high;
 * **water reflections** -> reflect world;
 * **shadow quality** -> high;
 * **color correction** -> enabled;
 * **antialiasing** -> disabled;
 * **filtering** -> anisotropic 4x;
 * **vertical synchronization** -> disabled;
 * **motion blur** -> disabled;
 * **DirectX mode** -> 9.0c;
 * **High Dynamic Range** -> full.

.. index:: maximum performance profile
.. _gt-maxfps:

Video profiles: maximum performance
===================================================

The **Maximum performance** button will set all graphic settings to recommended minimum:

 * **display mode** -> fullscreen;
 * **model quality** -> low;
 * **texture quality** -> low;
 * **shader quality** -> low;
 * **water reflections** -> simple;
 * **shadow quality** -> low;
 * **color correction** -> disabled;
 * **antialiasing** -> disabled;
 * **filtering** -> trilinear;
 * **vertical synchronization** -> disabled;
 * **motion blur** -> disabled;
 * **DirectX mode** -> will ask user to enable 8.0;
 * **High Dynamic Range** -> disabled.

If you want to reach maximum performance, you should install a special :ref:`FPS-config <fps-configs>`.

.. index:: useful information about video profiles
.. _gt-other:

Other useful information about profiles
================================================

**Maximum quality** and **Maximum performance** will not automatically save video settings. You should check and save them manually by clicking the **Save settings** button.

If safe clean is enabled (green light in the status bar), the backup will be created automatically. You can restore or delete it on :ref:`BackUps <backups-about>` tab.
