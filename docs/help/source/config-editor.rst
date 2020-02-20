.. _config-editor:

************************************
Config editor
************************************

Game configuration file editor (config editor) allow you easily create, view or edit any Source Engine game configs in a simple, but powerful GUI.

.. index:: config editor, working with config editor
.. _editor-working:

Working with config editor
==========================================

In the first column **Variable** you must specify variable name (eg. ``cl_cmdrate``), and in the second **Value** -- its value (eg. ``66``).

Variable name must not contain any spaces, commentaries, etc. Value can be both double-quoted, or without them. If value contains spaces, double quotes are mandatory.

All commentaries are forbidden and will be removed on save/load procedures to reduce config file size and increase its loading speed.

To add a new row, just start typing text in the last cell.

To remove currently selected row, press **Delete selected row** button on main toolbar, or press **Delete** on keybooard. You can select and remove multiple rows at once.

You can ask program to show hint. Select row and press **Show hint** button or press **F7** to show description of current variable. If no variables found, an error message will be shown.

If you want to edit config as text, press **Open config in Notepad** button. File will be loaded into :ref:`selected <settings-advanced>` (or system default) text editor.

.. index:: config editor, creating new configs
.. _editor-createcfg:

Creating new configs
==========================================

  1. Press **Create a new config** button on main toolbar.
  2. Edit config as described in :ref:`working with config editor <editor-working>`.
  3. Press **Save config** button on main toolbar to save changes in config file. If it was a new file, save as dialog will be shown. Extension is not required.

.. index:: config editor, loading config file
.. _editor-loadcfg:

Loading config file
================================================

  1. Press **Open config from file** button on main toolbar and select file on disk with standard open file dialog (depends on platform).
  2. Edit config as described in :ref:`working with config editor <editor-working>`.
  3. Press **Save config** button on main toolbar to save changes in config file.

If safe clean is enabled (green light in status bar), backup file will be created automatically. You can restore or delete in on :ref:`BackUps <backups-about>` tab.

.. index:: config editor, useful information
.. _editor-other:

Other useful information
================================================

If you :ref:`create a new <editor-createcfg>` or :ref:`load config <editor-loadcfg>`, all existing contents of editor will be removed. Save it before performing this actions.

If you want to close current config file and free its resources, just open another one or press **Create a new config** button.
