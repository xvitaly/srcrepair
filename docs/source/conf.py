# -*- coding: utf-8 -*-

#
# SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
#
# SPDX-License-Identifier: GPL-3.0-or-later
#

# All configuration values have a default; values that are commented out
# serve to show the default.

# Importing some Python modules.
from os import getenv
from time import strftime

# -- General configuration ------------------------------------------------

# If your documentation needs a minimal Sphinx version, state it here.
#
needs_sphinx = '1.6'

# If true, keep warnings as system message paragraphs in the built documents.
#
keep_warnings = False

# Add any Sphinx extension module names here, as strings. They can be
# extensions coming with Sphinx (named 'sphinx.ext.*') or your custom
# ones.
extensions = ['sphinx.ext.todo']

# Add any paths that contain templates here, relative to this directory.
templates_path = ['_templates']

# Localization support.
locale_dirs = ['locale']
gettext_compact = False

# The suffix(es) of source filenames.
# You can specify multiple suffix as a list of string:
#
# source_suffix = ['.rst', '.md']
source_suffix = '.rst'

# The master toctree document.
master_doc = 'index'

# General information about the project.
project = 'SRC Repair'
copyright = '2011 - {}, EasyCoding Team. All rights reserved'.format(strftime('%Y'))
author = 'EasyCoding Team'

# The version info for the project you're documenting, acts as replacement for
# |version| and |release|, also used in various other places throughout the
# built documents.
#
# The short X.Y version.
version = '43.0'
# The full version, including alpha/beta/rc tags.
release = '43.0.4'

# The language for content autogenerated by Sphinx. Refer to documentation
# for a list of supported languages.
#
# This is also used if you do content translation via gettext catalogs.
# Usually you set "language" from the command line for these cases.
envlang = getenv('BUILDLANG')
language = envlang if envlang else 'en'

# Creating dictionaries for localization purposes.
name_dict = {
    'en': 'SRC Repair offline help',
    'ru': 'Справочная система SRC Repair'
}
version_dict = {
    'en': 'version',
    'ru': 'версия'
}

# Generating filename for output files.
filename = 'srcrepair_{}'.format(language)

# List of patterns, relative to source directory, that match files and
# directories to ignore when looking for source files.
# This patterns also effect to html_static_path and html_extra_path
exclude_patterns = ['_build', 'Thumbs.db', '.DS_Store']

# The name of the Pygments (syntax highlighting) style to use.
pygments_style = 'sphinx'

# If true, `todo` and `todoList` produce output, else they produce nothing.
todo_include_todos = False


# -- Options for HTML output ----------------------------------------------

# The theme to use for HTML and HTML Help pages.  See the documentation for
# a list of builtin themes.
#
html_theme = 'alabaster'

# Theme options are theme-specific and customize the look and feel of a theme
# further.  For a list of options available for each theme, see the
# documentation.
#
html_theme_options = {
    'font_family': 'sans-serif',
    'head_font_family': 'serif',
    'font_size': '16px',
    'show_powered_by': False
}

# Overriding default title for HTML and HTML Help pages.
html_title = '{} ({} {})'.format(name_dict[language], version_dict[language], version)

# Add any paths that contain custom static files (such as style sheets) here,
# relative to this directory. They are copied after the builtin static files,
# so a file named "default.css" will overwrite the builtin "default.css".
html_static_path = ['_static']

# Custom sidebar templates, must be a dictionary that maps document names
# to template names.
#
# This is required for the alabaster theme
# refs: http://alabaster.readthedocs.io/en/latest/installation.html#sidebars
html_sidebars = {
    '**': [
        'relations.html',  # needs 'show_related': True theme option to display
        'searchbox.html',
    ]
}

# Other HTML options
html_copy_source = False
html_show_sourcelink = False
html_show_sphinx = False


# -- Options for HTMLHelp output ------------------------------------------

# Output file base name for HTML help builder.
htmlhelp_basename = filename


# -- Options for LaTeX output ---------------------------------------------

latex_engine = 'xelatex'
latex_elements = {
    'fontpkg': r'''
\setmainfont{DejaVu Serif}
\setromanfont{DejaVu Sans}
\setsansfont{DejaVu Sans}
\setmonofont{DejaVu Sans Mono}
'''
}

# Grouping the document tree into LaTeX files. List of tuples
# (source start file, target name, title,
#  author, documentclass [howto, manual, or own class]).
latex_documents = [
    (master_doc, filename, name_dict[language],
     'EasyCoding Team', 'manual'),
]


# -- Options for manual page output ---------------------------------------

# One entry per manual page. List of tuples
# (source start file, name, description, authors, manual section).
man_pages = [
    (master_doc, filename, name_dict[language],
     [author], 1)
]


# -- Options for Texinfo output -------------------------------------------

# Grouping the document tree into Texinfo files. List of tuples
# (source start file, target name, title, author,
#  dir menu entry, description, category)
texinfo_documents = [
    (master_doc, filename, name_dict[language],
     author, filename, name_dict[language],
     'Miscellaneous'),
]
