..
    SPDX-FileCopyrightText: 2011-2024 EasyCoding Team

    SPDX-License-Identifier: GPL-3.0-or-later

.. _config-editor:

************************************
Config editor
************************************

.. index:: about config editor
.. _editor-about:

About config editor
===============================

The Configuration File Editor (config editor) allows you to create, view, or edit any Source Engine game configuration using a simple yet powerful GUI.

.. index:: working with config editor
.. _editor-working:

Working with config editor
==========================================

In the first column **Variable** you must enter the name of the variable (eg. ``cl_cmdrate``), and in the second **Value** -- its value (eg. ``66``). The value can be omitted.

The variable name must not contain spaces, comments, etc. The value can be double-quoted or without them. If the value contains spaces, double quotes are mandatory.

All comments are prohibited and will be removed when saving/loading to reduce the configuration file size and increase its loading speed.

To add a new row, just start typing text in the last cell.

You can select and remove one or more rows by using the **Delete selected row** button on the main toolbar, or by pressing **Delete** on your keyboard.

You can ask the program to show a hint. Select a row and click the **Show hint** button or press **F7** to display a description of the current variable. If no variables were found, an error message will appear.

If you want to edit config as a plain text file, click the **Open config in Notepad** button. The file will be loaded with a :ref:`selected <settings-advanced>` (or system default) text editor.

.. index:: creating new configs
.. _editor-createcfg:

Creating new configs
==========================================

  1. Click the **Create a new config** button on the main toolbar.
  2. Edit the config file as described in the :ref:`working with config editor <editor-working>` section.
  3. Click the **Save config** button on the main toolbar to save it to disk. If it was a new file, a save dialog will appear. The file extension is optional and will be added automatically.

.. index:: loading config file
.. _editor-loadcfg:

Loading config file
================================================

  1. Click the **Open config from file** button on the main toolbar and select the desired file on disk in the standard file open dialog box (platform dependent).
  2. Edit the config file as described in the :ref:`working with config editor <editor-working>` section.
  3. Click the **Save config** button on the main toolbar to save the changes to disk.

If safe clean is enabled (green light in the status bar), a backup file will be created automatically. You can restore or delete it on :ref:`BackUps <backups-about>` tab.

.. index:: useful information about config editor
.. _editor-other:

Other useful information
================================================

If you :ref:`create a new <editor-createcfg>` or :ref:`load an existing <editor-loadcfg>` config, the current editor content will be lost. Save it before performing these actions.

If you want to close current config file and free up its resources, simply open another one or click the the **Create a new config** button.
