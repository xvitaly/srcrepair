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
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Text;

namespace srcrepair.gpg
{
    /// <summary>
    /// Class for working with GPG cryptographic functions.
    /// </summary>
    public static class GPGManager
    {
        /// <summary>
        /// Gets and returns GPG public key instance.
        /// </summary>
        /// <returns>Returns GPG public key instance.</returns>
        private static PgpPublicKey GetPublicKey()
        {
            PgpPublicKey Result = null;
            using (MemoryStream KeyStream = new MemoryStream(Encoding.ASCII.GetBytes(Properties.Resources.GPGPublicKey)))
            using (Stream DecoderStream = PgpUtilities.GetDecoderStream(KeyStream))
            {
                PgpPublicKeyRingBundle RingBundle = new PgpPublicKeyRingBundle(DecoderStream);
                foreach (PgpPublicKeyRing Ring in RingBundle.GetKeyRings())
                {
                    foreach (PgpPublicKey Key in Ring.GetPublicKeys())
                    {
                        if (Key.IsEncryptionKey && !Key.IsRevoked())
                        {
                            return Key;
                        }
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// Encrypts file with pre-defined GPG public key.
        /// </summary>
        /// <param name="SourceFileName">Full path to the source file.</param>
        /// <param name="DestinationFileName">Full path to the destination file.</param>
        public static void EncryptFile(string SourceFileName, string DestinationFileName)
        {
            PgpEncryptedDataGenerator EncryptedDataGenerator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Aes256, true, new SecureRandom());
            PgpCompressedDataGenerator CompressedDataGenerator = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);
            PgpLiteralDataGenerator LiteralDataGenerator = new PgpLiteralDataGenerator();
            EncryptedDataGenerator.AddMethod(GetPublicKey());

            using (FileStream InputFileStream = new FileStream(SourceFileName, FileMode.Open))
            using (FileStream OutputFileStream = new FileStream(DestinationFileName, FileMode.Create))
            using (Stream EncryptedDataGeneratorStream = EncryptedDataGenerator.Open(OutputFileStream, new byte[4096]))
            using (Stream CompressedDataGeneratorStream = CompressedDataGenerator.Open(EncryptedDataGeneratorStream))
            using (Stream LiteralDataGeneratorStream = LiteralDataGenerator.Open(CompressedDataGeneratorStream, PgpLiteralData.Binary, String.Empty, InputFileStream.Length, DateTime.Now))
            {
                InputFileStream.CopyTo(LiteralDataGeneratorStream);
            }
        }

        /// <summary>
        /// Encrypts file with pre-defined GPG public key.
        /// </summary>
        /// <param name="SourceFileName">Full path to the source file.</param>
        public static void EncryptFile(string SourceFileName)
        {
            EncryptFile(SourceFileName, SourceFileName + ".gpg");
        }

        /// <summary>
        /// Verifies the GPG-signature of the file.
        /// </summary>
        /// <param name="SignatureFileName">Full path to the detached signature file.</param>
        /// <param name="SourceFileName">Full path to the source file.</param>
        /// <returns>Returns True if signature check passed.</returns>
        public static bool VerifySignedFile(string SignatureFileName, string SourceFileName)
        {
            using (FileStream SignatureFileStream = new FileStream(SignatureFileName, FileMode.Open))
            using (Stream SignatureFileDecoderStream = PgpUtilities.GetDecoderStream(SignatureFileStream))
            {
                PgpObjectFactory ObjectFactory = new PgpObjectFactory(SignatureFileDecoderStream);
                if (!(ObjectFactory.NextPgpObject() is PgpSignatureList SignatureList)) throw new InvalidOperationException("Failed to create an instance of the PgpObjectFactory.");
                SignatureList[0].InitVerify(GetPublicKey());
                using (FileStream SourceFileStream = new FileStream(SourceFileName, FileMode.Open))
                using (BufferedStream BufferedSourceFileStream = new BufferedStream(SourceFileStream))
                {
                    const int BlockSize = 4096;
                    byte[] Buffer = new byte[BlockSize];
                    int BytesRead = 0;
                    while ((BytesRead = BufferedSourceFileStream.Read(Buffer, 0, BlockSize)) != 0)
                    {
                        SignatureList[0].Update(Buffer, 0, BytesRead);
                    }
                }
                return SignatureList[0].Verify();
            }
        }

        /// <summary>
        /// Verifies the GPG-signature of the file.
        /// </summary>
        /// <param name="SignatureFileName">Full path to the detached signature file.</param>
        /// <returns>Returns True if signature check passed.</returns>
        public static bool VerifySignedFile(string SignatureFileName)
        {
            return VerifySignedFile(SignatureFileName, Path.Combine(Path.GetDirectoryName(SignatureFileName), Path.GetFileNameWithoutExtension(SignatureFileName)));
        }
    }
}
