/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2020 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2020 EasyCoding Team.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
*/
using System;

namespace srcrepair.core
{
    /// <summary>
    /// Class with factory methods to work with video settings.
    /// </summary>
    public static class VideoFactory
    {
        /// <summary>
        /// Creates video settings class instance.
        /// </summary>
        /// <param name="Argument">Agrument for constructor.</param>
        public static CommonVideo Create(string Argument, string SourceType)
        {
            switch (SourceType)
            {
                case "1":
                    return new Type1Video(Argument);
                case "2":
                    return new Type2Video(Argument);
                case "4":
                    return new Type4Video(Argument);
                default:
                    throw new Exception("AA");
            }
        }
    }
}
