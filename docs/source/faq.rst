..
    SPDX-FileCopyrightText: 2011-2024 EasyCoding Team

    SPDX-License-Identifier: GPL-3.0-or-later

.. _faq:

*****************************************
Frequently asked questions (FAQ)
*****************************************

.. index:: dotnet framework version
.. _faq-framework:

How to fix an error with Microsoft .NET Framework 4 on program startup?
=================================================================================

You need to download and install Microsoft .NET Framework version 4.8 or later in order to use this program.

You can download if from the `official website <https://dotnet.microsoft.com/en-us/download/dotnet-framework>`__.

.. index:: slow startup
.. _faq-slow-start:

How to fix slow startup?
============================

You can install SRC Repair for all users. The installer will use NGen to generate native images, which should significantly improve cold startup speed.

.. index:: cracked game versions
.. _faq-crrrr:

Can I use this program with cracked version of the game?
============================================================

No. We only support genuine Steam versions.

.. index:: source 2 support
.. _faq-source2:

Will the Source 2 engine be supported and when?
===================================================

Maybe, but no ETA yet.

.. index:: steam account security
.. _faq-password:

Can this program steal my Steam account?
============================================

No. SRC Repair is a free and opensource program. You can check its `source code <https://github.com/xvitaly/srcrepair>`__.

.. index:: simultaneous launch with steam
.. _faq-steam-run:

Is it safe to run this program with Steam simultaneously?
=============================================================

Yes, unless you want use the :ref:`Steam cleanup module <modules-stmcln>`.

.. index:: simultaneous launch with the game
.. _faq-game-run:

Is it safe to run this program with controlled game simultaneously?
=======================================================================

We strongly recommend against doing this, as neither SRC Repair nor the managed game can gain exclusive access to files and settings.

.. index:: vac ban
.. _faq-vac:

Can I get VAC ban for using this program?
==============================================

No, but do not use this program with any running VAC protected game.

.. index:: report a bug, feature request
.. _faq-opinion:

How I can report bug or create a new feature request?
==========================================================

Please open a new issue in our `bug tracker <https://github.com/xvitaly/srcrepair/issues>`__.

.. index:: network access
.. _faq-firewall:

For what purposes does SRC Repair require Internet access?
==============================================================

SRC Repair need Internet access for checking for updates once a week (can be disabled in :ref:`advanced settings <settings-advanced>`).

.. index:: adding a new game
.. _faq-add-game:

I want to add a new game. What should I do?
===============================================

Please open a new issue in our `bug tracker <https://github.com/xvitaly/srcrepair/issues>`__.

We support only Source Engine games.

.. index:: building from sources
.. _faq-sources:

Can I build SRC Repair from sources?
========================================

:ref:`Yes, you can <building-from-sources>`.

.. index:: game cleanup
.. _faq-gb-games:

How I can remove garbage, accumulated by installed games?
=============================================================

Please use tools from :ref:`Troubleshooting and cleanup <cleanup-wizard>` tab.

.. index:: steam cleanup
.. _faq-gb-steam:

How I can remove garbage, accumulated by Steam?
===================================================

Please use the :ref:`Steam cleanup module <modules-stmcln>`.

.. index:: installation directory error
.. _faq-install-error:

I want to change the installation directory, but I can't do it because of an error. What should I do?
==========================================================================================================

If you want to install the program in a privileged directory, you need to run the installer as an administrator.

Right-click the installer and select **Run as administrator** from the context menu.

.. index:: open registry key error
.. _faq-reg-error:

How I can fix the "Couldn't open registry key" error?
=========================================================

Please launch the selected game at least once from Steam and then restart SRC Repair.

If this does not help, you have ``-autoconfig`` or ``-dxlevel`` :ref:`command-line options <gt-params>` enabled.

Open the command-line :ref:`options editor <gt-setparams>`, remove everything from this row, then launch the game.

Now SRC Repair will be able to load video settings correctly.

.. index:: graphic settings doesn't work
.. _faq-graph-na:

The game doesn't respect the settings set in the graphic tweaker. How I can fix this?
===========================================================================================

This is a known issue for all Source Engine games if the ``-dxlevel`` :ref:`command-line option <gt-params>` is set.

Open the command-line :ref:`options editor <gt-setparams>`, remove everything from this row, then launch the game.

.. index:: directx 8 mode
.. _faq-gm-dx8:

Garry's Mod can't use DirectX 8.x mode. How can I fix this?
===============================================================

DirectX 8.x support has been removed from recent versions of Garry's Mod. This game will always use DirectX 9.0c, regardless of the selected mode in :ref:`graphic tweaker <gt-type1>`.

.. index:: missing facial animation, missing eyes
.. _faq-tf2-eyes:

How can I fix the issue with missing eyes after installing FPS-config?
============================================================================

  1. Most of FPS-configs disable facial animation to significantly improve game performance.
  2. All FPS-configs are licensed "as is". We can't change them.
  3. You can enable facial animation by switching from ``r_eyes 0`` to ``r_eyes 1``.

.. index:: edit installed FPS-config
.. _faq-fps-edit:

How I can edit the installed FPS-config?
=============================================

If you have installed :ref:`FPS-config <fps-about>`, you will see a yellow exclamation mark in the :ref:`Graphic tweaker <graphic-tweaker>` tab.

Left-click on it and select FPS-config to edit using the :ref:`Config editor <config-editor>` or hold **Shift** to edit it using the :ref:`selected text editor <settings-advanced>`.

.. index:: FPS-configs compatibility
.. _faq-fps-compat:

Why does the description of some FPS-configs say that they are not fully compatible with the game?
=======================================================================================================

Some console variables are marked as cheats and can't be used on servers without ``sv_cheats 1``.

Unfortunately, some FPS-configs authors have abandoned their configs. You can still use them, but their effectiveness may be low.

.. index:: TF2 competitive mode
.. _faq-tf-comp:

Why I can't use FPS-config in a Team Fortress 2 competitive mode?
======================================================================

Valve has completely disabled all FPS-configs in Team Fortress 2's competitive mode.

.. index:: hud download error, FPS-config download error
.. _faq-download-error:

Can't download FPS-config or HUD. How can I fix this?
==========================================================

SRC Repair will automatically try to connect to another server if it can't download a file from the primary CDN.
