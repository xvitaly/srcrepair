/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 *
 * Copyright (c) 2011 - 2021 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2021 EasyCoding Team.
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

namespace srcrepair.gpg
{
    /// <summary>
    /// Class for working with GPG cryptographic functions.
    /// </summary>
    public static class GPGManager
    {
        /// <summary>
        /// Encrypts file with pre-defined GPG public key.
        /// </summary>
        /// <param name="FileName">Full path to the source file.</param>
        public static void EncryptFile(string FileName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifies the GPG-signature of the file.
        /// </summary>
        /// <param name="FileName">Full path to the detached signature file.</param>
        /// <returns>Returns True if signature check passed.</returns>
        public static bool VerifySignedFile(string FileName)
        {
            throw new NotImplementedException();
        }
    }
}
