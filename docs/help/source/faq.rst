.. _faq:

*****************************************
Frequently asked questions (FAQ)
*****************************************

.. index:: faq, framework, dotnet version
.. _faq-framework:

How to fix error with Microsoft .NET Framework 4 on program startup?
===========================================================================================

You need installed Microsoft .NET Framework version 4.7.2 or higher in order to start this program.

You can download if from `official Microsoft website <https://www.microsoft.com/net/download/dotnet-framework-runtime>`__.

.. index:: faq, windows xp, windows vista
.. _faq-legacy:

How can I run program under Windows XP/Vista?
=========================================================

These legacy operating systems are no longer supported.

.. index:: faq, slow startup
.. _faq-slow-start:

How to fix slow startup?
===========================================

You can install SRC Repair for all users. Installer will use NGen to generate machine images, which should speed-up program cold startup.

.. index:: faq, counterfeit, cracked versions
.. _faq-crrrr:

Can I use program with cracked versions of games?
=========================================================

No. We support only legal Steam versions of games.

.. index:: faq, source 2
.. _faq-source2:

Will the Source 2 engine be supported and when?
=======================================================

Maybe, but no ETA yet.

.. index:: faq, source 2, dota 2
.. _faq-dota2:

Is Dota 2 supported?
===================================

This game uses the Source 2 engine, which is not supported yet.

.. index:: faq, steam account, steam password, steam
.. _faq-password:

Can this program steal my Steam account?
============================================

No. SRC Repair is a free and opensource program. You can check its `source code on GitHub <https://github.com/xvitaly/srcrepair>`__.

.. index:: faq, steam, simultaneous launch
.. _faq-steam-run:

Is it safe to run this program with Steam simultaneously?
===============================================================

Yes, unless you use :ref:`Steam cleanup module <modules-stmcln>`.

.. index:: faq, simultaneous launch
.. _faq-game-run:

Is it safe to run this program with controlled game simultaneously?
===========================================================================

We strongly do not recommend doing this, as SRC Repair cannot get exclusive access to its files and settings.

.. index:: faq, vac ban
.. _faq-vac:

Can I get VAC ban for using this program?
=======================================================

No, but do not use this program with any running VAC-protected game.

.. index:: faq, report issue, report bug, feature request
.. _faq-opinion:

How I can report bug or create a new feature request?
===============================================================

Please open a new issue in our `GitHub bug tracker <https://github.com/xvitaly/srcrepair/issues>`__.

.. index:: faq, firewall, network activity
.. _faq-firewall:

SRC Repair require Internet access. For what purpose?
===================================================================================================

SRC Repair need Internet access for checking for updates once a week (can be disabled in :ref:`advanced settings <settings-advanced>`).

.. index:: faq, adding a new game, report issue
.. _faq-add-game:

I want to add a new game. What should I do?
===========================================================

Please open a new issue in our `GitHub bug tracker <https://github.com/xvitaly/srcrepair/issues>`__.

We support only Source Engine games.

.. index:: faq, building from sources
.. _faq-sources:

Can I build SRC Repair from sources?
=============================================

`Yes, you can <https://github.com/xvitaly/srcrepair/blob/dev/docs/building-from-sources.md>`__.

.. index:: faq, cleanup
.. _faq-gb-games:

How I can remove garbage, accumulated by installed games?
================================================================================================================

Please use tools from :ref:`Troubleshooting and cleanup <cleanup-wizard>` tab.

.. index:: faq, cleanup
.. _faq-gb-steam:

How I can remove garbage, accumulated by Steam?
=========================================================================================================

Please use :ref:`Steam cleanup module <modules-stmcln>`.

.. index:: faq, installation error, error
.. _faq-install-error:

I want to change installation directory, but cannot do it due to error. What shall I do?
====================================================================================================

If you want to install program to a privileged directory, you will need to run installer with admininstrator rights.

Press right mouse button on installer and select **Run as admininstrator** from context menu.

.. index:: faq, error
.. _faq-reg-error:

How I can fix "Could not open registry key" error?
==============================================================================================

Please start selected game at least once from Steam, then restart SRC Repair.

If that does not help, you have ``-autoconfig`` or ``-dxlevel`` :ref:`command-line options <gt-params>` enabled.

Open command-line :ref:`options editor <gt-setparams>`, remove everything from this row, then launch the game.

Now SRC Repair will load video settings corretly.

.. index:: faq, error
.. _faq-graph-na:

Game do not respect settings set in graphic tweaker. How I can fix this?
==================================================================================

This is a known issue for all Source Engine games if ``-dxlevel`` :ref:`command-line option <gt-params>` is set.

Open command-line :ref:`options editor <gt-setparams>`, remove everything from this row, then launch the game.

.. index:: faq, error
.. _faq-gm-dx8:

Garry's Mod cannot use DirectX 8.x mode. How can I fix this?
================================================================================

DirectX 8.x support was removed from modern Garry's Mod versions. This game will use DirectX 9.0c, regardless of selected mode in :ref:`graphic tweaker <gt-type1>`.

.. index:: faq, missing face animation, missing eyes, FPS-configs
.. _faq-tf2-eyes:

Missing eyes after installing FPS-config in Team Fortress 2. How can I fix this?
=========================================================================================

  1. Most of FPS-configs disable face animations in order to significantly increase game performance.
  2. All FPS-configs are licensed "as is". We cannot change them.
  3. You can enable face animations by switching from ``r_eyes 0`` to ``r_eyes 1``.

.. index:: faq, FPS-configs, FPS-config edit
.. _faq-fps-edit:

How I can edit installed FPS-config?
=====================================================

If you have installed :ref:`FPS-config <fps-about>`, you will see a yellow exclamation mark in :ref:`Graphic tweaker <graphic-tweaker>` tab.

Press left mouse button on it and select FPS-config to edit in :ref:`Config editor <config-editor>` or hold **Shift** to load it with :ref:`selected text editor <settings-advanced>`.

.. index:: faq, FPS-configs, FPS-configs compatibility
.. _faq-fps-compat:

Why does the description of some FPS-configs say that they are not fully compatible with the game?
=======================================================================================================

Some console variables we marked as cheats and cannot be used on servers without ``sv_cheats 1``.

Unfortunately, many authors of FPS-configs have abandoned their configs. You can still use them, but their effectiveness may be low.

.. index:: faq, FPS-configs, TF2 competitive mode
.. _faq-tf-comp:

Why I cannot use FPS-config in a Team Fortress 2 competitive mode?
==========================================================================================

Valve have completely disabled all FPS-configs in Team Fortress 2 competitive mode.

.. index:: faq, FPS-configs, hud, download error, FPS-config download error, hud download error
.. _faq-download-error:

Cannot download FPS-config or HUD. How can I fix this?
===================================================================

Please open :ref:`program settings <settings>` and enable following checkboxes:

  * **Allow download and install latest (untested) HUDs** (:ref:`common settings <settings-main>` tab) -- should fix issues with downloading HUD files;
  * **Use mirrors to download FPS-configs** (:ref:`advanced settings <settings-advanced>` tab) -- should fix issues with downloading FPS-configs.
