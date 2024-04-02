..
    SPDX-FileCopyrightText: 2011-2024 EasyCoding Team

    SPDX-License-Identifier: GPL-3.0-or-later

.. _graphic-tweaker:

**********************************
Graphic tweaker
**********************************

Tools on this page allow to view or edit graphic settings of all supported Source Engine games without needed to launch them.

.. index:: source type 1 video settings
.. _gt-type1:

Graphic settings of Type 1 games
==========================================

Available settings:

  * **horisontal resolution** -- integer in range between *640* to *2000* pixels;
  * **vertical resolution** -- integer in range between *480* to *1900* pixels;
  * **display mode**: *fullscreen* or *windoweed*. Fullscreen is recommended in the most cases to in order get maximum performance;
  * **model quality**: *low*, *medium* or *high*. Highest value will require the most powerful GPU;
  * **texture quality**: *low*, *medium* or *high* (the same as models);
  * **shaders quality**: *low* or *high* -- set shader effects quality;
  * **water reflections**: *simple* (no water reflections at all), *reflect world* (balanced between quality and performance) or *reflect all* (maximum quality, all reflections will be enabled; will require high-end CPU and GPU);
  * **shadow quality**: *low* or *high* -- set shadow quality. Shadows can be disabled completely only by :ref:`FPS-config installation <fps-install>`;
  * **color correction**: *enabled* or *disabled* -- configure software color and gamma correction;
  * **antialiasing mode**: *disabled*, *2x*, *4x*, *8x* or *16x* -- set anti-aliasing (FSAA) quality level and its alghorithm: hardware (only GPU will be used) or software (both GPU and CPU; maximum quality with huge resource consumption);
  * **filtering mode**: *bilinear*, *trilinear* or *anisotropic* \*x (2x, 4x, 8x, 16x) -- set texture filtering level. The higher level, the most powerful GPU will be required. Optimal performance/quality ratio can be achieved with anisotropic 4x or 8x levels. Trilinear filtration is faster then bilinear or anisotropic;
  * **vertical synchronization**: *disabled* or *enabled* -- control vertical synchronization (frame limiter). When enabled, framerate will be limited by your monitor refresh rate and prevent screen tearing;
  * **motion blur**: *disabled* or *enabled* -- enable or disable motion blur post-effect;
  * **DirectX mode**: *8.0*, *8.1*, *9.0*, *9.0c* -- set DirectX feature level. 8.x will disable some effects and significantly increase performance. :ref:`Some games <faq-gm-dx8>` supports only 9.0c;
  * **High Dynamic Range**: *disabled* or *enabled* -- allow or disallow High Dynamic Range effects.

.. index:: source type 2 video settings
.. _gt-type2:

Graphic settings of Type 2 games
==========================================

Available settings:

  * **horisontal resolution** -- integer in range between *640* to *2000* pixels;
  * **vertical resolution** -- integer in range between *480* to *1900* pixels;
  * **aspect ratio** -- *normal (4:3)*, *widescreen (16:9)* or *widescreen (16:10)*;
  * **brightness** -- set in-game brightness level;
  * **shadow quality**: *low* or *high* -- set shadow quality. Shadows can be disabled completely only by :ref:`FPS-config installation <fps-install>`;
  * **motion blur**: *disabled* or *enabled* -- enable or disable motion blur post-effect;
  * **display mode** -- *fullscreen*, *windoweed*, *windoweed (no border)*;
  * **antialiasing mode**: *disabled*, *2x*, *4x*, *8x* or *16x* -- set anti-aliasing (FSAA) quality level and its alghorithm: hardware (only GPU will be used) or software (both GPU and CPU; maximum quality with huge resource consumption);
  * **filtering mode**: *bilinear*, *trilinear* or *anisotropic* \*x (2x, 4x, 8x, 16x) -- set texture filtering level. The higher level, the most powerful GPU will be required. Optimal performance/quality ratio can be achieved with anisotropic 4x or 8x levels. Trilinear filtration is faster then bilinear or anisotropic;
  * **wait for vertical sync**: *disabled*, *enabled (double buffered)* or *enabled (triple buffered)* -- control vertical synchronization (frame limiter). When enabled, framerate will be limited by your monitor refresh rate and prevent screen tearing;
  * **multicore rendeding** -- *disabled* or *enabled*. Allow to use all physical cores in multicore CPUs. Can cause crashes, but significantly increase framerate;
  * **shader detail**: *low*, *medium*, *high* or *very high* -- set shader effects quality;
  * **effect detail**: *low*, *medium*, or *high* -- set normal effects quality;
  * **paged memory pool** -- *low*, *medium*, *high* - set maximum size of game memory pool;
  * **model and texture detail**: *low*, *medium* or *high*. Highest value will require the most powerful GPU.

.. index:: launch options, game launch options
.. _gt-params:

Game launch options
=================================================

**Game launch options** allow user to override game launch options. This option provided for power users only. Most of users should not use it.

All parameters must be separated by spaces. For example:

.. code-block:: text

    -novid -full -h 1280 -w 1024

Warning! Do not use ``-dxlevel`` command-line option, because it will lead to the fact that the game video settings will no longer be saved correctly. This is a known issue of all Source Engine games.

.. index:: changing game launch options
.. _gt-setparams:

Changing game launch options
=================================================

Start Steam client -- select the game from Steam Library -- press right mouse button -- select **Properties** -- press **Set launch options** button -- set new launch options -- press **OK** and **Close** -- launch game.

.. index:: list of supported launch options
.. _gt-launchopts:

List of supported launch options
=================================================

Available launch options:

  * **-novid** (**-novideo**) -- disable game intro video;
  * **-autoconfig** -- restore default settings. The game will ignore all settings and :ref:`FPS-configs <fps-about>`;
  * **-full** (**-fullscreen**) -- start the game in fullscreen mode;
  * **-window** (**-sw** или **-windowed**) -- start the game in windowed mode;
  * **-width** или **-w** -- set horisontal resolution or window size;
  * **-height** или **-h** -- set vertical resolution or window size;
  * **-console** -- enable and automatically launch developer console;
  * **-textmode** -- start the game in the text mode;
  * **-dxlevel** -- enforce DirectX level (available values are *80*, *81*, *90*, *95*). Do not use it due to :ref:`known major issues <gt-params>`. Use :ref:`graphic tweaker <gt-type1>` instead;
  * **-heapsize X** -- set heap size. Can be calculated by the following formula: (RAM / 2 * 1024). Examples for popular RAM sizes:

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
  * **-freq X** (**-refresh X**) -- set monitor refresh rate for fullscreen mode;
  * **-nocrashdialog** -- disable built-in debugger. Will disable crash reports;
  * **-32bit** -- start game in legacy 32-bit mode when running under 64-bit operating system (x64);
  * **-dev** -- enable developer mode: game debug messages will be shown directly on HUD;
  * **-condebug** -- write contents of developer console to ``console.log`` file;
  * **-toconsole** -- force console mode even if ``+map`` parameter is used;
  * **-lv** -- enable Low Violence mode in Left 4 Dead (2);
  * **-sillygibs** -- enable Low Violence mode in Team Fortress 2.

.. index:: maximum quality profile
.. _gt-maxquality:

Video profiles: maximum quality
=================================================

**Maximum quality** button will set all graphic settings to recommended maximum:

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

**Maximium performance** button will set all graphic settings to recommended minimum:

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

If you want to reach maximum performance, you should install special :ref:`FPS-config <fps-configs>`.

.. index:: useful information about video profiles
.. _gt-other:

Other useful information about profiles
================================================

**Maximum quality** and **Maximium performance** will not automatically save video settings. You should check and save them manually by pressing **Save settings** button.

If safe clean is enabled (green light in status bar), backup will be created automatically. You can restore or delete it on :ref:`BackUps <backups-about>` tab.
